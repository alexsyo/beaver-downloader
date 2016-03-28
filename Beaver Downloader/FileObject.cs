using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Beaver_Downloader
{
    public class FileObject
    {
        private int id { get; set; }
        private string url { get; set; }
        private string path { get; set; }
        private int totalBytes { get; set; }
        private int currentByte { get; set; }
        private bool status { get; set; }

        public FileObject(int id, string url, string path, int totalBytes, int currentByte = 0, bool status = false)
        {
            this.id = id;
            this.url = url;
            this.path = path;
            this.totalBytes = totalBytes;
            this.currentByte = currentByte;
            this.status = status;
        }

        public static string GetFileName(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string fileHeader = response.Headers["Content-Disposition"];
            string fileName = Regex.Match(fileHeader, "(?<=\\\").*(?=\\\")").Value;

            return fileName;
        }

        public void Download()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Download a new file
        /// </summary>
        /// <param name="url"></param>
        public static void Download(string url)
        {
            string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string fileHeader = response.Headers["Content-Disposition"];
            string fileName = Regex.Match(fileHeader, "(?<=\\\").*(?=\\\")").Value;
            string filePath = directoryPath + fileName;

            // todo: write to file
            // todo: update progress
            // todo: replace url with filename in the listbox
        }
    }
}
