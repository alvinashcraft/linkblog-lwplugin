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
        private static readonly Dictionary<string, string> categoryLookupDictionary;

        private static readonly Dictionary<string, Tuple<string, string>> authorExactLookupDictionary;

        private static readonly Dictionary<string, Tuple<string, string>> authorContainsLookupDictionary;

        /// <summary>
        /// Initializes static members of the <see cref="LookupHelper"/> class.
        /// </summary>
        static LookupHelper()
        {
            string file = File.ReadAllText(String.Format("{0}\\CategoryLookup.xml", AssemblyDirectory));

            categoryLookupDictionary =
                XElement.Parse(file)
                    .Elements("def")
                    .ToDictionary(el => (string)el.Attribute("keyword"), el => (string)el.Attribute("category"));

            string fileContains = File.ReadAllText(String.Format("{0}\\AuthorContainsLookup.xml", AssemblyDirectory));

            authorContainsLookupDictionary =
                XElement.Parse(fileContains)
                    .Elements("def")
                    .ToDictionary(el => (string)el.Attribute("keyword"), el => new Tuple<string, string>((string)el.Attribute("author"), (string)el.Attribute("category")));

            string fileExact = File.ReadAllText(String.Format("{0}\\AuthorExactLookup.xml", AssemblyDirectory));

            authorExactLookupDictionary =
                XElement.Parse(fileExact)
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
            foreach (string kw in categoryLookupDictionary.Keys.Where(kw => keyword.ToLower().Contains(kw)))
            {
                return new Category((CategoryType)Enum.Parse(typeof(CategoryType), categoryLookupDictionary[kw]));
            }

            return defaultCategory;
        }

        public static AuthorResult GetAuthorInfoByName(string authorName, bool findExactMatch)
        {
            if (findExactMatch)
            {
                foreach (string kw in authorExactLookupDictionary.Keys.Where(kw => authorName.ToLower().Equals(kw)))
                {
                    return new AuthorResult(authorExactLookupDictionary[kw].First, new Category((CategoryType)Enum.Parse(typeof(CategoryType), authorExactLookupDictionary[kw].Second)));
                }
            }
            else
            {
                foreach (string kw in authorContainsLookupDictionary.Keys.Where(kw => authorName.ToLower().Contains(kw)))
                {
                    return new AuthorResult(authorContainsLookupDictionary[kw].First, new Category((CategoryType)Enum.Parse(typeof(CategoryType), authorContainsLookupDictionary[kw].Second)));
                }
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