using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Windows.Data;
using System.Xml;
using System.Windows;

namespace Beaver_Downloader
{
    public class XmlData
    {
        private string xmlPath;
        private XmlDataProvider xmlDataProvider;

        public XmlData(XmlDataProvider xmlDataProvider)
        {
            this.xmlDataProvider = xmlDataProvider;
            SetPath();
        }

        /// <summary>
        /// Create the xml file if not present and set it as the source for the XmlDataProvider
        /// </summary>
        /// <param name="provider"></param>
        public void SetPath()
        {
            // Set the path for the file used to store the data
            string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Beaver Downloader\\";
            xmlPath = directoryPath + "Files.xml";

            // Find or create a new directory for the xml file
            if(!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Find or create the xml file to store the data
            if(!File.Exists(xmlPath))
            {
                XElement xml = new XElement("Files");
                xml.Save(xmlPath);
            }

            // Set the generated path as the xmlDataProvider source
            xmlDataProvider.Source = new Uri(xmlPath);
        }

        /// <summary>
        /// Get the node with the passed id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public XmlNode GetRow(string id)
        {
            // Set the XPath
            string xPath = String.Format("//File[@id={0}]", id);

            // Get the Node
            XmlNode node = xmlDataProvider.Document.SelectSingleNode(xPath);

            return node;
        }

        /// <summary>
        /// Add an element to the xml file and save it
        /// </summary>
        /// <param name="urlText"></param>
        public void AddRow(string url, string dir, string name, long currentByte, long totalBytes)
        {
            // Set the root Document
            XmlElement files = xmlDataProvider.Document.DocumentElement;

            // Create a new file entry
            XmlElement file = xmlDataProvider.Document.CreateElement("File");

            // Create an id attribute
            string id;

            if(files.ChildNodes.Count > 0)
            {
                int lastId = Int32.Parse((xmlDataProvider.Document.DocumentElement.LastChild.Attributes["id"].Value));
                id = (lastId + 1).ToString();
            }
            else
            {
                id = "0";
            }

            file.SetAttribute("id", id);

            // Create a Url node
            XmlElement urlNode = xmlDataProvider.Document.CreateElement("Url");
            XmlText urlValue = xmlDataProvider.Document.CreateTextNode(url);
            urlNode.AppendChild(urlValue);

            // Create a Dir node
            XmlElement dirNode = xmlDataProvider.Document.CreateElement("Dir");
            XmlText dirValue = xmlDataProvider.Document.CreateTextNode(dir);
            dirNode.AppendChild(dirValue);

            // Create a Name node
            XmlElement nameNode = xmlDataProvider.Document.CreateElement("Name");
            XmlText nameValue = xmlDataProvider.Document.CreateTextNode(name);
            nameNode.AppendChild(nameValue);

            // Create a CurrentByte node
            XmlElement currentByteNode = xmlDataProvider.Document.CreateElement("CurrentByte");
            XmlText currentByteValue = xmlDataProvider.Document.CreateTextNode(currentByte.ToString());
            currentByteNode.AppendChild(currentByteValue);

            // Create a TotalBytes node
            XmlElement totalBytesNode = xmlDataProvider.Document.CreateElement("TotalBytes");
            XmlText totalByteValue = xmlDataProvider.Document.CreateTextNode(totalBytes.ToString());
            totalBytesNode.AppendChild(totalByteValue);

            // Append attributes and nodes
            file.AppendChild(urlNode);
            file.AppendChild(dirNode);
            file.AppendChild(nameNode);
            file.AppendChild(currentByteNode);
            file.AppendChild(totalBytesNode);
            files.AppendChild(file);

            // Save the xml document
            xmlDataProvider.Document.Save(xmlPath);
        }

        /// <summary>
        /// Update the xml node with the passed values and save it
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentByte"></param>
        public void UpdateRow(string id, long currentByte)
        {
            // Select the node with the passed id
            XmlNode node = this.GetRow(id);

            // Get the currentByte node from the collection
            XmlNode currentByteNode = node["CurrentByte"];

            // Update the node
            currentByteNode.InnerText = currentByte.ToString();

            // Save the xml Document
            xmlDataProvider.Document.Save(xmlPath);
        }

        /// <summary>
        /// Remove the the xml node with the passed id and save it
        /// </summary>
        /// <param name="id"></param>
        public void RemoveRow(string id)
        {
            // Select the node with the passed id
            XmlNode node = this.GetRow(id);

            // Remove the selected node
            xmlDataProvider.Document.DocumentElement.RemoveChild(node);

            // Save the xml document
            xmlDataProvider.Document.Save(xmlPath);
        }
    }
}
