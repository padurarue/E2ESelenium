namespace E2ESelenium.Common
{
    public interface IConfig
    {
        string GetBrowserVersion();
        string GetBaseUrl();
        string GetTestResultFolder();
    }
}
