using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beaver_Downloader
{
    public class Files
    {
        public string url { get; private set; }
        public int progress { get; set; }

        public Files(string url, int progress = 0)
        {
            this.url = url;
            this.progress = progress;
        }
    }
}
