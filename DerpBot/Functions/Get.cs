using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DerpBot.Models;
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

        public static List<bool> GetHash(Bitmap bmpSource)
        {
            List<bool> lResult = new List<bool>();
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
            return lResult;
        }

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
    }
}
