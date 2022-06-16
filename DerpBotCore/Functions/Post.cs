using System.Net.Http;
using System.Threading.Tasks;
using DerpBotCore.Models;
using Imgur.API.Authentication;
using Imgur.API.Endpoints;
using Imgur.API.Models;


namespace DerpBotCore.Functions
{
    public class Post
    {
        public static async Task<IImage> PostToImgur(Request.Imgur model)
        {
            var client = new ApiClient(model.ApiKey);
            using (var webClient = new HttpClient())
            {
                var imageEndpoint = new ImageEndpoint(client, webClient);
                var stream = await webClient.GetStreamAsync(model.Url);
                IImage image = await imageEndpoint.UploadImageAsync(stream, null, model.Title, model.Description);
                return image;
            }
        }
    }
}