using System.Xml;
using DerpBot.Models;

namespace DerpBot.Functions
{
    class Load
    {
        public static configuration Config()
        {
            string directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            XmlDocument config = new XmlDocument();
            string slash = Get.IsRunningOnMono() ? @"\" : @"/";
            config.Load($@"{directory}{slash}Configuration.xml");
            XmlNode node = config.DocumentElement;
            return Convert.ConvertNode<configuration>(node);
        }
    }
}
