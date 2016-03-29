using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace Beaver_Downloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private XmlData xmlData;
        private Downloader downloader;
        public string path { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            xmlData = new XmlData(FilesProvider);
            downloader = new Downloader(new HttpClient(), xmlData);
        }

        private async void addRowButton_Click(object sender, RoutedEventArgs e)
        {
            string url = urlBox.Text;
            HttpResponseMessage headers = null;

            // Make a request to get the Headers
            try
            {
                headers = await downloader.GetFileHeaders(url);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            // Extract variables from the headers
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads\";
            string fileName = headers.Content.Headers.ContentDisposition.FileName.Replace("\"", "");
            long size = headers.Content.Headers.ContentLength.Value;

            // Add a node to the xml file
            xmlData.AddRow(url, dir, fileName, 0, size);
        }

        private async void startButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the file Id
            string id = GetFileId(sender);

            // Start to download the file
            try
            {
                await downloader.GetFile(id);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the File Id
            string id = GetFileId(sender);

            // Remove the node from the xml file
            xmlData.RemoveRow(id);
        }

        /// <summary>
        /// Get the id from the Tag of the passed button control
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string GetFileId(object obj)
        {
            // Cast the obj to a Button
            Button button = (Button)obj;

            // Cast the Tag to an XmlAttribute to get the value
            XmlAttribute attribute = (XmlAttribute)button.Tag;

            // Get the value
            string id = attribute.Value;

            return id;
        }
    }
}
