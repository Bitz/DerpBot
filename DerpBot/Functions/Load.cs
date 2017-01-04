using System.Xml;
using DerpBot.Models;

namespace DerpBot.Functions
{
    class Load
    {
        public static configuration Config()
        {
            XmlDocument config = new XmlDocument();
            config.Load("Configuration.xml");
            XmlNode node = config.DocumentElement;
            return Convert.ConvertNode<configuration>(node);
        }
    }
}
