using AlvinAshcraft.LinkBuilder.Model;

namespace AlvinAshcraft.LinkBuilder.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    public static class LookupHelper
    {
        private static readonly IDictionary<string, string> CategoryLookupDictionary;

        private static readonly IDictionary<string, Tuple<string, string>> AuthorExactLookupDictionary;

        private static readonly IDictionary<string, Tuple<string, string>> AuthorContainsLookupDictionary;

        private static readonly IDictionary<string, Tuple<string, string>> UrlContainsLookupDictionary;

        /// <summary>
        /// Initializes static members of the <see cref="LookupHelper"/> class.
        /// </summary>
        static LookupHelper()
        {
            string file = File.ReadAllText($"{AssemblyDirectory}\\CategoryLookup.xml");

            CategoryLookupDictionary =
                XElement.Parse(file)
                    .Elements("def")
                    .ToDictionary(el => (string)el.Attribute("keyword"), el => (string)el.Attribute("category"));

            string fileContains = File.ReadAllText($"{AssemblyDirectory}\\AuthorContainsLookup.xml");

            AuthorContainsLookupDictionary =
                XElement.Parse(fileContains)
                    .Elements("def")
                    .ToDictionary(el => (string)el.Attribute("keyword"), el => new Tuple<string, string>((string)el.Attribute("author"), (string)el.Attribute("category")));

            string fileExact = File.ReadAllText($"{AssemblyDirectory}\\AuthorExactLookup.xml");

            AuthorExactLookupDictionary =
                XElement.Parse(fileExact)
                    .Elements("def")
                    .ToDictionary(el => (string)el.Attribute("keyword"), el => new Tuple<string, string>((string)el.Attribute("author"), (string)el.Attribute("category")));

            string fileUrl = File.ReadAllText($"{AssemblyDirectory}\\UrlContainsLookup.xml");

            UrlContainsLookupDictionary =
                XElement.Parse(fileUrl)
                    .Elements("def")
                    .ToDictionary(el => (string)el.Attribute("keyword"), el => new Tuple<string, string>((string)el.Attribute("author"), (string)el.Attribute("category")));
        }

        /// <summary>
        /// Gets the category by keyword.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <param name="defaultCategory">The default category.</param>
        /// <returns>Category.</returns>
        public static Category GetCategoryByKeyword(string keyword, Category defaultCategory)
        {
            foreach (string kw in CategoryLookupDictionary.Keys.Where(kw => keyword.ToLower().Contains(kw)))
            {
                return new Category((CategoryType)Enum.Parse(typeof(CategoryType), CategoryLookupDictionary[kw]));
            }

            return defaultCategory;
        }

        /// <summary>
        /// Finds and author result for a given author name.
        /// </summary>
        /// <param name="authorName">The current author name.</param>
        /// <param name="findExactMatch">Indicates if an 'exact' or 'contains' search should be performed.</param>
        /// <returns>An author result for the lookup.</returns>
        public static AuthorResult GetAuthorInfoByName(string authorName, bool findExactMatch)
        {
            if (findExactMatch)
            {
                foreach (string kw in AuthorExactLookupDictionary.Keys.Where(kw => authorName.ToLower().Equals(kw)))
                {
                    return new AuthorResult(AuthorExactLookupDictionary[kw].Item1, new Category((CategoryType)Enum.Parse(typeof(CategoryType), AuthorExactLookupDictionary[kw].Item2)));
                }
            }
            else
            {
                foreach (string kw in AuthorContainsLookupDictionary.Keys.Where(kw => authorName.ToLower().Contains(kw)))
                {
                    return new AuthorResult(AuthorContainsLookupDictionary[kw].Item1, new Category((CategoryType)Enum.Parse(typeof(CategoryType), AuthorContainsLookupDictionary[kw].Item2)));
                }
            }

            return new AuthorResult(authorName, new Category());
        }

        /// <summary>
        /// Finds an author result for a given Url.
        /// </summary>
        /// <param name="authorName">The current author name.</param>
        /// <param name="url">The url to search.</param>
        /// <returns>An author result for the lookup.</returns>
        public static AuthorResult GetAuthorInfoByUrl(string authorName, string url)
        {
            foreach (string kw in UrlContainsLookupDictionary.Keys.Where(kw => url.ToLower().Contains(kw)))
            {
                return new AuthorResult(UrlContainsLookupDictionary[kw].Item1, new Category((CategoryType)Enum.Parse(typeof(CategoryType), UrlContainsLookupDictionary[kw].Item2)));
            }

            return new AuthorResult(authorName, new Category());
        }

        /// <summary>
        /// Gets the assembly directory.
        /// </summary>
        /// <value>The assembly directory.</value>
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}