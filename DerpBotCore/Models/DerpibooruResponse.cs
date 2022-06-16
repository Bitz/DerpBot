namespace DerpBotCore.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Derpibooru
    {
        [JsonProperty("images")] public List<Image> Images { get; set; }

        //[JsonProperty("total")] public long Total { get; set; }
    }

    public class Image
    {

        //[JsonProperty("comment_count")] public long CommentCount { get; set; }

        //[JsonProperty("upvotes")] public long Upvotes { get; set; }

        //[JsonProperty("duplicate_of")] public object DuplicateOf { get; set; }

        [JsonProperty("representations")] public Representations Representations { get; set; }

        [JsonProperty("source_url")] public string SourceUrl { get; set; }
        
        //[JsonProperty("score")] public long Score { get; set; }

        //[JsonProperty("sha512_hash")] public string Sha512Hash { get; set; }

        //[JsonProperty("description")] public string Description { get; set; }

        //[JsonProperty("uploader")] public string Uploader { get; set; }

        //[JsonProperty("faves")] public long Faves { get; set; }

        //[JsonProperty("downvotes")] public long Downvotes { get; set; }

        //[JsonProperty("processed")] public bool Processed { get; set; }

        //[JsonProperty("wilson_score")] public double WilsonScore { get; set; }

        //[JsonProperty("orig_sha512_hash")] public string OrigSha512Hash { get; set; }

        //[JsonProperty("created_at")] public DateTimeOffset CreatedAt { get; set; }

        //[JsonProperty("width")] public long Width { get; set; }

        //[JsonProperty("hidden_from_users")] public bool HiddenFromUsers { get; set; }

        //[JsonProperty("aspect_ratio")] public double AspectRatio { get; set; }

        //[JsonProperty("tag_count")] public long TagCount { get; set; }

        //[JsonProperty("height")] public long Height { get; set; }

        //[JsonProperty("first_seen_at")] public DateTimeOffset FirstSeenAt { get; set; }

        [JsonProperty("view_url")] public string ViewUrl { get; set; }

        //[JsonProperty("tag_ids")] public List<long> TagIds { get; set; }

        //[JsonProperty("spoilered")] public bool Spoilered { get; set; }

        //[JsonProperty("updated_at")] public DateTimeOffset UpdatedAt { get; set; }
        [JsonProperty("id")] public long Id { get; set; }

        //[JsonProperty("intensities")] public Intensities Intensities { get; set; }

        //[JsonProperty("thumbnails_generated")] public bool ThumbnailsGenerated { get; set; }
        [JsonProperty("tags")] public List<string> Tags { get; set; }

        //[JsonProperty("deletion_reason")] public object DeletionReason { get; set; }

        //[JsonProperty("name")] public string Name { get; set; }

        //[JsonProperty("uploader_id")] public long? UploaderId { get; set; }
    }

    //public class Intensities
    //{
    //    [JsonProperty("ne")] public double Ne { get; set; }

    //    [JsonProperty("nw")] public double Nw { get; set; }

    //    [JsonProperty("se")] public double Se { get; set; }

    //    [JsonProperty("sw")] public double Sw { get; set; }
    //}

    public partial class Representations
    {
        //[JsonProperty("full")] public Uri Full { get; set; }

        [JsonProperty("large")] public Uri Large { get; set; }

        //[JsonProperty("medium")] public Uri Medium { get; set; }

        //[JsonProperty("small")] public Uri Small { get; set; }

        //[JsonProperty("tall")] public Uri Tall { get; set; }

        //[JsonProperty("thumb")] public Uri Thumb { get; set; }

        //[JsonProperty("thumb_small")] public Uri ThumbSmall { get; set; }

        //[JsonProperty("thumb_tiny")] public Uri ThumbTiny { get; set; }
    }
}