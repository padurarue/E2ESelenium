using E2ESelenium.Common;
using OpenQA.Selenium;

namespace E2ESelenium.Pages
{
    public abstract class BaseComponent
    {
        protected static string BaseUrl => new AppConfigReader().BaseUrl;

        public IWebDriver Driver { get; }
        public WebTestContext Context { get; }

        protected BaseComponent(WebTestContext context)
        {
            Context = context;
            Driver = context.Driver;
        }
    }
}
