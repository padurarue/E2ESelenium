using System;
using System.Globalization;
using System.Threading;
using E2ESelenium.Common.Browser;
using NUnit.Framework;

namespace E2ESelenium.Common
{
    [TestFixture]
    public abstract class WebTestBase : IDisposable
    {
        public TestContext TestContext { get; set; }

        public WebTestContext WebTestContextInstance { get; set; }

        [OneTimeSetUp]
        public void Init()
        {
            WebTestContextInstance = new WebTestContext();
            if (Runner.Browser == null)
            {
                Runner.InitializeHost(WebTestContextInstance);
            }
        }

        [SetUp]
        public virtual void SetUp()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            if (WebTestContextInstance != null)
            {
                WebTestContextInstance = new WebTestContext();
            }

            WebTestContextInstance.Driver = Runner.Browser;
        }

        [TearDown]
        public virtual void TearDown()
        {
            WebTestContextInstance.TearDown();
        }

        [OneTimeTearDown]
        public void ClassCleanup()
        {
            Runner.StopHost();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                WebTestContextInstance?.Dispose();
            }
        }
    }
}
