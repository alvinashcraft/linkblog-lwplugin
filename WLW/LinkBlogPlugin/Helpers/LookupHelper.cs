using AlvinAshcraft.LinkBuilder.Model;

namespace AlvinAshcraft.LinkBuilder.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    public class LookupHelper
    {
        private readonly IDictionary<string, string> _categoryLookupDictionary;

        private readonly IDictionary<string, Tuple<string, string>> _authorExactLookupDictionary;

        private readonly IDictionary<string, Tuple<string, string>> _authorContainsLookupDictionary;

        private readonly IDictionary<string, Tuple<string, string>> _urlContainsLookupDictionary;

        /// <summary>
        /// Initializes static members of the <see cref="LookupHelper"/> class.
        /// </summary>
        public LookupHelper()
        {
            _categoryLookupDictionary =
                XDocument.Load($"{AssemblyDirectory}\\CategoryLookup.xml")
                    .Descendants("def")
                    .ToDictionary(e1 => (string)e1.Attribute("keyword"), e2 => (string)e2.Attribute("category"));
            
            _authorContainsLookupDictionary =
                XDocument.Load($"{AssemblyDirectory}\\AuthorContainsLookup.xml")
                    .Descendants("def")
                    .ToDictionary(e1 => (string)e1.Attribute("keyword"), e2 => new Tuple<string, string>((string)e2.Attribute("author"), (string)e2.Attribute("category")));
            
            _authorExactLookupDictionary =
                XDocument.Load($"{AssemblyDirectory}\\AuthorExactLookup.xml")
                    .Descendants("def")
                    .ToDictionary(e1 => (string)e1.Attribute("keyword"), e2 => new Tuple<string, string>((string)e2.Attribute("author"), (string)e2.Attribute("category")));
            
            _urlContainsLookupDictionary =
                XDocument.Load($"{AssemblyDirectory}\\UrlContainsLookup.xml")
                    .Descendants("def")
                    .ToDictionary(e1 => (string)e1.Attribute("keyword"), e2 => new Tuple<string, string>((string)e2.Attribute("author"), (string)e2.Attribute("category")));
        }

        /// <summary>
        /// Gets the category by keyword.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <param name="defaultCategory">The default category.</param>
        /// <returns>Category.</returns>
        public Category GetCategoryByKeyword(string keyword, Category defaultCategory)
        {
            foreach (string kw in _categoryLookupDictionary.Keys.Where(kw => keyword.ToLower().Contains(kw)))
            {
                return new Category((CategoryType)Enum.Parse(typeof(CategoryType), _categoryLookupDictionary[kw]));
            }

            return defaultCategory;
        }

        /// <summary>
        /// Finds and author result for a given author name.
        /// </summary>
        /// <param name="authorName">The current author name.</param>
        /// <param name="findExactMatch">Indicates if an 'exact' or 'contains' search should be performed.</param>
        /// <returns>An author result for the lookup.</returns>
        public AuthorResult GetAuthorInfoByName(string authorName, bool findExactMatch)
        {
            string result;

            if (findExactMatch)
            {
                result = _authorExactLookupDictionary.Keys.FirstOrDefault(authorName.Equals);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    return new AuthorResult(_authorExactLookupDictionary[result].Item1,
                        new Category((CategoryType) Enum.Parse(typeof(CategoryType),
                            _authorExactLookupDictionary[result].Item2)));
                }
            }
            else
            {
                result = _authorContainsLookupDictionary.Keys.FirstOrDefault(authorName.Contains);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    return new AuthorResult(_authorContainsLookupDictionary[result].Item1,
                        new Category((CategoryType) Enum.Parse(typeof(CategoryType),
                            _authorContainsLookupDictionary[result].Item2)));
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
        public AuthorResult GetAuthorInfoByUrl(string authorName, string url)
        {
            var result = _urlContainsLookupDictionary.Keys.FirstOrDefault(url.Contains);

            if (!string.IsNullOrWhiteSpace(result))
            {
                return new AuthorResult(_urlContainsLookupDictionary[result].Item1,
                    new Category((CategoryType) Enum.Parse(typeof(CategoryType),
                        _urlContainsLookupDictionary[result].Item2)));
            }

            return new AuthorResult(authorName, new Category());
        }

        /// <summary>
        /// Gets the assembly directory.
        /// </summary>
        /// <value>The assembly directory.</value>
        public string AssemblyDirectory
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