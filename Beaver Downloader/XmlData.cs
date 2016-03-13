using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Windows.Data;
using System.Xml;

namespace Beaver_Downloader
{
    public class XmlData
    {
        private string path { get; set; }
        private XmlDataProvider xmlFile { get; set; }

        /// <summary>
        /// Create the xml file if not present and set the variables
        /// </summary>
        /// <param name="provider"></param>
        public void setPath(XmlDataProvider provider)
        {
            xmlFile = provider;
            string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Beaver Downloader\\";
            path = directoryPath + "Files.xml";

            if(!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if(!File.Exists(path))
            {
                XElement xml = new XElement("Files");
                xml.Save(path);
            }

            provider.Source = new Uri(path);
        }

        /// <summary>
        /// Add an element to the xml file
        /// </summary>
        /// <param name="urlText"></param>
        public void AddRow(string urlText)
        {
            XmlElement file = xmlFile.Document.CreateElement("File");

            XmlElement url = xmlFile.Document.CreateElement("Url");
            XmlText urlValue = xmlFile.Document.CreateTextNode(urlText);
            url.AppendChild(urlValue);

            XmlElement progress = xmlFile.Document.CreateElement("Progress");
            XmlText progressValue = xmlFile.Document.CreateTextNode("0");
            progress.AppendChild(progressValue);

            file.AppendChild(url);
            file.AppendChild(progress);
            xmlFile.Document.DocumentElement.AppendChild(file);

            xmlFile.Document.Save(path);
        }

    }
}
