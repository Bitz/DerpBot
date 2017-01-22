using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DerpBot.Models;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using Newtonsoft.Json;


namespace DerpBot.Functions
{
    public class Post
    {
        public static async Task<IImage> PostToImgur(Request.Imgur model)
        {
            ImgurClient client = new ImgurClient(model.ApiKey);
            ImageEndpoint endpoint = new ImageEndpoint(client);
            IImage image = await endpoint.UploadImageUrlAsync(model.Url, null, model.Title, model.Description);
            return image;
        }

        public static string PostToGfycat(Request.Gfycat model)
        {
            string token = Get.GetGfycatToken(model);
            string text = null;
            if (token.Length > 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.gfycat.com/v1/gfycats");
                    request.Method = WebRequestMethods.Http.Post;
                    request.Accept = "application/json";
                    request.UserAgent = "curl/7.37.0";
                    request.ContentType = "application/json";


                    client.DefaultRequestHeaders.Authorization  = AuthenticationHeaderValue.Parse($"Bearer {token}");
                    Dictionary<string, string> createDictionary = new Dictionary<string, string>
                    {
                        {"noMd5", "true"},
                        {"noResize", "true"},
                        {"nsfw", "3"},
                        {"fetchUrl", model.ImageUrl},
                        {"title", model.Title}
                    };

                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {

                        streamWriter.Write(JsonConvert.SerializeObject(createDictionary, Formatting.Indented));
                    }
                    

                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        // ReSharper disable once AssignNullToNotNullAttribute
                        using (var sr = new StreamReader(response.GetResponseStream()))
                        {
                            text = sr.ReadToEnd();
                        }
                    }
                }
            }
            Gfycat.Response.Rootobject result =  JsonConvert.DeserializeObject<Gfycat.Response.Rootobject>(text);

            if (result.isOk)
            {
                bool isDone = false;
                string cUrl = $"https://api.gfycat.com/v1/gfycats/fetch/status/{result.gfyname}";
                while (!isDone)
                {
                    HttpWebRequest r = (HttpWebRequest)WebRequest.Create(cUrl);
                    using (HttpWebResponse k = (HttpWebResponse)r.GetResponse())
                    {
                        if (k.StatusCode == HttpStatusCode.OK)
                        {
                            using (var sr = new StreamReader(k.GetResponseStream()))
                            {
                                text = sr.ReadToEnd();
                            }
                            if (text.Contains("complete"))
                            {
                                isDone = true;
                            }
                            else
                            {
                                Thread.Sleep(500);
                            }
                        }
                    }
                }
                
                //request.BeginGetResponse(new AsyncCallback(FinishRequest), request);
            }

            return $"https://gfycat.com/{result.gfyname}";
        }
    }
}