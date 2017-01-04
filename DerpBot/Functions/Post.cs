using System.Threading.Tasks;
using DerpBot.Models;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;


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
        }
}
