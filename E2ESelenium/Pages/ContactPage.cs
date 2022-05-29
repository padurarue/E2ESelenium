using E2ESelenium.Common;
using E2ESelenium.Common.Browser;
using E2ESelenium.Data;
using NUnit.Framework;
using OpenQA.Selenium;

namespace E2ESelenium.Pages
{
    public class ContactPage : BasePage
    {
        private const string MessageMandatoryField = "Please fill the required field.";
        private const string GeneralMessageValidation = "Validation errors occurred. Please confirm the fields and submit it again.";

        By SendButtonSelector = By.CssSelector("input[value='Send']");
        By NameSenderSelector = By.CssSelector("input[name='your-name']");
        By ValidationNameSenderSelector = By.CssSelector("span.your-name span.wpcf7-not-valid-tip");
        By ValidationEmailSenderSelector = By.CssSelector("span.your-email span.wpcf7-not-valid-tip");
        By ValidationMessageSelector = By.CssSelector("div.screen-reader-response p");

        private IWebElement SectionCorrectlyScroll => Driver.FindElement(By.CssSelector("#get-in-touch div.skrollable-between"));
        private IWebElement NameSender => Driver.FindElement(NameSenderSelector);
        private IWebElement EmailSender => Driver.FindElement(By.CssSelector("input[name='your-email']"));
        private IWebElement TextBody => Driver.FindElement(By.CssSelector("textarea[name='your-message']"));
        private IWebElement SendButton => Driver.FindElement(SendButtonSelector);

        public ContactPage(WebTestContext context)
            : base(context)
        {
            Assert.IsTrue(IsOnPage());
        }

        public ContactPage FillForm(ContactMessageData data)
        {
            NameSender.SendKeys(data.Name);
            EmailSender.SendKeys(data.Email);
            TextBody.SendKeys(data.TextMessage);

            return this;
        }

        public ContactPage SendMessage()
        {
            Driver.WaitUntilElementIsPresentAndEnabled(SendButtonSelector);
            SendButton.Click();

            return this;
        }

        public void AssertNameFieldIsMandatory()
        {
            Driver.WaitUntilTextIsPresentInElement(ValidationNameSenderSelector, MessageMandatoryField);
            Driver.WaitUntilTextIsPresentInElement(ValidationMessageSelector, GeneralMessageValidation);
        }

        public void AssertEmailFieldIsMandatory()
        {
            Driver.WaitUntilTextIsPresentInElement(ValidationEmailSenderSelector, MessageMandatoryField);
            Driver.WaitUntilTextIsPresentInElement(ValidationMessageSelector, GeneralMessageValidation);
        }

        public override bool IsOnPage()
        {
            return SectionCorrectlyScroll.Displayed;
        }
    }
}
