using E2ESelenium.Common;
using E2ESelenium.Data;
using E2ESelenium.Pages;
using NUnit.Framework;

namespace E2ESelenium.Tests
{
    [TestFixture]
    public class ContactMessagesTests : WebTestBase
    {
        [TestCase("Attempt1", "mail1")]
        [TestCase("Attempt2", "mail2")]
        public void SendMessage(string name, string email)
        {
            var messageData = new ContactMessageData(name, email);

            new HomePage(WebTestContextInstance)
                 .NavigateTo()
                 .NavigateToContactPage()
                 .FillForm(messageData)
                 .SendMessage();
        }
    }
}