using System;
using System.IO;
using System.Linq;
using DerpBot.Functions;
using DerpBot.Models;
using Imgur.API.Models;
using Newtonsoft.Json;
using RedditSharp;
using RedditSharp.Things;

using static System.DateTime;
using static System.Console;
using static System.String;
using Post = DerpBot.Functions.Post;

namespace DerpBot
{
    class Program
    {
        static void Main()
        {
            string version = "0.3";
            Title = $"Derpbot {version}";

            configuration config = Load.Config();
            //Pesudo-settings!
            string loggingpath = @"logs";
            string derpibooruApiKey = config.derpibooru.apikey;
            string urltype = config.derpibooru.type;
            string domain = config.derpibooru.domain;


            string imgurApiKey = config.imgur.apikey;
            bool rehost_on_imgur = config.imgur.useimgur == 1;
            string reddit_username = config.reddit.username;
            string reddit_password = config.reddit.password;
            string client_id = config.reddit.client_id;
            string secret_key = config.reddit.secret_id;
            string callback_url = config.reddit.callback_url;


            //Todo  support for multiple subs 
            string method = config.subreddit_configurations[0].method;
            string reddit_sub = config.subreddit_configurations[0].subreddit;
            string time_frame = config.subreddit_configurations[0].timeframe;
            //daily, weekly, monthly, off 
            //Daily will look at yesterdays post
            //Weekly will look at the posts for the last 7 days (if ran on 10th, will get top posts between 3rd-10th)
            //Monthly will look at last months posts (if ran in September, you will get August's top posts)
            string tags = config.subreddit_configurations[0].tags;

            switch (time_frame)
            {
                case "daily":
                    tags += $", created_at:{Now.AddDays(-1):yyyy-MM-dd}";
                    break;
                case "weekly":
                    tags += $", created_at.gte:{Now.AddDays(-7):yyyy-MM-dd}";
                    break;
                case "monthly":
                    tags +=  $", created_at:{Now.AddMonths(-1):yyyy-MM}";
                    break;
            }

            StreamWriter logfile = Create.Log(loggingpath);

            logfile.AutoFlush = true;
            Log log = new Log(logfile);

            SetOut(new PrefixedWriter());
            log.WriteLine($"Log created {Now:MM:dd:yyyy}");


            //StringBuilder builtSearch = new StringBuilder();

            //string last = tags.Last();
            //foreach (var tag in tags)
            //{
            //    if (tag != last)
            //        builtSearch.Append(tag).Append(",").Append(" ");
            //    else
            //        builtSearch.Append(tag);
            //}


            string requestUrl = $"{urltype}://{domain}/{method}?q={tags}&key={derpibooruApiKey}&sf=score&sd=desc";


            //Why not shove the api call method, json serialization and object lambada selection all in one statment!? MWHAHAHAHA
            DerpibooruResponse.Search top =
                JsonConvert.DeserializeObject<DerpibooruResponse.Rootobject>(Get.Derpibooru(requestUrl).Result)
                    .search.First();
            string[] topImageTags = top.tags.Split(',');
            string artist = Empty;
            foreach (string tag in topImageTags)
                if (tag.Contains("artist:"))
                    artist = tag.Replace("artist:", Empty).Replace(" ", Empty);

            string postTitle = $"Derpibooru Top Image of {Now.AddDays(-1):MM-dd-yyyy} [Artist: {artist}]";
            string source = top.source_url.Length > 0 ? top.source_url : Empty;

            string hostedImageLink;
            WriteLine($"Top found with ID {top.id}");
            if (rehost_on_imgur)
            {

                Request.Imgur imgurRequest = new Request.Imgur
                {
                    ApiKey = imgurApiKey,
                    Description = source,
                    Title = postTitle,
                    Url = $"{urltype}:{top.image}"
                };


                IImage hostedImage = Post.PostToImgur(imgurRequest).Result;
                WriteLine($"Image uploaded to Imgur: {hostedImage.Link}");
                hostedImageLink = hostedImage.Link;
                HashDb.Write($"{hostedImage.Link} http://imgur.com/delete/{hostedImage.DeleteHash}");
            }
            else
            {
                hostedImageLink = $"{urltype}://{domain}/{top.id}";
            }

            string parsedsource = source == Empty ? "No source provided" : $"[Original Source]({source})";

            string comment = "[](/sweetiecardbot) " +
                             $"{parsedsource} | [Derpibooru Link]({urltype}://{domain}/{top.id})" +
                             $" \r\n  \r\n[](/sp)  \r\n  \r\n---  \r\n  \r\n^(This is a bot | )[^Info](http://club-flank.com/sweetiebot/)^( | )[^(Report problems)](/message/compose/?to=BitzLeon&subject=_sweetiebot running derpbot {version})^( | )[^(Source code)](https://google.com)";


            BotWebAgent webAgent = new BotWebAgent(
                reddit_username,
                reddit_password,
                client_id,
                secret_key,
                callback_url);
            Reddit reddit = new Reddit(webAgent, true);
            reddit.LogIn(reddit_username, reddit_password);
            if (reddit.User.FullName == reddit_username)
            {
                WriteLine("Logged into Reddit.");
            }
            Subreddit subreddit = reddit.GetSubredditAsync(reddit_sub).Result;

            try
            {
                RedditSharp.Things.Post post = subreddit.SubmitPost(postTitle, hostedImageLink);
                post.Comment(comment);
                WriteLine("Commented on and posted to Reddit");
            }
            catch (Exception e)
            {
                WriteLine(e.Message);
                log.WriteLine(e.Message);
            }




            log.WriteLine("Application exited normally.");
            WriteLine("Application exited normally.");
            ReadLine();
        }
    }
}
