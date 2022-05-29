using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace E2ESelenium.Common.Browser
{
    public static class CommonDriverExtensions
    {
        public static readonly TimeSpan ImplicitlyWaitTime = TimeSpan.FromSeconds(30);

        public static bool WaitUntilElementIsPresentAndEnabled(this IWebDriver driver, By selector)
        {
            try
            {
                driver.Manage().Timeouts().ImplicitWait = ImplicitlyWaitTime;

                var elements = driver.FindElements(selector);

                if (elements.Count > 0)
                {
                    GetWebDriverWait(driver).Until(d => ElementIsPresent(selector, d));

                    return true;
                }
            }
            catch
            {
            }
            finally
            {
                driver.Manage().Timeouts().ImplicitWait = ImplicitlyWaitTime;
            }
            return false;
        }

        public static void WaitUntilTextIsPresentInElement(this IWebDriver driver, By selector, string text)
        {
            GetWebDriverWait(driver).Until(ExpectedConditions.TextToBePresentInElementLocated(selector, text));
        }

        private static WebDriverWait GetWebDriverWait(IWebDriver driver)
        {
            return new WebDriverWait(driver, ImplicitlyWaitTime);
        }

        public static bool ElementIsPresent(By locator, IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);

            try
            {
                var elements = driver.FindElements(locator);

                return elements.Any(element => element.Displayed && element.Enabled);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                driver.Manage().Timeouts().ImplicitWait = ImplicitlyWaitTime;
            }
        }
    }
}
