using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raindrop_Tiktok_Automater
{
    public class FileDownloader
    {
        public FileDownloader()
        {
            
        }

        public async void UnknownTypeByUrlDownload(string url)
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();

                var filename = response.Content.Headers.ContentDisposition.FileName;

                var test = "test";
   

            }
        }
    }
}
