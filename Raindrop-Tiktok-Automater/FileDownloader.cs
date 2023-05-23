using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raindrop_Tiktok_Automater
{
    public static class FileDownloader
    {
        public static async void UnknownTypeByUrlDownload(string url)
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();

                var filename = response.Content.Headers.ContentDisposition.FileName;

                var stream = await response.Content.ReadAsStreamAsync();

                var destinationFile = Path.Combine("D:\\ETC Dump\\tiktok - output", filename);

                using(var fileStream = File.Create(destinationFile))
                {
                    await fileStream.CopyToAsync(stream);
                }
            }
        }
    }
}
