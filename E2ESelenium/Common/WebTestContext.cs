using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace E2ESelenium.Common
{
    [TestFixture]
    public class WebTestContext : IDisposable
    {
        private readonly string _driverVersion;
        private readonly string _testResultsDirectory;

        public IWebDriver Driver { get; set; }

        public WebTestContext()
        {
            var config = new AppConfigReader();
            _driverVersion = config.Browser;
            _testResultsDirectory = config.GetTestResultFolder();
        }

        public IWebDriver InitializeWebDriver()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("test-type");

            chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
            chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.cookies", 2);
            chromeOptions.AddArguments("--disable-notifications");

            chromeOptions.AddArguments("disable-logging");
            chromeOptions.AddArguments("--start-maximized");
            chromeOptions.AddArguments("no-sandbox");
            chromeOptions.AddArguments("disable-application-cache");
            chromeOptions.PageLoadStrategy = PageLoadStrategy.Normal;

            new DriverManager().SetUpDriver(new ChromeConfig(), version: _driverVersion);
            ChromeDriver driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), chromeOptions, TimeSpan.FromMinutes(1));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(1);

            return driver;
        }

        public void TearDown()
        {
            try
            {
                if (!IsTestPassed)
                {
                    TakeScreenshot();
                    LogJavaScriptErrors();
                }
            }
            catch
            {
                Console.WriteLine(@"Error reading on TearDown");
            }
        }

        public void TakeScreenshot()
        {
            var timeStamp = DateTime.Now.ToString("HH_mm_ss");
            var fileName = Path.Combine(TestResultsDirectory, TestContext.CurrentContext.Test.FullName.Split('.').Last() + "_" + timeStamp + ".png");

            Console.WriteLine("Taken screenshot: file://" + fileName.Replace("\\", "/"));
            Console.WriteLine("Current url: " + Driver.Url);
            Console.WriteLine("Current window title: " + Driver.Title);

            if (!Directory.Exists(TestResultsDirectory))
            {
                Directory.CreateDirectory(TestResultsDirectory);
            }

            var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
            screenshot.SaveAsFile(fileName, ScreenshotImageFormat.Png);
            TestContext.AddTestAttachment(fileName, TestContext.CurrentContext.Test.FullName);
        }

        private string TestResultsDirectory
        {
            get
            {

                if (string.IsNullOrEmpty(_testResultsDirectory))
                {
                    return AppDomain.CurrentDomain.BaseDirectory;
                }

                return _testResultsDirectory;
            }
        }

        private void LogJavaScriptErrors()
        {
            var javascriptDriver = Driver as IJavaScriptExecutor;
            var errors = javascriptDriver.ExecuteScript("return window.JSErrorCollector_errors ? window.JSErrorCollector_errors.pump() : []");

            if (errors is ReadOnlyCollection<object> collection && collection.Count > 0)
            {
                var error = new StringBuilder();
                error.AppendLine("JavaScript Error found:");

                foreach (var item in collection)
                {
                    var errorObject = item as Dictionary<string, object>;
                    foreach (var field in errorObject)
                    {
                        error.AppendLine(field.Key + " - " + field.Value);
                    }
                }

                if (IsTestPassed)
                {
                    Assert.Fail(error.ToString());
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine(error.ToString());
                    Console.WriteLine(@"-------------------");
                    Console.WriteLine();
                }
            }
        }

        private static bool IsTestPassed
        {
            get
            {
                if (TestContext.CurrentContext == null)
                {
                    Console.WriteLine(@"CurrentContext null");
                }

                return TestContext.CurrentContext.Result.Outcome == ResultState.Success;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
