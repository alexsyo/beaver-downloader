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
    class FileObject
    {
        /// <summary>
        /// Download a new file
        /// </summary>
        /// <param name="url"></param>
        public static void Download(string url)
        {
            string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\";

            HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();

            string fileHeader = response.Headers["Content-Disposition"];
            string fileName = Regex.Match(fileHeader, "(?<=\\\").*(?=\\\")").Value;
            string filePath = directoryPath + fileName;

            // todo: write to file
            // todo: update progress
            // todo: replace url with filename in the listbox
        }
    }
}
