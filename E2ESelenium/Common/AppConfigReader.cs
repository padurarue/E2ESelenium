using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2ESelenium.Common
{
    public class AppConfigReader : IConfig
    {
        public string Browser => GetBrowserVersion();
        public string BaseUrl => GetBaseUrl();

        public string GetBaseUrl()
        {
            return ConfigurationManager.AppSettings["base.url"];
        }

        public string GetBrowserVersion()
        {
            return ConfigurationManager.AppSettings["seleniumDriverVersion.chrome"];
        }

        public string GetTestResultFolder()
        {
            return ConfigurationManager.AppSettings["TestResultPath"];
        }
    }
}
