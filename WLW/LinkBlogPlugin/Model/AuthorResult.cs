namespace AlvinAshcraft.LinkBuilder.Model
{
    public class AuthorResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorResult"/> class.
        /// </summary>
        /// <param name="authorName">Name of the author.</param>
        /// <param name="defaultCategory">The default category.</param>
        public AuthorResult(string authorName, Category defaultCategory)
        {
            AuthorName = authorName;
            DefaultCategory = defaultCategory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorResult"/> class.
        /// </summary>
        public AuthorResult()
        {
            DefaultCategory = new Category();
        }

        /// <summary>
        /// Gets or sets the name of the author.
        /// </summary>
        /// <value>The name of the author.</value>
        public string AuthorName { get; set; }

        /// <summary>
        /// Gets or sets the default category.
        /// </summary>
        /// <value>The default category.</value>
        public Category DefaultCategory { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return AuthorName;
        }
    }
}