using System.Runtime.CompilerServices;
using System.Threading;
using OpenQA.Selenium;
using Scenarioo.Annotations;

namespace E2ESelenium.Common.Browser
{
    public static class Runner
    {
        [UsedImplicitly]
        private static readonly ThreadLocal<IWebDriver> _browser = new ThreadLocal<IWebDriver>();

        /// <summary>
        /// Global access to the browser, currently only one instance for all tests.
        /// As soon as we start to run our tests in parallel, we have to change this access to use a ThreadLocal !
        /// </summary>
        public static IWebDriver Browser
        {
            get
            {
                return _browser.Value;
            }

            private set
            {
                _browser.Value = value;
            }
        }

        /// <summary>
        /// Initializes the browser window
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void InitializeHost(WebTestContext context)
        {
            Browser = BrowserManager.Acquire(context);
        }

        /// <summary>
        /// Exits the browser window
        /// </summary>
        public static void StopHost()
        {
            try
            {
                BrowserManager.Release(Browser);
            }
            finally
            {
                Browser = null;
            }
        }
    }
}
