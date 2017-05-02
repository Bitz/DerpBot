using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DerpBot.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace DerpBot.Functions
{
    public class Get
    {
        public static async Task<string> Derpibooru(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                string type = "application/json";
                client.BaseAddress = new Uri(url);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(type));
                HttpResponseMessage response = await client.GetAsync(String.Empty);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                return string.Empty;
            }
        }

        public static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }



        public static List<bool> GetHash(string url)
        {
            //Download the image...
            List<bool> lResult = new List<bool>();
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            if (responseStream != null)
            {
            Bitmap bmpSource = new Bitmap(responseStream);
                //TODO Maybe someday fix thumbnail support for performance...
            //if (bmpSource.Width != bmpSource.Height)
            //{
            //   Bitmap bitmap = BitmapTools.CropFunction(bmpSource);
               
            //    bmpSource = bitmap;
            //    bitmap.Save(@"C:\test.png", ImageFormat.Png);
            //}
                //create new image with 16x16 pixel
                Bitmap bmpMin = new Bitmap(bmpSource, new Size(16, 16));
                for (int j = 0; j < bmpMin.Height; j++)
                {
                    for (int i = 0; i < bmpMin.Width; i++)
                    {
                        //reduce colors to true / false                
                        lResult.Add(bmpMin.GetPixel(i, j).GetBrightness() < 0.5f);
                    }
                }
                
            }
            return lResult;
        }

        //public class BitmapTools
        //{
        //    public static Bitmap CropFunction(Bitmap bmpSource)
        //    {
        //        int targetHeight = 140;
        //        int y = bmpSource.Height;
        //        int x = bmpSource.Width;
        //        while (y > targetHeight)
        //        {
        //            int sliceHeight = Math.Min(y - targetHeight, 10);
        //            Bitmap bottom = CropBitmap(bmpSource, new Rectangle(0, y - sliceHeight, x, y));
        //            Bitmap top = CropBitmap(bmpSource, new Rectangle(0, 0, x, sliceHeight));
        //            if (GetEntropyValue(bottom) < GetEntropyValue(top))
        //            {
        //                bmpSource = CropBitmap(bmpSource, new Rectangle(0, 0, x, y - sliceHeight));
        //            }
        //            else
        //            {
        //                bmpSource = CropBitmap(bmpSource, new Rectangle(0, sliceHeight, x, y));
        //            }
        //            y = bmpSource.Height;
        //            x = bmpSource.Width;
        //        }
        //        Bitmap result = new Bitmap(bmpSource);
        //        return result;
        //    }

        //    public static Bitmap CropBitmap(Bitmap bmpSource, Rectangle r)
        //    {
        //        using (Graphics g = Graphics.FromImage(bmpSource))
        //        {
        //            g.SmoothingMode = SmoothingMode.HighQuality;
        //            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        //            g.CompositingQuality = CompositingQuality.HighQuality;
        //            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            g.DrawImage(bmpSource, new Rectangle(0, 0, bmpSource.Width, bmpSource.Height), r, GraphicsUnit.Pixel);
        //        }
        //        return bmpSource;
        //    }
        //    public static int GetEntropyValue(Bitmap b)
        //    {
        //        int trues = 0;
        //        Bitmap bmpMin = new Bitmap(b, new Size(16, 16));
        //        for (int j = 0; j < bmpMin.Height; j++)
        //        {
        //            for (int i = 0; i < bmpMin.Width; i++)
        //            {
        //                if (bmpMin.GetPixel(i, j).GetBrightness() < 0.5f)
        //                {
        //                    trues++;
        //                }
        //            }
        //        }
        //        return trues;
        //    }
            
        //} 

        

        public static string GetGfycatToken(Request.Gfycat requestData)
        {
            string j = "application/json";
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://api.gfycat.com/v1/oauth/token");
            request.Method = WebRequestMethods.Http.Post;
            request.Accept = j;
            request.UserAgent = "curl/7.37.0";
            request.ContentType = j;
            Dictionary<string, string> createDictionary = new Dictionary<string, string>
                {
                    {"grant_type", "client_credentials"},
                    {"client_id", requestData.ClientId},
                    {"client_secret", requestData.ApiKey}
                };

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                
                streamWriter.Write(JsonConvert.SerializeObject(createDictionary, Formatting.Indented));
            }
            string text;
          
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    text = sr.ReadToEnd();
                }
            }
            return JsonConvert.DeserializeObject<Gfycat.Response.Token>(text).access_token;
        }

        public class ImageFromPost
        {
            public class ImageUrl
            {
                public string Url { get; set; }
                public bool IsValid { get; set; }
            }
            public static ImageUrl GetImageUrl(RedditSharp.Things.Post post)
            {
                ImageUrl response = new ImageUrl();
                List<string> imageList = new List<string> { ".jpg", ".png", ".gif" };
                string returnUrl;
                string path = post.Url.ToString();
                string extension = Path.GetExtension(path);
                //if (!String.IsNullOrEmpty(post.Thumbnail.ToString()) && post.Thumbnail.ToString() != "default")
                //{
                 //   returnUrl = post.Thumbnail.ToString();
                //}
                //else
                //{
                    returnUrl = imageList.Any(x => x.Equals(extension) && !String.IsNullOrEmpty(extension)) ? post.Url.ToString() : GetOg(post.Url.ToString());
               // }
                Uri test;
                response.IsValid = Uri.TryCreate(returnUrl, UriKind.Absolute, out test) && (test.Scheme == Uri.UriSchemeHttp || test.Scheme == Uri.UriSchemeHttps);
                response.Url = returnUrl;
                return response;
            }

            private static string GetOg(string url)
            {
                if (url.Contains("mobile.twitter.com"))
                {
                    url = url.Replace("mobile.", String.Empty);
                }
                string resultUrl = "";
                string html = FetchHtml(url);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                HtmlNodeCollection list = doc.DocumentNode.SelectNodes("//meta");
                if (list == null) return string.Empty;

                HtmlNode first = list.First(x => x.Attributes["property"]?.Value == "og:image");
                
              
                if (first != null)
                {
                    resultUrl = first.Attributes["content"].Value;
                }
                return resultUrl;
            }

            private static string FetchHtml(string url)
            {
                string o = "";

                try
                {
                    HttpWebRequest oReq = (HttpWebRequest)WebRequest.Create(url);
                    oReq.UserAgent = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";
                    HttpWebResponse resp = (HttpWebResponse)oReq.GetResponse();
                    Stream stream = resp.GetResponseStream();
                    if (stream != null)
                    {
                        StreamReader reader = new StreamReader(stream);
                        o = reader.ReadToEnd();
                    }
                }
                catch (Exception)
                {
                    // ignored
                }

                return o;
            }


        }
    }
}
