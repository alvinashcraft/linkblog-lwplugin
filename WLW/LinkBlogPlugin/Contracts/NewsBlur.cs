namespace AlvinAshcraft.LinkBuilder.Contracts
{
    public class Rootobject
    {
        public bool authenticated { get; set; }
        public Story[] stories { get; set; }
        public string result { get; set; }
        public object[] feeds { get; set; }
        public Classifiers classifiers { get; set; }
        public User_Profiles[] user_profiles { get; set; }
    }

    public class Classifiers
    {
        public _19247 _19247 { get; set; }
        public _716051 _716051 { get; set; }
        public _58418 _58418 { get; set; }
        public _13246 _13246 { get; set; }
        public _576138 _576138 { get; set; }
        public _1519608 _1519608 { get; set; }
    }

    public class _19247
    {
        public Authors authors { get; set; }
        public Feeds feeds { get; set; }
        public Titles titles { get; set; }
        public Tags tags { get; set; }
    }

    public class Authors
    {
    }

    public class Feeds
    {
    }

    public class Titles
    {
    }

    public class Tags
    {
    }

    public class _716051
    {
        public Authors1 authors { get; set; }
        public Feeds1 feeds { get; set; }
        public Titles1 titles { get; set; }
        public Tags1 tags { get; set; }
    }

    public class Authors1
    {
    }

    public class Feeds1
    {
    }

    public class Titles1
    {
    }

    public class Tags1
    {
    }

    public class _58418
    {
        public Authors2 authors { get; set; }
        public Feeds2 feeds { get; set; }
        public Titles2 titles { get; set; }
        public Tags2 tags { get; set; }
    }

    public class Authors2
    {
    }

    public class Feeds2
    {
    }

    public class Titles2
    {
    }

    public class Tags2
    {
    }

    public class _13246
    {
        public Authors3 authors { get; set; }
        public Feeds3 feeds { get; set; }
        public Titles3 titles { get; set; }
        public Tags3 tags { get; set; }
    }

    public class Authors3
    {
    }

    public class Feeds3
    {
    }

    public class Titles3
    {
    }

    public class Tags3
    {
    }

    public class _576138
    {
        public Authors4 authors { get; set; }
        public Feeds4 feeds { get; set; }
        public Titles4 titles { get; set; }
        public Tags4 tags { get; set; }
    }

    public class Authors4
    {
    }

    public class Feeds4
    {
    }

    public class Titles4
    {
    }

    public class Tags4
    {
    }

    public class _1519608
    {
        public Authors5 authors { get; set; }
        public Feeds5 feeds { get; set; }
        public Titles5 titles { get; set; }
        public Tags5 tags { get; set; }
    }

    public class Authors5
    {
    }

    public class Feeds5
    {
    }

    public class Titles5
    {
    }

    public class Tags5
    {
    }

    public class Story
    {
        public object source_user_id { get; set; }
        public string story_authors { get; set; }
        public Intelligence intelligence { get; set; }
        public int[] shared_by_friends { get; set; }
        public string story_permalink { get; set; }
        public int reply_count { get; set; }
        public object[] comment_user_ids { get; set; }
        public string story_timestamp { get; set; }
        public int[] share_user_ids { get; set; }
        public int comment_count_friends { get; set; }
        public object[] public_user_ids { get; set; }
        public string story_hash { get; set; }
        public string shared_date { get; set; }
        public string id { get; set; }
        public int comment_count { get; set; }
        public string story_title { get; set; }
        public string guid_hash { get; set; }
        public int social_user_id { get; set; }
        public int share_count { get; set; }
        public object[] friend_comments { get; set; }
        public string story_date { get; set; }
        public int share_count_public { get; set; }
        public int[] friend_user_ids { get; set; }
        public string shared_comments { get; set; }
        public string short_parsed_date { get; set; }
        public string[] story_tags { get; set; }
        public int share_count_friends { get; set; }
        public int story_feed_id { get; set; }
        public string long_parsed_date { get; set; }
        public bool shared { get; set; }
        public object[] public_comments { get; set; }
        public int read_status { get; set; }
        public object[] shared_by_public { get; set; }
        public bool has_modifications { get; set; }
        public int comment_count_public { get; set; }
        public object[] commented_by_public { get; set; }
        public string story_content { get; set; }
        public object[] commented_by_friends { get; set; }
    }

    public class Intelligence
    {
        public int feed { get; set; }
        public int tags { get; set; }
        public int author { get; set; }
        public int title { get; set; }
    }

    public class User_Profiles
    {
        public string username { get; set; }
        public string feed_address { get; set; }
        public int user_id { get; set; }
        public string feed_link { get; set; }
        public int num_subscribers { get; set; }
        public string feed_title { get; set; }
        public bool _private { get; set; }
        public bool _protected { get; set; }
        public string location { get; set; }
        public string large_photo_url { get; set; }
        public string id { get; set; }
        public string photo_url { get; set; }
    }
}