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
        private string xmlPath { get; set; }
        private XmlDataProvider xmlFile { get; set; }

        /// <summary>
        /// Create the xml file if not present and set it as the source for the XmlDataProvider
        /// </summary>
        /// <param name="provider"></param>
        public void setPath(XmlDataProvider provider)
        {
            xmlFile = provider;
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
            provider.Source = new Uri(xmlPath);
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
            XmlNode node = xmlFile.Document.SelectSingleNode(xPath);

            return node;
        }

        /// <summary>
        /// Add an element to the xml file and save it
        /// </summary>
        /// <param name="urlText"></param>
        public void AddRow(string url, string dir, string name, long currentByte, long totalBytes)
        {
            // Set the root Document
            XmlElement files = xmlFile.Document.DocumentElement;

            // Create a new file entry
            XmlElement file = xmlFile.Document.CreateElement("File");

            // Create an id attribute
            string id;

            if(files.ChildNodes.Count > 0)
            {
                int lastId = Int32.Parse((xmlFile.Document.DocumentElement.LastChild.Attributes["id"].Value));
                id = (lastId + 1).ToString();
            }
            else
            {
                id = "0";
            }

            XmlAttribute idAttribute = xmlFile.Document.CreateAttribute("id");
            idAttribute.Value = id;

            // Create a Url node
            XmlElement urlNode = xmlFile.Document.CreateElement("Url");
            XmlText urlValue = xmlFile.Document.CreateTextNode(url);
            urlNode.AppendChild(urlValue);

            // Create a Dir node
            XmlElement dirNode = xmlFile.Document.CreateElement("Dir");
            XmlText dirValue = xmlFile.Document.CreateTextNode(dir);
            dirNode.AppendChild(dirValue);

            // Create a Name node
            XmlElement nameNode = xmlFile.Document.CreateElement("Name");
            XmlText nameValue = xmlFile.Document.CreateTextNode(name);
            nameNode.AppendChild(nameValue);

            // Create a CurrentByte node
            XmlElement currentByteNode = xmlFile.Document.CreateElement("CurrentByte");
            XmlText currentByteValue = xmlFile.Document.CreateTextNode(currentByte.ToString());
            currentByteNode.AppendChild(currentByteValue);

            // Create a TotalBytes node
            XmlElement totalBytesNode = xmlFile.Document.CreateElement("TotalBytes");
            XmlText totalByteValue = xmlFile.Document.CreateTextNode(totalBytes.ToString());
            totalBytesNode.AppendChild(totalByteValue);

            // Append attributes and nodes
            file.Attributes.Append(idAttribute);
            file.AppendChild(urlNode);
            file.AppendChild(dirNode);
            file.AppendChild(nameNode);
            file.AppendChild(currentByteNode);
            file.AppendChild(totalBytesNode);
            files.AppendChild(file);

            // Save the xml document
            xmlFile.Document.Save(xmlPath);
        }

        /// <summary>
        /// Update the xml node with the passed values and save it
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentByte"></param>
        public void updateRow(string id, long currentByte)
        {
            // Select the node with the passed id
            XmlNode node = this.GetRow(id);

            // Update the currentByte node
            XmlNode currentByteNode = node["CurrentByte"];
            currentByteNode.InnerText = currentByte.ToString();

            // Save the xml Document
            xmlFile.Document.Save(xmlPath);
        }

        /// <summary>
        /// Remove the the xml node with the passed id and save it
        /// </summary>
        /// <param name="id"></param>
        public void removeRow(string id)
        {
            // Select the node with the passed id
            XmlNode node = this.GetRow(id);

            // Remove the selected node
            xmlFile.Document.DocumentElement.RemoveChild(node);

            // Save the xml document
            xmlFile.Document.Save(xmlPath);
        }
    }
}
