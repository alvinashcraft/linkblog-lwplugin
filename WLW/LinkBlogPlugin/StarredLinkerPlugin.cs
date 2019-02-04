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
using AlvinAshcraft.LinkBuilder.Contracts;
using AlvinAshcraft.LinkBuilder.Helpers;
using AlvinAshcraft.LinkBuilder.Model;
using OpenLiveWriter.Api;

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

        private LookupHelper _lookupHelper = new LookupHelper();
        
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
        /// <param name="content">The new content.</param>
        /// <returns>The content to insert in the document.</returns>
        public override DialogResult CreateContent(IWin32Window dialogOwner, ref string content)
        {
            using (new CursorKeeper(Cursors.WaitCursor))
            {
                var result = MessageBox.Show("Your latest shared links will be inserted. Continue?", "Insert Shared Links", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    content =
                        string.Equals(_options.FeedTypeOption, "newsblur api",
                            StringComparison.CurrentCultureIgnoreCase)
                            ? BuildLinksFromNewsBlur()
                            : BuildLinks(
                                string.Equals(_options.FeedTypeOption, "rss 2.0",
                                    StringComparison.CurrentCultureIgnoreCase)
                                    ? (SyndicationFeedFormatter) new Rss20FeedFormatter()
                                    : new Atom10FeedFormatter());
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

            return dirInfo.GetFiles($"*{fileExtension}", SearchOption.TopDirectoryOnly).ToList();
        }

        /// <summary>
        /// Builds the links from an Atom feed.
        /// </summary>
        /// <param name="feedFormatter">The feed formatter.</param>
        /// <returns>A string containing the generated links.</returns>
        private string BuildLinks(SyndicationFeedFormatter feedFormatter)
        {
            string newContent = string.Empty;

            var xmlSettings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse
            };

            using (var reader = XmlReader.Create($"{_options.FeedUrlOption}?n={_options.MaxPostsOption}", xmlSettings))
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
                linkListing.Append($"<h3><a name=\"{category.Name}\"></a>{category.Caption}</h3>");

                foreach (BlogEntry entry in blogEntries.Where(e => e.AuthorInfo.DefaultCategory.Name == category.Name))
                {
                    string url = entry.Url;

                    url = AppendMvpIdToUrl(url);

                    linkListing.Append($"<p><a href=\"{url}\" target=\"_blank\">{entry.Title}</a> ({entry.AuthorInfo.AuthorName})</p>");
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
            string restResource = _options.FeedUrlOption.Replace($"{newsBlurBaseUrl}/", string.Empty);
            client.Authenticator = new HttpBasicAuthenticator("alvinashcraft", "Po02zxn#");
            var request = new RestRequest($"{restResource}?limit={_options.MaxPostsOption}", Method.GET);

            IRestResponse response = client.Execute(request);

            var stories = JsonConvert.DeserializeObject<Rootobject>(response.Content).stories;

            var storyList = stories.Where(s => DateTime.Parse(s.shared_date) > GetLastBlogDate().AddHours(_options.BufferOption * -1))
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
        private Category GetPostCategory(BlogEntry entry)
        {
            return _lookupHelper.GetCategoryByKeyword(entry.Title.ToLower(), entry.AuthorInfo.DefaultCategory);
        }

        /// <summary>
        /// Gets the blog author.
        /// </summary>
        /// <param name="author">The author.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        private AuthorResult GetBlogAuthor(string author, string url)
        {
            author = author ?? string.Empty;
            url = url ?? string.Empty;

            string lowerAuthor = author.ToLower();
            string lowerUrl = url.ToLower();

            AuthorResult authorResult = CheckAuthorContains(lowerAuthor);

            if (!string.Equals(lowerAuthor, authorResult.AuthorName)) return authorResult;

            authorResult = FindExactAuthorMatch(lowerAuthor);

            if (!string.Equals(lowerAuthor, authorResult.AuthorName)) return authorResult;

            authorResult = CheckAuthorAndUrl(lowerAuthor, lowerUrl);

            if (!string.Equals(lowerAuthor, authorResult.AuthorName)) return authorResult;

            authorResult = CheckUrlContains(lowerAuthor, lowerUrl);

            if (!string.Equals(lowerAuthor, authorResult.AuthorName)) return authorResult;

            if (!string.IsNullOrEmpty(author) && !lowerAuthor.Contains("unknown"))
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
                case "scott":
                    if (url.Contains("roguetechnology")) return new AuthorResult("Scott Banwart", new Category(CategoryType.Links));
                    break;
                case "tobias":
                    if (url.Contains("powershell")) return new AuthorResult("Tobias Weltner", new Category(CategoryType.PowerShell));
                    break;
                case "tess":
                    if (url.Contains("msdn")) return new AuthorResult("Tess Ferrandez", new Category(CategoryType.DotNet));
                    break;
                case "alexanderb":
                    if (url.Contains("dzone")) return new AuthorResult("Alexander Beletsky", new Category(CategoryType.WebDevelopment));
                    break;
            }
            
            return new AuthorResult(authorName, new Category());
        }

        /// <summary>
        /// Checks if the authorName field contains particular text and returns the correct name.
        /// </summary>
        /// <param name="authorName">Name of the author.</param>
        /// <returns>An author result for the lookup.</returns>
        private AuthorResult CheckAuthorContains(string authorName)
        {
            return _lookupHelper.GetAuthorInfoByName(authorName, false);
        }

        /// <summary>
        /// Finds the exact author match.
        /// </summary>
        /// <param name="authorName">Name of the author.</param>
        /// <returns>An author result for the lookup.</returns>
        private AuthorResult FindExactAuthorMatch(string authorName)
        {
            return _lookupHelper.GetAuthorInfoByName(authorName, true);
        }

        /// <summary>
        /// Checks the URL contains.
        /// </summary>
        /// <param name="authorName">Name of the author.</param>
        /// <param name="url">The URL.</param>
        /// <returns>An author result for the lookup.</returns>
        private AuthorResult CheckUrlContains(string authorName, string url)
        {
            return _lookupHelper.GetAuthorInfoByUrl(authorName, url);
        }

        /// <summary>
        /// Gets the last blog date.
        /// </summary>
        /// <returns>DateTime of the last blog posting.</returns>
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
        /// <returns>A list of category names.</returns>
        private static List<Category> NameAndGetHeadings()
        {
            return new List<Category>
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
        }
    }
}