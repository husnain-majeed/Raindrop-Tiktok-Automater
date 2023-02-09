using PuppeteerSharp;
using PuppeteerSharp.Input;
using System;
using System.Threading.Tasks;

namespace Raindrop_Tiktok_Automater
{
     class Program
     {
        static async Task Main(string[] args)
        {
            const string url = "https://snaptik.app/";
            string tiktokUrl = "https://vm.tiktok.com/ZMYYg2edw/";

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

            await page.GoToAsync(url);

            var downloadUrlSelector = "#url";
            await page.WaitForSelectorAsync(downloadUrlSelector);

            var downloadButtonSelector = ".btn-go";
            await TypeFieldValue(page, downloadUrlSelector, tiktokUrl);
            await clickButtton(page, downloadButtonSelector);

            var downloadButtonSelector2 = ".btn-main";
            await page.WaitForSelectorAsync(downloadButtonSelector2);
            var jsSelectAllAnchors = @"Array.from(document.querySelectorAll('.btn-main')).map(a => a.href);";
            var urls = await page.EvaluateExpressionAsync<string[]>(jsSelectAllAnchors);

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
     }
}