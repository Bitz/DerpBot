using System;
using System.IO;
using System.Reflection;
using System.Xml;
using DerpBotCore.Models;

namespace DerpBotCore.Functions
{
    class Load
    {
        public static configuration Config()
        {
            string directory = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            XmlDocument config = new XmlDocument();
            config.Load(Path.Combine(directory ?? throw new InvalidOperationException(), "Configuration.xml"));
            XmlNode node = config.DocumentElement;
            return Convert.ConvertNode<configuration>(node);
        }
    }
}
