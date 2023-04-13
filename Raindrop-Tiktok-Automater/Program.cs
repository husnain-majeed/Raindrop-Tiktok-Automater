namespace Raindrop_Tiktok_Automater
{
    class Program
    {
        static void Main(string[] args)
        {
            TiktokCaller();
            while (true)
            {
               var input = Console.ReadLine();
               if(input == "y")
               TiktokCaller();
            }
            
        }

        static async Task<string[]> TiktokCaller()
        {
            var url = "https://www.tiktok.com/@squanchymovies/video/7221476071567396142";
            //var url = "https://www.tiktok.com/@lonewolfez/video/7186763608259300614?is_from_webapp=1&sender_device=pc";
            TiktokFetcher tiktokFetcher = new(url);
            var downloadLinks = await tiktokFetcher.FetchDownloadLinks();
            //return result;

            FileDownloader fileDownloader = new FileDownloader();
            fileDownloader.UnknownTypeByUrlDownload(downloadLinks[0]);
            return null;
        }
    }
}
