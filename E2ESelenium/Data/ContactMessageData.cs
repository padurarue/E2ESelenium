using System;

namespace E2ESelenium.Data
{
    public class ContactMessageData
    {
        public string Name;
        public string Email;
        public string TextMessage;

        public ContactMessageData(string name, string email)
        {
            Name = name;
            Email = email;
            TextMessage = Guid.NewGuid().ToString();
        }
    }
}
