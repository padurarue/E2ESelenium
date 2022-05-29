using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace E2ESelenium.Common.Browser
{
    public static class BrowserManager
    {
        private static readonly List<BrowserManagerItem> _usedBrowsers = new List<BrowserManagerItem>();

        private static readonly Queue<BrowserManagerItem> _availableBrowsers = new Queue<BrowserManagerItem>();

        private static readonly object _locker = new object();

        static BrowserManager()
        {
            // Needed to close the Browsers when all tests are run.
            AppDomain.CurrentDomain.ProcessExit += Cleanup;
        }

        private static void Cleanup(object sender, EventArgs e)
        {
            foreach (var browserManagerItem in _usedBrowsers)
            {
                Dispose(browserManagerItem.Driver);
            }

            foreach (var browserManagerItem in _availableBrowsers)
            {
                Dispose(browserManagerItem.Driver);
            }
        }

        public static IWebDriver Acquire(WebTestContext context)
        {
            lock (_locker)
            {
                if (_availableBrowsers.Any() == false)
                {
                    _availableBrowsers.Enqueue(new BrowserManagerItem { Driver = context.InitializeWebDriver() });
                }

                var browserManagerItem = _availableBrowsers.Dequeue();

                _usedBrowsers.Add(browserManagerItem);

                return browserManagerItem.Driver;
            }
        }

        public static void Release(IWebDriver browser)
        {
            lock (_locker)
            {
                var browserManageItem = _usedBrowsers.Single(x => x.Driver.Equals(browser));
                _usedBrowsers.Remove(browserManageItem);
                _availableBrowsers.Enqueue(browserManageItem);
            }
        }

        public static void Dispose(IWebDriver driver)
        {
            driver?.Close();
            driver?.Quit();
            driver?.Dispose();
        }
    }
}
