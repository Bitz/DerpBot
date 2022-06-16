using System;
using Imgur.API.Models;
using Newtonsoft.Json;
using static System.DateTime;
using static System.Console;
using static System.String;
using Post = DerpBotCore.Functions.Post;
using System.Collections.Generic;
using System.IO;
using DerpBotCore.Functions;
using DerpBotCore.Models;
using System.Linq;
using Reddit;
using Reddit.Controllers;

namespace DerpBotCore
{
    class Program
    {
        static void Main(string[] args)
        {
            const string version = "0.2.0";
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
            string domain = "derpibooru.org";

            string imgurApiKey = config.imgur.apikey;
            bool rehostOnImgur = config.imgur.useimgur == 1;

            string clientId = config.reddit.client_id;
            string refreshToken = config.reddit.refresh_token;
            string accessToken = config.reddit.access_token;
            string secretKey = config.reddit.secret_id;
            StreamWriter logfile = Functions.Create.Log(loggingpath);

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
                WriteLine($"Running for subreddit /r/{config.subreddit_configurations[argumentIndex].subreddit}");
            }
            catch (Exception e)
            {
                WriteLine($"Error loading subreddit config: {e.Message}");
                ReadLine();
                Environment.Exit(1);
            }
            string redditSub = config.subreddit_configurations[argumentIndex].subreddit;
            string timeFrame = config.subreddit_configurations[argumentIndex].timeframe;
            List<string> sensitivetags =
                config.subreddit_configurations[argumentIndex].sensitivetags.Split(',').ToList();
            //daily, weekly, monthly, off 
            //Daily will look at yesterdays post
            //Weekly will look at the posts for the last 7 days (if ran on 10th, will get top posts between 3rd-10th)
            //Monthly will look at last months posts (if ran in September, you will get August's top posts)
            string tags = config.subreddit_configurations[argumentIndex].tags;
            DateTime minage;
            switch (timeFrame)
            {
                case "daily":
                    minage = Now.AddDays(-1);
                    tags += $", created_at:{minage:yyyy-MM-dd}";
                    break;
                case "weekly":
                    minage = Now.AddDays(-7);
                    tags += $", created_at:{minage:yyyy-MM-dd}";
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
            tags += ", -animated";

            //Build the request url for Derpibooru
            string requestUrl =
                $"https://{domain}/api/v1/json/search/images?q={tags}&key={derpibooruApiKey}&sf=score&sd=desc&perpage=15";

            //Why not shove the api call method, json serialization and object lambada selection all in one statment!? MWHAHAHAHA
            var top = JsonConvert.DeserializeObject<Derpibooru>(Get.Derpibooru(requestUrl).Result).Images.First();

            //Find the artist data in the tags that were pulled from Derpibooru
            List<string> topImageTags = top.Tags.Select(x => x.Trim()).ToList();
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
                        artistString += $"{a.Replace("artist:", Empty)} + ";
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
            string source = top.SourceUrl.Length > 0 ? top.SourceUrl : Empty;

            string hostedImageLink;
            WriteLine($"Top found with ID {top.Id}");
            if (rehostOnImgur)
            {
                //Build imgur request
                Request.Imgur imgurRequest = new Request.Imgur
                {
                    ApiKey = imgurApiKey,
                    Description = source,
                    Title = postTitle,
                    Url = top.ViewUrl
                };

                //Post to imgur
                IImage hostedImage = null;
                try
                {
                    hostedImage = Post.PostToImgur(imgurRequest).Result;
                }
                catch (AggregateException e)
                {
                    WriteLine(e);
                    imgurRequest.Url = $"{top.Representations.Large}";
                    hostedImage = Post.PostToImgur(imgurRequest).Result;
                }
                WriteLine($"Image uploaded to Imgur: {hostedImage?.Link}");
                hostedImageLink = hostedImage?.Link;
                //Keep the delete hash in a text file, incase someone doesn't want us reposting their content
                HashDb.Write($"{hostedImage?.Link} http://imgur.com/delete/{hostedImage?.DeleteHash}");
            }
            else
            {
                //If we aren't using imgur to host, we are just going to build a link to derpibooru instead
                hostedImageLink = $"https://{domain}/images/{top.Id}";
            }

            //Building all the reddit request parts
            string parsedSource = source == Empty ? "No source provided" : $"[Original Source]({source})";
            string parsedDerpibooruSource = config.imgur.useimgur != 1
                ? Empty
                : $"[Derpibooru Link](https://{domain}/images/{top.Id})";
            string comment = $"[](/sweetiecardbot) {parsedSource} | {parsedDerpibooruSource} " +
                             "\r\n  \r\n" +
                             "---" +
                             "\r\n  \r\n" +
                             $"This is a bot | [Drawfiend](https://drawfiend.com) |[Info](https://bitz.rocks/derpbot/) | [Report problems](/message/compose/?to=BitzLeon&subject=Derpbot {version}) | [Source code](https://github.com/Bitz/DerpBot)";

            //Create reddit client instance
            var reddit = new RedditClient(clientId, refreshToken, secretKey, accessToken);

            //Check to see if we logged in properly
            if (!IsNullOrEmpty(reddit.Account?.Me?.Name))
            {
                WriteLine("Logged into Reddit.");
                var subreddit = reddit.Subreddit(redditSub).About();

                var newPosts =
                    subreddit.Posts.New.Where(x => x.NSFW && !x.Listing.IsSelf && x.Created >= minage).ToList();
                WriteLine($"New posts to compare to: {newPosts.Count}");
                List<bool> currentImageHash = Get.GetHash(hostedImageLink);
                bool duplicateFound = false;
                foreach (var post in newPosts)
                {
                    WriteLine($"Comparing post: {post.Title}");
                    var temp = Get.ImageFromPost.GetImageUrl((LinkPost) post);
                    if (temp.IsValid)
                    {
                        double equalElements =
                            currentImageHash.Zip(Get.GetHash(temp.Url), (i, j) => i == j).Count(eq => eq) / 256.00;
                        WriteLine($"{equalElements:P2} similar");
                        if (equalElements >= 0.99)
                        {
                            duplicateFound = true;
                            break;
                        }
                    }
                }
                if (!duplicateFound)
                {
                    try
                    {
                        WriteLine("Posting to reddit");
                        var post = subreddit.LinkPost(postTitle, hostedImageLink).Submit();
                        WriteLine("Commenting on post on Reddit");
                        post.Comment(comment);
                    }
                    catch (ArithmeticException e)
                    {
                        WriteLine(e.Message);
                        log.WriteLine(e.Message);
                    }
                }
                else
                {
                    //TODO Use delete hash to remove image from imgur? would be nice for them.
                    WriteLine("Did not post to Reddit. Duplicate found.");
                }
            }
            else
            {
                WriteLine("Something bad happened while trying to log in to Reddit.");
            }
            log.WriteLine("Application exited normally.");
            WriteLine("Application exited normally.");
        }
    }
}
