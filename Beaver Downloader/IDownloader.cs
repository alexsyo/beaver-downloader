﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Beaver_Downloader
{
    interface IDownloader
    {
        Task<HttpResponseMessage> GetFileHeaders(string url);

        Task GetFile(string id);
    }
}
