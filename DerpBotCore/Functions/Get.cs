using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Reddit.Controllers;

namespace DerpBotCore.Functions
{
    public class Get
    {
        static readonly List<string> ImageList = new List<string> { ".jpg", ".png", ".gif", ".jepg" };
        public static async Task<string> Derpibooru(string url)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Top Yiff Bot/1.0 (by @ClubFlank on Twitter)");
            return await client.GetStringAsync(url);
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


        public class ImageFromPost
        {
            public class ImageUrl
            {
                public string Url { get; set; }
                public bool IsValid { get; set; }
            }
            public static ImageUrl GetImageUrl(LinkPost post)
            {
                ImageUrl response = new ImageUrl();
                string returnUrl = post.URL;
                response.IsValid = ImageList.Any(x => returnUrl.EndsWith(x));
                response.Url = returnUrl;
                return response;
            }
        }
    }
}
