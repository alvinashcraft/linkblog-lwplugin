namespace AlvinAshcraft.LinkBuilder.Model
{
    public class Category
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <param name="categoryType">Type of the category.</param>
        public Category(CategoryType categoryType)
        {
            CatType = categoryType;
            Name = GetCategoryNameForType(categoryType);
            Caption = GetCategoryCaptionForType(categoryType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        public Category()
        {
            Name = "misc";
            Caption = "Miscellaneous";
            CatType = CategoryType.Miscellaneous;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption { get; }

        public CategoryType CatType { get; }

        private string GetCategoryCaptionForType(CategoryType categoryType)
        {
            switch (categoryType)
            {
                case CategoryType.DotNet:
                    return "Visual Studio & .NET";
                case CategoryType.WebDevelopment:
                    return "Web & Cloud Development";
                case CategoryType.Design:
                    return "Design, Methodology & Testing";
                case CategoryType.Xaml:
                    return "XAML, UWP & Xamarin";
                case CategoryType.Mobile:
                    return "Mobile, IoT & Game Development";
                case CategoryType.Podcasts:
                    return "Podcasts, Screencasts & Videos";
                case CategoryType.Community:
                    return "Community & Events";
                case CategoryType.Sql:
                    return "Database";
                case CategoryType.SharePoint:
                    return "SharePoint & MS Teams";
                case CategoryType.PowerShell:
                    return "PowerShell";
                case CategoryType.Miscellaneous:
                    return "Miscellaneous";
                case CategoryType.Links:
                    return "More Link Collections";
                case CategoryType.Top:
                    return "Top Links";
                case CategoryType.Shelf:
                    return "The Geek Shelf";
                default:
                    return "Miscellaneous";
            }
        }

        private string GetCategoryNameForType(CategoryType categoryType)
        {
            switch (categoryType)
            {
                case CategoryType.DotNet:
                    return "dotnet";
                case CategoryType.WebDevelopment:
                    return "web";
                case CategoryType.Design:
                    return "design";
                case CategoryType.Xaml:
                    return "silverlight";
                case CategoryType.Mobile:
                    return "mobile";
                case CategoryType.Podcasts:
                    return "podcasts";
                case CategoryType.Community:
                    return "events";
                case CategoryType.Sql:
                    return "sql";
                case CategoryType.SharePoint:
                    return "sp";
                case CategoryType.PowerShell:
                    return "ps";
                case CategoryType.Miscellaneous:
                    return "misc";
                case CategoryType.Links:
                    return "links";
                case CategoryType.Top:
                    return "top";
                case CategoryType.Shelf:
                    return "shelf";
                default:
                    return "misc";
            }
        }
    }
}
