using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerpBot.Models
{
    public class Request
    {
        public class Imgur
        {
            public string ApiKey { get; set; }
            public string Url { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }

        public class Gfycat
        {
            public string Title { get; set; }
            public string ClientId { get; set; }
            public string ApiKey { get; set; }
            public string ImageUrl { get; set; }
        }
    }
}
