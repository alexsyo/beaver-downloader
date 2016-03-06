using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beaver_Downloader
{
    public class DownloadItem
    {
        public string url { get; private set; }
        public int progress { get; set; }

        public DownloadItem(string url, int progress)
        {
            this.url = url;
            this.progress = progress;
        }
    }
}
