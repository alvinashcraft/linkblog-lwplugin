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
        private const string BlogPath = @"C:\Users\aashcraft.ECLIPSYS\Documents\My Weblog Posts\Recent Posts";

        private IProperties m_properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginSettings"/> class.
        /// </summary>
        /// <param name="properties">The properties.</param>
        public PluginSettings(IProperties properties)
        {
            m_properties = properties;
        }

        /// <summary>
        /// Gets or sets the feed type option.
        /// </summary>
        /// <value>The feed type option.</value>
        public string FeedTypeOption
        {
            get
            {
                return m_properties.GetString("FeedType", "Atom 1.0");
            }
            set
            {
                m_properties.SetString("FeedType", value);
            }
        }

        /// <summary>
        /// Gets or sets the feed URL option.
        /// </summary>
        /// <value>The feed URL option.</value>
        public string FeedUrlOption
        {
            get
            {
                return m_properties.GetString("FeedUrl", Url);
            }
            set
            {
                m_properties.SetString("FeedUrl", value);
            }
        }

        /// <summary>
        /// Gets or sets the post path option.
        /// </summary>
        /// <value>The post path option.</value>
        public string PostPathOption
        {
            get
            {
                return m_properties.GetString("PostPath", BlogPath);
            }
            set
            {
                m_properties.SetString("PostPath", value);
            }
        }

        /// <summary>
        /// Gets or sets the post prefix option.
        /// </summary>
        /// <value>The post prefix option.</value>
        public string PostPrefixOption
        {
            get
            {
                return m_properties.GetString("PostPrefix", "Dew Drop");
            }
            set
            {
                m_properties.SetString("PostPrefix", value);
            }
        }

        /// <summary>
        /// Gets or sets the max posts option.
        /// </summary>
        /// <value>The max posts option.</value>
        public int MaxPostsOption
        {
            get
            {
                return m_properties.GetInt("MaxPosts", 50);
            }
            set
            {
                m_properties.SetInt("MaxPosts", value);
            }
        }

        /// <summary>
        /// Gets or sets the buffer option.
        /// </summary>
        /// <value>The buffer option.</value>
        public int BufferOption
        {
            get
            {
                return m_properties.GetInt("Buffer", 12);
            }
            set
            {
                m_properties.SetInt("Buffer", value);
            }
        }
    }
}
