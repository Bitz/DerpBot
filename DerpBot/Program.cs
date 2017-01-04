using System;
using System.Collections.Generic;
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
        static void Main(string[] args)
        {
            string version = "0.3";
            Title = $"Derpbot {version}";
            configuration config = null;
            try
            {
                config = Load.Config();
            }
            catch (Exception e)
            {
                WriteLine($"Error trying to load the config: {e.Message}");
                ReadLine();
                Environment.Exit(0);
            }
            //Assign variables from the loaded settings.
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

            int argument_index = 0;
            if (args.Length > 0)
            {
                if (int.TryParse(args[0], out argument_index))
                {
                    WriteLine("Found valid argument!");
                }
            }

            try
            {
                WriteLine($"Running for subreddit /r/{config.subreddit_configurations[argument_index].method}");
            }
            catch (Exception e)
            {
                WriteLine($"Error loading subreddit config: {e.Message}");
                ReadLine();
                Environment.Exit(1);
            }
            string method = config.subreddit_configurations[argument_index].method;
            string reddit_sub = config.subreddit_configurations[argument_index].subreddit;
            string time_frame = config.subreddit_configurations[argument_index].timeframe;
            //daily, weekly, monthly, off 
            //Daily will look at yesterdays post
            //Weekly will look at the posts for the last 7 days (if ran on 10th, will get top posts between 3rd-10th)
            //Monthly will look at last months posts (if ran in September, you will get August's top posts)
            string tags = config.subreddit_configurations[argument_index].tags;

            switch (time_frame)
            {
                case "daily":
                    tags += $", created_at:{Now.AddDays(-1):yyyy-MM-dd}";
                    break;
                case "weekly":
                    tags += $", created_at.gte:{Now.AddDays(-7):yyyy-MM-dd}";
                    break;
                case "monthly":
                    tags += $", created_at:{Now.AddMonths(-1):yyyy-MM}";
                    break;
            }

            StreamWriter logfile = Create.Log(loggingpath);

            logfile.AutoFlush = true;
            Log log = new Log(logfile);

            SetOut(new PrefixedWriter());
            log.WriteLine($"Log created {Now:MM:dd:yyyy}");


            //Was going to rebuild using a list, but it is easier to skip this as XML doesn't play well with lists that arent csv
            //StringBuilder builtSearch = new StringBuilder();
            //string last = tags.Last();
            //foreach (var tag in tags)
            //{
            //    if (tag != last)
            //        builtSearch.Append(tag).Append(",").Append(" ");
            //    else
            //        builtSearch.Append(tag);
            //}

            //Build the request url for Derpibooru
            string requestUrl = $"{urltype}://{domain}/{method}?q={tags}&key={derpibooruApiKey}&sf=score&sd=desc";


            //Why not shove the api call method, json serialization and object lambada selection all in one statment!? MWHAHAHAHA
            DerpibooruResponse.Search top =
                JsonConvert.DeserializeObject<DerpibooruResponse.Rootobject>(Get.Derpibooru(requestUrl).Result)
                    .search.First();

            //Find the artist data in the tags that were pulled from Derpibooru
            List<string> topImageTags = top.tags.Split(',').ToList();
            string artist = topImageTags.SingleOrDefault(x => x.Contains("artist:"));
            //TODO- Fix support for names with spaces
            artist = artist != Empty ? artist?.Replace("artist:", Empty) : "Unknown";

            string postTitle = $"Derpibooru Top Image of {Now.AddDays(-1):MM-dd-yyyy} [Artist:{artist}]";
            string source = top.source_url.Length > 0 ? top.source_url : Empty;

            string hostedImageLink;
            WriteLine($"Top found with ID {top.id}");
            if (rehost_on_imgur)
            {
                //Build imgur request
                Request.Imgur imgurRequest = new Request.Imgur
                {
                    ApiKey = imgurApiKey,
                    Description = source,
                    Title = postTitle,
                    Url = $"{urltype}:{top.image}"
                };

                //Post to imgur
                IImage hostedImage = Post.PostToImgur(imgurRequest).Result;
                WriteLine($"Image uploaded to Imgur: {hostedImage.Link}");
                hostedImageLink = hostedImage.Link;
                //Keep the delete hash in a text file, incase someone doesn't want us reposting their content
                HashDb.Write($"{hostedImage.Link} http://imgur.com/delete/{hostedImage.DeleteHash}");
            }
            else
            {
                //If we aren't using imgur to host, we are just going to build a link to derpibooru instead
                hostedImageLink = $"{urltype}://{domain}/{top.id}";
            }

            //Building all the reddit request parts
            string parsedsource = source == Empty ? "No source provided" : $"[Original Source]({source})";
            string parsed_derpibooru_source = config.imgur.useimgur != 1
                ? Empty
                : $"| [Derpibooru Link]({urltype}://{domain}/{top.id})";
            string comment = $"[](/sweetiecardbot) {parsedsource} {parsed_derpibooru_source} " +
                             "\r\n  \r\n" +
                             "[](/sp)" +
                             "\r\n  \r\n" +
                             "---" +
                             "\r\n  \r\n" +
                             $"This is a bot | [Info](http://club-flank.com/sweetiebot/) | [Report problems](/message/compose/?to=BitzLeon&subject=_sweetiebot running Derpbot {version}) | [Source code](https://google.com)";


            BotWebAgent webAgent = new BotWebAgent(
                reddit_username,
                reddit_password,
                client_id,
                secret_key,
                callback_url);
            //Create reddit client instance
            Reddit reddit = new Reddit(webAgent, true);
            //Login to reddit
            reddit.LogIn(reddit_username, reddit_password);
            //Check to see if we logged in properly
            if (reddit.User.FullName == reddit_username)
            {
                WriteLine("Logged into Reddit.");
                Subreddit subreddit = reddit.GetSubredditAsync(reddit_sub).Result;

                try
                {
                    WriteLine("Posting to reddit");
                    RedditSharp.Things.Post post = subreddit.SubmitPost(postTitle, hostedImageLink);
                    WriteLine("Commenting on post on reddit");
                    post.Comment(comment);
                }
                catch (Exception e)
                {
                    WriteLine(e.Message);
                    log.WriteLine(e.Message);
                }
            }
            else
            {
                WriteLine("Something happened trying to log in to reddit.");
            }



            log.WriteLine("Application exited normally.");
            WriteLine("Application exited normally.");
            ReadLine();
        }
    }
}