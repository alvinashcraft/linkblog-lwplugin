//--------------------------------------------------------------------------------------------------------------------- 
// <copyright file="BlogEntry.cs" company="Bit Tappers">
//   Copyright by Alvin Ashcraft 2009 - Bit Tappers.
// </copyright>
// <summary>
//   Defines the BlogEntry type.
// </summary>
//---------------------------------------------------------------------------------------------------------------------
namespace AlvinAshcraft.LinkBuilder
{
    /// <summary>
    /// Defines the BlogEntry type.
    /// </summary>
    public class BlogEntry
    {
        /// <summary>
        /// Gets or sets the title of the blog entry.
        /// </summary>
        /// <value>The article title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The article's URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>The article's author.</value>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the author info.
        /// </summary>
        /// <value>The author info.</value>
        public AuthorResult AuthorInfo { get; set; }
    }
}
