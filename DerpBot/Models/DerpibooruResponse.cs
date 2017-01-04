using System;
// ReSharper disable InconsistentNaming

namespace DerpBot.Models
{
    public class DerpibooruResponse
    {

        public class Rootobject
        {
            public Search[] search { get; set; }
            public int total { get; set; }
            public object[] interactions { get; set; }
        }

        public class Search
        {
            public string id { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public Duplicate_Reports[] duplicate_reports { get; set; }
            public DateTime first_seen_at { get; set; }
            public string uploader_id { get; set; }
            public int score { get; set; }
            public int comment_count { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public string file_name { get; set; }
            public string description { get; set; }
            public string uploader { get; set; }
            public string image { get; set; }
            public int upvotes { get; set; }
            public int downvotes { get; set; }
            public int faves { get; set; }
            public string tags { get; set; }
            public string[] tag_ids { get; set; }
            public float aspect_ratio { get; set; }
            public string original_format { get; set; }
            public string mime_type { get; set; }
            public string sha512_hash { get; set; }
            public string orig_sha512_hash { get; set; }
            public string source_url { get; set; }
            public Representations representations { get; set; }
            public bool is_rendered { get; set; }
            public bool is_optimized { get; set; }
        }

        public class Representations
        {
            public string thumb_tiny { get; set; }
            public string thumb_small { get; set; }
            public string thumb { get; set; }
            public string small { get; set; }
            public string medium { get; set; }
            public string large { get; set; }
            public string tall { get; set; }
            public string full { get; set; }
        }

        public class Duplicate_Reports
        {
            public int id { get; set; }
            public string state { get; set; }
            public string reason { get; set; }
            public int image_id { get; set; }
            public int duplicate_of_image_id { get; set; }
            public object user_id { get; set; }
            public Modifier modifier { get; set; }
            public DateTime created_at { get; set; }
        }

        public class Modifier
        {
            public int id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
            public string role { get; set; }
            public string description { get; set; }
            public string avatar_url { get; set; }
            public DateTime created_at { get; set; }
            public int comment_count { get; set; }
            public int uploads_count { get; set; }
            public int post_count { get; set; }
            public int topic_count { get; set; }
            public Link[] links { get; set; }
            public Award[] awards { get; set; }
        }

        public class Link
        {
            public int user_id { get; set; }
            public DateTime created_at { get; set; }
            public string state { get; set; }
            public int[] tag_ids { get; set; }
        }

        public class Award
        {
            public string image_url { get; set; }
            public string title { get; set; }
            public int id { get; set; }
            public string label { get; set; }
            public DateTime awarded_on { get; set; }
        }

    }
}
