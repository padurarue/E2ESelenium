using E2ESelenium.Common;

namespace E2ESelenium.Pages
{
    public abstract class BasePage : BaseComponent
    {
        public abstract bool IsOnPage();

        protected BasePage(WebTestContext context)
            : base(context)
        {
        }
    }
}
