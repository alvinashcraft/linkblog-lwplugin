using OpenLiveWriter.Api;

namespace AlvinAshcraft.LinkBuilder
{
    public class PluginSettings
    {
        /// <summary>
        /// The URL to pull the ATOM feed from.
        /// </summary>
        private const string Url = "http://www.google.com/reader/public/atom/user/15217774922118815663/state/com.google/starred";

        /// <summary>
        /// The path to WLW blog posts that have been published.
        /// </summary>
        private const string BlogPath = @"C:\Users\aashcraft\Documents\My Weblog Posts\Recent Posts";

        private readonly IProperties _properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginSettings"/> class.
        /// </summary>
        /// <param name="properties">The properties.</param>
        public PluginSettings(IProperties properties)
        {
            _properties = properties;
        }

        /// <summary>
        /// Gets or sets the feed type option.
        /// </summary>
        /// <value>The feed type option.</value>
        public string FeedTypeOption
        {
            get => _properties.GetString("FeedType", "Atom 1.0");
            set => _properties.SetString("FeedType", value);
        }

        /// <summary>
        /// Gets or sets the feed URL option.
        /// </summary>
        /// <value>The feed URL option.</value>
        public string FeedUrlOption
        {
            get => _properties.GetString("FeedUrl", Url);
            set => _properties.SetString("FeedUrl", value);
        }

        /// <summary>
        /// Gets or sets the post path option.
        /// </summary>
        /// <value>The post path option.</value>
        public string PostPathOption
        {
            get => _properties.GetString("PostPath", BlogPath);
            set => _properties.SetString("PostPath", value);
        }

        /// <summary>
        /// Gets or sets the post prefix option.
        /// </summary>
        /// <value>The post prefix option.</value>
        public string PostPrefixOption
        {
            get => _properties.GetString("PostPrefix", "Dew Drop");
            set => _properties.SetString("PostPrefix", value);
        }

        /// <summary>
        /// Gets or sets the max posts option.
        /// </summary>
        /// <value>The max posts option.</value>
        public int MaxPostsOption
        {
            get => _properties.GetInt("MaxPosts", 50);
            set => _properties.SetInt("MaxPosts", value);
        }

        /// <summary>
        /// Gets or sets the buffer option.
        /// </summary>
        /// <value>The buffer option.</value>
        public int BufferOption
        {
            get => _properties.GetInt("Buffer", 12);
            set => _properties.SetInt("Buffer", value);
        }
    }
}