//--------------------------------------------------------------------------------------------------------------------- 
// <copyright file="StarredLinkerPlugin.cs" company="Bit Tappers">
//   Copyright by Alvin Ashcraft 2009 - Bit Tappers.
// </copyright>
// <summary>
//   Defines the StarredLinkerPlugin type.
// </summary>
//---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using WindowsLive.Writer.Api;

using AlvinAshcraft.LinkBuilder.Contracts;
using AlvinAshcraft.LinkBuilder.Helpers;

namespace AlvinAshcraft.LinkBuilder
{
    /// <summary>
    /// WLW Plugin to generate links from a shared feed.
    /// </summary>
    [WriterPlugin("61B83824-ADEA-401d-86A1-87282C425E37", "Insert Shared Links", ImagePath = "DEW_Logo128.png", PublisherUrl = "http://www.alvinashcraft.com", Description = "A plugin to insert shared links from NewsBlur or an RSS/ATOM feed.", HasEditableOptions = true)]
    [InsertableContentSource("Insert Shared Links")]
    public class StarredLinkerPlugin : ContentSource
    {
        /// <summary>
        /// Stores the options for the plugin.
        /// </summary>
        private PluginSettings _options;
        
        /// <summary>
        /// Initializes the specified plugin options.
        /// </summary>
        /// <param name="pluginOptions">The plugin options.</param>
        public override void Initialize(IProperties pluginOptions)
        {
            base.Initialize(pluginOptions);
            _options = new PluginSettings(pluginOptions);
        }

        /// <summary>
        /// Edits the options.
        /// </summary>
        /// <param name="dialogOwner">The dialog owner.</param>
        public override void EditOptions(IWin32Window dialogOwner)
        {
            using (var of = new OptionsForm(_options))
            {
                of.ShowDialog(dialogOwner);
            }
        }

