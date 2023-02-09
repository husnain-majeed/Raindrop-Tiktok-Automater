using PuppeteerSharp;
using PuppeteerSharp.Input;
using System;
using System.Threading.Tasks;

namespace Raindrop_Tiktok_Automater
{
     class TiktokFetcher
     { 
       const string _url = "https://snaptik.app/";
       readonly string _videoUrl;
       string[] _allUrls = null;

       public TiktokFetcher(string videoUrl)
        {
           _videoUrl = videoUrl;
        }

        public async Task Fetch()
        { 
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

            await page.GoToAsync(_url);

            var downloadUrlSelector = "#url";
            await page.WaitForSelectorAsync(downloadUrlSelector);

            var downloadButtonSelector = ".btn-go";
            await TypeFieldValue(page, downloadUrlSelector, _videoUrl);
            await clickButtton(page, downloadButtonSelector);

            var downloadButtonSelector2 = ".btn-main";
            await page.WaitForSelectorAsync(downloadButtonSelector2);
            var jsSelectAllAnchors = @"Array.from(document.querySelectorAll('.btn-main')).map(a => a.href);";
            var urls = await page.EvaluateExpressionAsync<string[]>(jsSelectAllAnchors);
            _allUrls = urls;

        }
         private static async void Page_Response(object sender, ResponseCreatedEventArgs e)
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
        private static async Task clickButtton(IPage page, string buttonSelector)
        {
            await page.ClickAsync(buttonSelector);
        }

        public string[] Return()
        {
            if (_allUrls == null)
                throw new InvalidOperationException("Please run fetch method first");

            return _allUrls;
        }

     }

}