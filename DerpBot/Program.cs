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
            const string version = "0.3";
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
            const string loggingpath = @"logs";
            string derpibooruApiKey = config.derpibooru.apikey;
            string urltype = config.derpibooru.type;
            string domain = config.derpibooru.domain;
            //List<string> imageExtractableDomains = new List<string> { "imgur.com", "derpibooru.org", "i.imgur.com" };

            string imgurApiKey = config.imgur.apikey;
            bool rehostOnImgur = config.imgur.useimgur == 1;

            string redditUsername = config.reddit.username;
            string redditPassword = config.reddit.password;
            string clientId = config.reddit.client_id;
            string secretKey = config.reddit.secret_id;
            string callbackUrl = config.reddit.callback_url;

            StreamWriter logfile = Create.Log(loggingpath);

            logfile.AutoFlush = true;
            Log log = new Log(logfile);

            SetOut(new PrefixedWriter());
            log.WriteLine($"Log created {Now:MM:dd:yyyy}");

            int argumentIndex = 0;
            if (args.Length > 0)
            {
                if (int.TryParse(args[0], out argumentIndex))
                {
                    WriteLine("Found valid argument!");
                }
            }

            try
            {
                WriteLine($"Running for subreddit /r/{config.subreddit_configurations.sub[argumentIndex].subreddit}");
            }
            catch (Exception e)
            {
                WriteLine($"Error loading subreddit config: {e.Message}");
                ReadLine();
                Environment.Exit(1);
            }
            string method = config.subreddit_configurations.sub[argumentIndex].method;
            string redditSub = config.subreddit_configurations.sub[argumentIndex].subreddit;
            string timeFrame = config.subreddit_configurations.sub[argumentIndex].timeframe;
            List<string> sensitivetags = config.subreddit_configurations.sub[argumentIndex].sensitivetags.Split(',').ToList();
            //daily, weekly, monthly, off 
            //Daily will look at yesterdays post
            //Weekly will look at the posts for the last 7 days (if ran on 10th, will get top posts between 3rd-10th)
            //Monthly will look at last months posts (if ran in September, you will get August's top posts)
            string tags = config.subreddit_configurations.sub[argumentIndex].tags;
            DateTime minage;
            switch (timeFrame)
            {
                case "daily":
                    minage = Now.AddDays(-1);
                    tags += $", created_at:{minage:yyyy-MM-dd}";
                    break;
                case "weekly":
                    minage = Now.AddDays(-7);
                    tags += $", created_at.gte:{minage:yyyy-MM-dd}";
                    break;
                case "monthly":
                    minage = Now.AddMonths(-1);
                    tags += $", created_at:{minage:yyyy-MM}";
                    break;
                default:
                    minage = Now.AddDays(-1);
                    tags += $", created_at:{minage:yyyy-MM-dd}";
                    break;
            }

            //Build the request url for Derpibooru
            string requestUrl = $"{urltype}://{domain}/{method}?q={tags}&key={derpibooruApiKey}&sf=score&sd=desc&perpage=15";


            //Why not shove the api call method, json serialization and object lambada selection all in one statment!? MWHAHAHAHA
            DerpibooruResponse.Search top = 
            JsonConvert.DeserializeObject<DerpibooruResponse.Rootobject>(Get.Derpibooru(requestUrl).Result).search.First();

            //List<DerpibooruResponse.Search> listing =
            //    JsonConvert.DeserializeObject<DerpibooruResponse.Rootobject>(Get.Derpibooru(requestUrl).Result).search.ToList();
            ////Testing advanced sorting.
            //DateTime now = Now;
            //List<Test> tests = new List<Test>();
            //double t = TimeSpan.FromDays(1).TotalSeconds;
            //foreach (DerpibooruResponse.Search item in listing)
            //{
            //    TimeSpan k = now - item.created_at;
            //    double positivity = (double) item.upvotes/(item.upvotes + item.downvotes);
            //    double popularityRating = Math.Log10(positivity + k.TotalSeconds / t); 
            //    WriteLine($"ID: {item.id}");
            //    WriteLine($"Positivity rate: {positivity:P}");
            //    //WriteLine($"Popularity rating: {popularityRating}");
            //    WriteLine("-------------------------------------");
            //    tests.Add(new Test {item = item, Popularity = popularityRating});
            //}
            //WriteLine("-------------------------------------");
            //WriteLine("-------------------------------------");
            //WriteLine("-------------------------------------");
            //WriteLine("-------------------------------------");
            //WriteLine("-------------------------------------");
            //tests = new List<Test>(tests.OrderByDescending(o => o.Popularity));
            //foreach (var i in tests)
            //{
            //    WriteLine($"ID: {i.item.id}");
            //    WriteLine($"Score: {i.Popularity:P}");
            //    WriteLine("-------------------------------------");
            //}
            
            //Find the artist data in the tags that were pulled from Derpibooru
            List<string> topImageTags = top.tags.Split(',').Select(x => x.Trim()).ToList();
            List<string> artists = topImageTags.Where(x => x.Contains("artist:")).ToList();
            string artistString = null;
            if (artists.Count == 0)
            {
                artistString = "Unknown";
            }
            if (artists.Count == 1)
            {
                
                artistString = artists.Single().Replace("artist:", Empty);
            }
                
            if (artists.Count > 1)
            {
                foreach (var a in artists)
                {
                    if (a != artists.Last())
                    {
                        artistString += $"{a.Replace("artist:", Empty)} +";
                    }
                    else
                    {
                        artistString += a.Replace("artist:", Empty);
                    }
                }
            }
            string sensitiveTitle = Empty;
            if (topImageTags.Intersect(sensitivetags).Any())
            {
                foreach (string match in topImageTags.Intersect(sensitivetags))
                {
                    sensitiveTitle = $"[{match.ToUpper()}] ";
                }
            }
            string postTitle = $"Top Image of {Now.AddDays(-1):MM-dd-yyyy} [Artist: {artistString}] {sensitiveTitle}";
            string source = top.source_url.Length > 0 ? top.source_url : Empty;

            string hostedImageLink;
            WriteLine($"Top found with ID {top.id}");
            if (rehostOnImgur)
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
            string parsedSource = source == Empty ? "No source provided" : $"[Original Source]({source})";
            string parsedDerpibooruSource = config.imgur.useimgur != 1
                ? Empty
                : $"| [Derpibooru Link]({urltype}://{domain}/{top.id})";
            string comment = $"[](/sweetiecardbot) {parsedSource} {parsedDerpibooruSource} " +
                             "\r\n  \r\n" +
                             "---" +
                             "\r\n  \r\n" +
                             $"This is a bot | [Info](https://bitz.rocks/derpbot/) | [Report problems](/message/compose/?to=BitzLeon&subject=_sweetiebot running Derpbot {version}) | [Source code](https://github.com/Bitz/DerpBot)";


            BotWebAgent webAgent = new BotWebAgent(
                redditUsername,
                redditPassword,
                clientId,
                secretKey,
                callbackUrl);
            //Create reddit client instance
            Reddit reddit = new Reddit(webAgent, true);
            //Login to reddit
            reddit.LogIn(redditUsername, redditPassword);
            //Check to see if we logged in properly
            if (reddit.User.FullName.ToLower() == redditUsername)
            {
                WriteLine("Logged into Reddit.");
                Subreddit subreddit = reddit.GetSubredditAsync(redditSub).Result;
                //TODO: Working on image matching.
                //List<RedditSharp.Things.Post> newPosts = subreddit.Search(minage, Now).ToList();
                //newPosts = newPosts.Where(x => !x.IsSelfPost && imageExtractableDomains.Contains(x.Domain)).ToList();


                //WriteLine($"posts matching amount: {newPosts.Count}");

                //foreach (var post in newPosts)
                //{
                //    WriteLine(post.Title);
                //    WriteLine(post.Domain);
                //    WriteLine(post.Url);
                //}
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
        }
    }
}