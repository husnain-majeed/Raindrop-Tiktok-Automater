using PuppeteerSharp;
using PuppeteerSharp.Input;

namespace Raindrop_Tiktok_Automater
{
     static class TiktokFetcher
     { 
       const string _downloaderUrl = "https://snaptik.app/";
      
        public static async Task<List<string>> FetchDownloadLinks(string videoUrl)
        {
            var allUrls = new List<string>();

            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false,
                DefaultViewport = null
            });

            //var delay = 100;
            var page = await browser.NewPageAsync();
            page.Request += Page_Request;
            page.Response += Page_Response;

           // while (page == null) {};

            await page.GoToAsync(_downloaderUrl);

            var downloadUrlSelector = "#url";
            await page.WaitForSelectorAsync(downloadUrlSelector);

            var mainPageDownloadButtonSelector = ".button-go";
            await TypeFieldValue(page, downloadUrlSelector, videoUrl);
            await clickButton(page, mainPageDownloadButtonSelector);

            Thread.Sleep(3000);
            var downloadButtonSelectors = downloadButtonSelectorDetector(page);

            foreach (var selector in downloadButtonSelectors) 
            {
                var jsSelectAllAnchors = $"Array.from(document.querySelectorAll('{selector}')).map(a => a.href);";
                var urls = await page.EvaluateExpressionAsync<string[]>(jsSelectAllAnchors);

                var urlsList = new List<string>(urls);
                allUrls = allUrls.Concat(urlsList).ToList();

            }

            if (allUrls == null)
                throw new InvalidOperationException("Error string is null");

            return allUrls;

        }

        private static List<string> downloadButtonSelectorDetector(IPage page)
        {
            page.WaitForSelectorAsync("#thumbnail");

            var results = new List<string>();

            var downloadButtonSelectorVideo = ".download-file"; ;
            var downloadButtonSelectorSlideShow = ".w100";

            var findingMainButton = page.QuerySelectorAsync(downloadButtonSelectorVideo);
            findingMainButton.Wait();

            var findingImagesButton = page.QuerySelectorAsync(downloadButtonSelectorSlideShow);
            findingImagesButton.Wait();

            results.Add(downloadButtonSelectorVideo);

            if (findingImagesButton.Result != null)
                results.Add(downloadButtonSelectorSlideShow);
                
            return results;  
        }


        private static void Page_Response(object sender, ResponseCreatedEventArgs e)
        {
           Console.WriteLine(e.Response.Status);
        }

        private static void Page_Request(object sender, RequestEventArgs e)
        {
            Console.WriteLine(e.Request.ResourceType.ToString());
            Console.WriteLine(e.Request.Url);
        }

        private static async Task TypeFieldValue(IPage page, string fieldSelector, string value, int delay = 0)
        {
            await page.FocusAsync(fieldSelector);
            await page.TypeAsync(fieldSelector, value, new TypeOptions { Delay = delay });
            await page.Keyboard.PressAsync("Tab");
        }

        private static async Task clickButton(IPage page, string buttonSelector)
        {
            await page.ClickAsync(buttonSelector);
        }

     }

}