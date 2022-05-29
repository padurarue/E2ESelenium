using System;
using E2ESelenium.Common;
using NUnit.Framework;
using OpenQA.Selenium;

namespace E2ESelenium.Pages
{
    public class HomePage : BasePage
    {
        private IWebElement AcceptCookiesButton => Driver.FindElement(By.CssSelector("#cn-accept-cookie"));
        private IWebElement SectionCorrectlyScroll => Driver.FindElement(By.CssSelector("#masthead"));
        private IWebElement ScrollToContactPage => Driver.FindElement(By.CssSelector("#site-navigation a[href='#get-in-touch']"));

        public HomePage(WebTestContext context)
            : base(context)
        {
        }

        public HomePage NavigateTo()
        {
            Driver.Navigate().GoToUrl(BaseUrl);
            Console.WriteLine("Goto Url {0}", BaseUrl);

            AcceptCookiesButton.Click();
            Assert.IsTrue(IsOnPage());

            return this;
        }

        public ContactPage NavigateToContactPage()
        {
            ScrollToContactPage.Click();
            Console.WriteLine("Scroll To Contact Page");

            return new ContactPage(Context);
        }

        public override bool IsOnPage()
        {
            return SectionCorrectlyScroll.Displayed;
        }
    }
}
