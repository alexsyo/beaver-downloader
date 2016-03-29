using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Beaver_Downloader
{
    public class Downloader
    {
        private HttpClient httpClient;
        private XmlData xmlData;

        public Downloader(HttpClient httpClient, XmlData xmlData)
        {
            this.httpClient = httpClient;
            this.xmlData = xmlData;
        }

        /// <summary>
        /// Get the the Headers of the request
        /// </summary>
        /// <param name="webResponse"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetFileHeaders(string url)
        {
            // Make a Header request
            HttpResponseMessage response = await MakeRequest(url, HttpMethod.Head);

            return response;
        }

        /// <summary>
        /// Start the download of the file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task GetFile(string id)
        {
            // Get variables from the xml file
            XmlNode file = xmlData.GetRow(id);
            string url = file["Url"].InnerText;
            string path = file["Dir"].InnerText + file["Name"].InnerText;
            long currentByte = long.Parse(file["CurrentByte"].InnerText);

            // Make a full request with the proper range
            HttpResponseMessage response = await MakeRequest(url, HttpMethod.Get, currentByte);

            // Get the stream from the response
            Stream responseStream = await response.Content.ReadAsStreamAsync();

            // Instantiate a FileStream with the proper access mode
            FileStream fileStream = SetFileStream(path, currentByte);

            // Download the file and store the buffer
            await StreamToFile(responseStream, fileStream, currentByte, id);
        }

        /// <summary>
        /// Make a custom request to the passed url with a min range defined by the currentByte parameter
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="currentByte"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> MakeRequest(string url, HttpMethod method, long currentByte = 0)
        {
            // Create the request
            HttpRequestMessage request = new HttpRequestMessage(method, url);

            // Add a range header
            RangeHeaderValue range = new RangeHeaderValue(currentByte, null);
            request.Headers.Range = range;

            // Make the request
            HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            // Throw an error if the status code is different that 200
            response.EnsureSuccessStatusCode();

            return response;
        }

        /// <summary>
        /// Instantiate a FileStream with a custom access mode depending on the progress
        /// </summary>
        /// <param name="path"></param>
        /// <param name="currentByte"></param>
        /// <returns></returns>
        private FileStream SetFileStream(string path, long currentByte)
        {
            // Set the file mode to create or append depending on the progress
            FileMode fileMode = (currentByte == 0) ? FileMode.Create : FileMode.Append;

            // Instantiate a new fileStream to store the data retrived from the response
            FileStream fileStream = new FileStream(path, fileMode, FileAccess.Write);

            return fileStream;
        }

        /// <summary>
        /// Stream the response to a file while updating the xml with the currentByte
        /// </summary>
        /// <param name="responseStream"></param>
        /// <param name="fileStream"></param>
        /// <param name="currentByte"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task StreamToFile(Stream responseStream, FileStream fileStream, long currentByte, string id)
        {
            // Set the buffer size
            byte[] buffer = new byte[4096];

            // Set a read container to read the responseStream
            int read;

            // Execute while the responseStream read return something
            while ((read = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                // write to the local file
                await fileStream.WriteAsync(buffer, 0, read);

                // Update the UI and the currentByte in the xml file
                currentByte += buffer.Length;
                xmlData.UpdateRow(id, currentByte);
            }
        }
    }
}