        /// <summary>
        /// Creates the content.
        /// </summary>
        /// <param name="dialogOwner">The dialog owner.</param>
        /// <param name="newContent">The new content.</param>
        /// <returns>The content to insert in the document.</returns>
        public override DialogResult CreateContent(IWin32Window dialogOwner, ref string newContent)
        {
            using (new CursorKeeper(Cursors.WaitCursor))
            {
                var result = MessageBox.Show("Your latest shared links will be inserted. Continue?", "Insert Shared Links", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    newContent = _options.FeedTypeOption.ToLower() == "newsblur api" ? BuildLinksFromNewsBlur() : BuildLinks(_options.FeedTypeOption.ToLower() == "rss 2.0" ? (SyndicationFeedFormatter)new Rss20FeedFormatter() : new Atom10FeedFormatter());
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <param name="fileExtension">The file extension.</param>
        /// <returns>A list of FileInfo objects matching the given file extension.</returns>
        private static IEnumerable<FileInfo> GetFiles(string path, string fileExtension)
        {
            var dirInfo = new DirectoryInfo(path);
            return dirInfo.GetFiles(String.Format("*{0}", fileExtension), SearchOption.TopDirectoryOnly).ToList();
        }

        /// <summary>
        /// Builds the links from an Atom feed.
        /// </summary>
        /// <param name="feedFormatter">The feed formatter.</param>
        /// <returns>A string containing the generated links.</returns>
        private string BuildLinks(SyndicationFeedFormatter feedFormatter)
        {
            string newContent = String.Empty;

            using (var reader = XmlReader.Create(String.Format("{0}?n={1}", _options.FeedUrlOption, _options.MaxPostsOption)))
            {
                if (feedFormatter == null || !feedFormatter.CanRead(reader))
                {
                    return newContent;
                }

                feedFormatter.ReadFrom(reader);

                var items =
                    feedFormatter.Feed.Items.OfType<SyndicationItem>()
                        .Where(item => item.LastUpdatedTime.DateTime > GetLastBlogDate().AddHours(_options.BufferOption * -1))
                        .OrderBy(item => item.Authors.FirstOrDefault().Name)
                        .ThenBy(item => item.LastUpdatedTime.DateTime)
                        .Select(item =>
                                new BlogEntry
                                {
                                    Title = item.Title.Text,
                                    Url = item.Links.Count > 0
                                            ? item.Links.FirstOrDefault().Uri.AbsoluteUri
                                            : String.Empty,
                                    Author = item.Authors.FirstOrDefault().Name
                                });

                newContent = BuildContent(items.ToList());

                return newContent;
            }
        }
  
        /// <summary>
        /// Builds the content.
        /// </summary>
        /// <param name="blogEntries">The blog entries.</param>
        /// <returns></returns>
        private string BuildContent(List<BlogEntry> blogEntries)
        {
            foreach (BlogEntry entry in blogEntries)
            {
                entry.AuthorInfo = GetBlogAuthor(entry.Author, entry.Url);

                if (entry.AuthorInfo.DefaultCategory.Name == "misc")
                {
                    entry.AuthorInfo.DefaultCategory = GetPostCategory(entry);
                }
            }

            var categories = NameAndGetHeadings();
            var linkListing = new StringBuilder();

            categories.ForEach(category =>
            {
                linkListing.Append(String.Format("<h3><a name=\"{0}\"></a>{1}</h3>", category.Name, category.Caption));

                foreach (BlogEntry entry in blogEntries.Where(e => e.AuthorInfo.DefaultCategory.Name == category.Name))
                {
                    string url = entry.Url;

                    url = AppendMvpIdToUrl(url);

                    linkListing.Append(String.Format("<p><a href=\"{0}\" target=\"_blank\">{1}</a> ({2})</p>", url, entry.Title, entry.AuthorInfo.AuthorName));
                }

                linkListing.Append("<p>&#160;</p>");
            });

            return linkListing.ToString();
        }

        /// <summary>
        /// Appends an MVP identifier to the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>System.String.</returns>
        private static string AppendMvpIdToUrl(string url)
        {
            if (url.Contains("WT.mc_id=rss_alldownloads_all"))
                url = url.Replace("WT.mc_id=rss_alldownloads_all", "WT.mc_id=DX_MVP4025064");

            if ((url.Contains("blogs.msdn.com") || url.Contains("channel9.msdn.com") 
                || url.Contains("blogs.windows.com") || url.Contains("weblogs.asp.net")) 
                && !url.Contains("?"))
            {
                url = url + "?WT.mc_id=DX_MVP4025064";
            }
            return url;
        }

        /// <summary>
        /// Builds the links from newsblur.
        /// </summary>
        /// <returns></returns>
        private string BuildLinksFromNewsBlur()
        {
            const string newsBlurBaseUrl = "http://www.newsblur.com";
            var client = new RestClient(newsBlurBaseUrl);
            string restResource = _options.FeedUrlOption.Replace(String.Format("{0}/", newsBlurBaseUrl), String.Empty);
            client.Authenticator = new HttpBasicAuthenticator("alvinashcraft", "Po02zxn#");
            var request = new RestRequest(String.Format("{0}?limit={1}", restResource, _options.MaxPostsOption), Method.GET);

            IRestResponse response = client.Execute(request);

            var stories = JsonConvert.DeserializeObject<Rootobject>(response.Content).stories;

            var storyList = stories.Where(s => DateTime.Parse(s.story_date) > GetLastBlogDate().AddHours(_options.BufferOption * -1))
                                            .OrderBy(s => s.story_authors)
                                            .ThenBy(s => s.story_date).ToList();

            var blogEntries = storyList.Select(story => new BlogEntry
                {
                    Author = story.story_authors, Title = story.story_title, Url = story.story_permalink
                }).ToList();

            return BuildContent(blogEntries);
        }

        /// <summary>
        /// Gets the post category.
        /// </summary>
        /// <param name="entry">The blog entry.</param>
        /// <returns></returns>
        private static Category GetPostCategory(BlogEntry entry)
        {
            return LookupHelper.GetCategoryByKeyword(entry.Title, entry.AuthorInfo.DefaultCategory);
        }

        /// <summary>
        /// Gets the blog author.
        /// </summary>
        /// <param name="author">The author.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        private static AuthorResult GetBlogAuthor(string author, string url)
        {
            author = author ?? String.Empty;
            url = url ?? String.Empty;

            string lowerAuthor = author.ToLower();
            string lowerUrl = url.ToLower();

            AuthorResult authorResult = CheckAuthorContains(lowerAuthor);

            if (!String.Equals(lowerAuthor, authorResult.AuthorName)) return authorResult;

            authorResult = FindExactAuthorMatch(lowerAuthor);

            if (!String.Equals(lowerAuthor, authorResult.AuthorName)) return authorResult;

            authorResult = CheckAuthorAndUrl(lowerAuthor, lowerUrl);

            if (!String.Equals(lowerAuthor, authorResult.AuthorName)) return authorResult;

            authorResult = CheckUrlContains(lowerAuthor, lowerUrl);

            if (!String.Equals(lowerAuthor, authorResult.AuthorName)) return authorResult;

            if (!String.IsNullOrEmpty(author) && !lowerAuthor.Contains("unknown"))
                return new AuthorResult(author, new Category());

            return new AuthorResult(lowerAuthor.Contains("unknown") ? "Unknown Author" : author, new Category());
        }

        /// <summary>
        /// Checks the author and URL.
        /// </summary>
        /// <param name="authorName">Name of the author.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        private static AuthorResult CheckAuthorAndUrl(string authorName, string url)
        {
            switch (authorName)
            {
                case "don":
                    if (url.Contains("d4dotnet")) return new AuthorResult("Don Burnett", new Category(CategoryType.Xaml));
                    break;
                case "brian":
                    if (url.Contains("dbug")) return new AuthorResult("Brian Reindel", new Category());
                    break;
                case "charles":
                    if (url.Contains("channel9")) return new AuthorResult("Charles Torre", new Category(CategoryType.Podcasts));
                    break;
                case "arvind":
                    if (url.Contains("zoho")) return new AuthorResult("Arvind Natarajan", new Category());
                    break;
                case "christophe":
                    if (url.Contains("runningagile")) return new AuthorResult("Christophe Louvion", new Category(CategoryType.Design));
                    break;
                case "kathleen":
                    if (url.Contains("msmvps.com/blogs/kathleen")) return new AuthorResult("Kathleen Dollard", new Category(CategoryType.DotNet));
                    break;
                case "jd":
                    if (url.Contains("sourcesofinsight")) return new AuthorResult("J.D. Meier", new Category(CategoryType.Design));
                    break;
                case "blaine":
                    if (url.Contains("blogs.msdn.com/blaine")) return new AuthorResult("Blaine Wastell", new Category(CategoryType.Miscellaneous));
                    break;
                case "clarance":
                    if (url.Contains("zoho")) return new AuthorResult("Clarence Rozario", new Category());
                    break;
                case "daily":
                    if (url.Contains("onsaas")) return new AuthorResult("Jian Zhen", new Category(CategoryType.Links));
                    break;
                case "davembush":
                    return new AuthorResult("Dave M. Bush", new Category(CategoryType.WebDevelopment));
                case "major nelson":
                    return new AuthorResult("Larry Hyrb", new Category(CategoryType.Podcasts));
                case "scott":
                    if (url.Contains("roguetechnology")) return new AuthorResult("Scott Banwart", new Category(CategoryType.Links));
                    break;
                case "tobias":
                    if (url.Contains("powershell")) return new AuthorResult("Tobias Weltner", new Category(CategoryType.PowerShell));
                    break;
                case "alexanderb":
                    if (url.Contains("dzone")) return new AuthorResult("Alexander Beletsky", new Category(CategoryType.WebDevelopment));
                    break;
            }

            if (url.Contains("dmbcllc")) return new AuthorResult("Dave M. Bush", new Category(CategoryType.WebDevelopment));
            if (url.Contains("majornelsonblogcast")) return new AuthorResult("Larry Hyrb", new Category(CategoryType.Podcasts));

            return new AuthorResult(authorName, new Category());
        }

        /// <summary>
        /// Checks if the authorname field contains particular text and returns the correct name.
        /// </summary>
        /// <param name="authorName">Name of the author.</param>
        /// <returns></returns>
        private static AuthorResult CheckAuthorContains(string authorName)
        {
            return LookupHelper.GetAuthorInfoByName(authorName, false);
        }

        /// <summary>
        /// Finds the exact author match.
        /// </summary>
        /// <param name="authorName">Name of the author.</param>
        /// <returns></returns>
        private static AuthorResult FindExactAuthorMatch(string authorName)
        {
            return LookupHelper.GetAuthorInfoByName(authorName, true);
        }

        /// <summary>
        /// Checks the URL contains.
        /// </summary>
        /// <param name="authorName">Name of the author.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        private static AuthorResult CheckUrlContains(string authorName, string url)
        {
            if (url.Contains("bhandler")) return new AuthorResult("Blake Handler", new Category());
            if (url.Contains("dougholland")) return new AuthorResult("Doug Holland", new Category());
            if (url.Contains("bennadel")) return new AuthorResult("Ben Nadel", new Category(CategoryType.WebDevelopment));
            if (url.Contains("paulhammant")) return new AuthorResult("Paul Hammant", new Category(CategoryType.WebDevelopment));
            if (url.Contains("bleroy")) return new AuthorResult("Bertrand Le Roy", new Category(CategoryType.WebDevelopment));
            if (url.Contains("mike-ward")) return new AuthorResult("Mike Ward", new Category());
            if (url.Contains("msdevshow")) return new AuthorResult("Jason Young & Carl Schweitzer", new Category(CategoryType.Podcasts));
            if (url.Contains("mikeormond")) return new AuthorResult("Mike Ormond", new Category());
            if (url.Contains("agilescout")) return new AuthorResult("Peter Saddington", new Category(CategoryType.Design));
            if (url.Contains("codinghorror")) return new AuthorResult("Jeff Atwood", new Category());
            if (url.Contains("devhammer")) return new AuthorResult("G. Andrew Duthie", new Category(CategoryType.Community));
            if (url.Contains("codingthearchitecture")) return new AuthorResult("Simon Brown", new Category(CategoryType.Design));
            if (url.Contains("www.wduffy.co.uk")) return new AuthorResult("William Duffy", new Category());
            if (url.Contains("pixel8")) return new AuthorResult("Craig Shoemaker", new Category(CategoryType.Podcasts));
            if (url.Contains("geekswithblogs.net/ranganh")) return new AuthorResult("Harish Ranganathan", new Category());
            if (url.Contains("maartenballiauw")) return new AuthorResult("Maarten Balliauw", new Category(CategoryType.WebDevelopment));
            if (url.Contains("love2dev")) return new AuthorResult("Chris Love", new Category(CategoryType.WebDevelopment));
            if (url.Contains("electricbeach")) return new AuthorResult("Christian Schormann", new Category(CategoryType.Xaml));
            if (url.Contains("4guysfrom")) return new AuthorResult("Scott Mitchell", new Category(CategoryType.WebDevelopment));
            if (url.Contains("highoncoding.com")) return new AuthorResult("Mohammad Azam", new Category());
            if (url.Contains("charliedigital")) return new AuthorResult("Charles Chen", new Category());
            if (url.Contains("jamesshore")) return new AuthorResult("James Shore", new Category(CategoryType.Design));
            if (url.Contains("nikhilk")) return new AuthorResult("Nikhil Kothari", new Category(CategoryType.Xaml));
            if (url.Contains("windowsphonegeek")) return new AuthorResult("Windows Phone Geek", new Category(CategoryType.Xaml));
            if (url.Contains("deepfriedbytes")) return new AuthorResult("Keith Elder & Chris Woodruff", new Category(CategoryType.Podcasts));
            if (url.Contains("wekeroad")) return new AuthorResult("Rob Conery", new Category(CategoryType.WebDevelopment));
            if (url.Contains("peterkellnernet")) return new AuthorResult("Peter Kellner", new Category());
            if (url.Contains("ayende")) return new AuthorResult("Oren Eini", new Category(CategoryType.DotNet));
            if (url.Contains("reedcopsey")) return new AuthorResult("Reed Copsey", new Category(CategoryType.DotNet));
            if (url.Contains("roryprimrose")) return new AuthorResult("Rory Primrose", new Category(CategoryType.DotNet));
            if (url.Contains("microsoft.com/events/podcasts")) return new AuthorResult("MS Podcasts", new Category(CategoryType.Podcasts));
            if (url.Contains("sqlskills.com")) return new AuthorResult("Paul S. Randal", new Category(CategoryType.Sql));
            if (url.Contains("dotnetkicks")) return new AuthorResult("DotNetKicks", new Category());
            if (url.Contains("microsoftdownload") || url.Contains("msdownloads")) return new AuthorResult("MS Downloads", new Category(CategoryType.Miscellaneous));
            if (url.Contains("lazycoder")) return new AuthorResult("Scott Koon", new Category());
            if (url.Contains("kirupa")) return new AuthorResult("Kirupa Chinnathambi", new Category(CategoryType.WebDevelopment));
            if (url.Contains("grantpalin")) return new AuthorResult("Grant Palin", new Category(CategoryType.Links));
            if (url.Contains("webbtechsolutions")) return new AuthorResult("Joe Webb", new Category(CategoryType.Sql));
            if (url.Contains("thedatafarm")) return new AuthorResult("Julie Lerman", new Category(CategoryType.DotNet));
            if (url.Contains("firstfloorsoftware")) return new AuthorResult("Koen Zwikstra", new Category(CategoryType.Xaml));
            if (url.Contains("mistergoodcat")) return new AuthorResult("Peter Kuhn", new Category(CategoryType.Xaml));
            if (url.Contains("rudigrobler")) return new AuthorResult("Rudi Grobler", new Category(CategoryType.Xaml));
            if (url.Contains("japf.fr")) return new AuthorResult("Jeremy Alles", new Category(CategoryType.Xaml));
            if (url.Contains("stevegilham")) return new AuthorResult("Steve Gilham", new Category());
            if (url.Contains("jasonbock")) return new AuthorResult("Jason Bock", new Category(CategoryType.DotNet));
            if (url.Contains("hanselminutes")) return new AuthorResult("Scott Hanselman", new Category(CategoryType.Podcasts));
            if (url.Contains("programmingtour")) return new AuthorResult("Corey Haines", new Category(CategoryType.Design));
            if (url.Contains("paul-zubkov")) return new AuthorResult("Paul Zubkov", new Category());
            if (url.Contains("thekua")) return new AuthorResult("Patrick Kua", new Category(CategoryType.Design));
            if (url.Contains("kevingriffin")) return new AuthorResult("Kevin Griffin", new Category());
            if (url.Contains("robmiles")) return new AuthorResult("Rob Miles", new Category());
            if (url.Contains("ariankulp")) return new AuthorResult("Arian Kulp", new Category());
            if (url.Contains("gweek.libsyn")) return new AuthorResult("Mark Frauenfelder", new Category(CategoryType.Podcasts));
            if (url.Contains("windowsphonedevpodcast")) return new AuthorResult("Ryan Lowdermilk & Travis Lowdermilk", new Category(CategoryType.Podcasts));
            if (url.Contains("netcave.org")) return new AuthorResult("Alan Stevens", new Category());
            if (url.Contains("codingqa")) return new AuthorResult("Mark Berryman & Jim Wang", new Category(CategoryType.Podcasts));
            if (url.Contains("wpf.2000things")) return new AuthorResult("Sean Sexton", new Category(CategoryType.Xaml));
            if (url.Contains("developingfor.net")) return new AuthorResult("Joel Cochran", new Category(CategoryType.Xaml));
            if (url.Contains("martinfowler.com")) return new AuthorResult("Martin Fowler", new Category(CategoryType.Design));
            if (url.Contains("kirkk.com")) return new AuthorResult("Kirk Knoernschild", new Category(CategoryType.Design));
            if (url.Contains("dotnetrocks")) return new AuthorResult("Carl Franklin & Richard Campbell", new Category(CategoryType.Podcasts));
            if (url.Contains("nerdyhearn")) return new AuthorResult("Tom Hearn", new Category(CategoryType.WebDevelopment));
            if (url.Contains("jsmag")) return new AuthorResult("Michael Kimsal", new Category(CategoryType.WebDevelopment));
            if (url.Contains("tess") && url.Contains("msdn")) return new AuthorResult("Tess Ferrandez", new Category(CategoryType.DotNet));
            if (url.Contains("whereslou")) return new AuthorResult("Louis DeJardin", new Category(CategoryType.WebDevelopment));
            if (url.Contains("dotnet.org.za/rudi") || url.Contains("rudigrobler")) return new AuthorResult("Rudi Grobler", new Category(CategoryType.Xaml));
            if (url.Contains("platinumbay")) return new AuthorResult("Steve Andrews", new Category(CategoryType.DotNet));
            if (url.Contains("rtipton.wordpress")) return new AuthorResult("Rhonda Tipton", new Category(CategoryType.Links));
            if (url.Contains("coderant")) return new AuthorResult("Mike Hadlow", new Category(CategoryType.DotNet));
            if (url.Contains("herdingcode")) return new AuthorResult("Jon Galloway", new Category(CategoryType.Podcasts));
            if (url.Contains("coolthingoftheday")) return new AuthorResult("Greg Duncan", new Category());
            if (url.Contains("charlespetzold")) return new AuthorResult("Charles Petzold", new Category(CategoryType.Xaml));
            if (url.Contains("blackwasp")) return new AuthorResult("Richard Carr", new Category(CategoryType.Xaml));
            if (url.Contains("trelford")) return new AuthorResult("Phillip Trelford", new Category());
            if (url.Contains("rthand.com")) return new AuthorResult("Miha Markic", new Category(CategoryType.DotNet));
            if (url.Contains("openmymind.net")) return new AuthorResult("Karl Seguin", new Category(CategoryType.WebDevelopment));
            if (url.Contains("andrewconnell")) return new AuthorResult("Andrew Connell", new Category(CategoryType.SharePoint));
            if (url.Contains("windowsappdev.com") || url.Contains("allaboutxamarin.com")) return new AuthorResult("Dan Rigby", new Category(CategoryType.Links));

            return new AuthorResult(authorName, new Category());
        }

        /// <summary>
        /// Gets the last blog date.
        /// </summary>
        /// <returns>Date/Time of the last blog posting.</returns>
        private DateTime GetLastBlogDate()
        {
            IEnumerable<FileInfo> theFiles = GetFiles(_options.PostPathOption, ".wpost");

            var files = from file in theFiles
                        where file.Name.Contains(_options.PostPrefixOption)
                        orderby file.CreationTime descending
                        select file;

            List<FileInfo> matchingFiles = files.ToList();

            return matchingFiles.Count > 0 ? matchingFiles[0].CreationTime : DateTime.Now.AddDays(-1);
        }

        /// <summary>
        /// Names and gets the category headings.
        /// </summary>
        /// <returns></returns>
        private static List<Category> NameAndGetHeadings()
        {
            var categories = new List<Category>
                                 {
                                     new Category(CategoryType.Top),
                                     new Category(CategoryType.WebDevelopment),
                                     new Category(CategoryType.Xaml),
                                     new Category(CategoryType.DotNet),
                                     new Category(CategoryType.Design),
                                     new Category(CategoryType.Mobile),
                                     new Category(CategoryType.Podcasts),
                                     new Category(CategoryType.Community),
                                     new Category(CategoryType.Sql),
                                     new Category(CategoryType.SharePoint),
                                     new Category(CategoryType.PowerShell),
                                     new Category(CategoryType.Miscellaneous),
                                     new Category(CategoryType.Links),
                                     new Category(CategoryType.Shelf)
                                 };

            return categories;
        }
    }
}
