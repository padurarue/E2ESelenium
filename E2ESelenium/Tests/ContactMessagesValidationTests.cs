using E2ESelenium.Common;
using E2ESelenium.Data;
using E2ESelenium.Pages;
using NUnit.Framework;

namespace E2ESelenium.Tests
{
    [TestFixture]
    public class ContactMessagesValidationTests : WebTestBase
    {
        [Test]
        public void GivenContactMessage_WhenClickSend_ThenNameCannotBeEmpty()
        {
            var messageData = new ContactMessageData(string.Empty, "someEmail@mail.com");

            new HomePage(WebTestContextInstance)
                 .NavigateTo()
                 .NavigateToContactPage()
                 .FillForm(messageData)
                 .SendMessage()
                 .AssertNameFieldIsMandatory();
        }

        [Test]
        public void GivenContactMessage_WhenClickSend_ThenEmailCannotBeEmpty()
        {
            var messageData = new ContactMessageData("Alex", string.Empty);

            new HomePage(WebTestContextInstance)
                 .NavigateTo()
                 .NavigateToContactPage()
                 .FillForm(messageData)
                 .SendMessage()
                 .AssertEmailFieldIsMandatory();
        }
    }
}
