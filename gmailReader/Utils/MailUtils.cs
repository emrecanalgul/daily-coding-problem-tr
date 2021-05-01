using System.Collections.Generic;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;


namespace Utils
{
    /// <summary>
    /// https://stackoverflow.com/questions/7056715/reading-emails-from-gmail-in-c-sharp
    /// </summary>
    public class MailUtils
    {
        private readonly string mailServer, login, password;
        private readonly int port;
        private readonly bool ssl;

        public MailUtils(string mailServer, int port, bool ssl, string login, string password)
        {
            this.mailServer = mailServer;
            this.port = port;
            this.ssl = ssl;
            this.login = login;
            this.password = password;
        }

        public IEnumerable<string> GetAllMailsByFolderName(string folderName, SearchQuery searchQuery)
        {
            var messages = new List<string>();

            using (var client = new ImapClient())
            {
                client.Connect(mailServer, port, ssl);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                client.Authenticate(login, password);

                var folder = client.GetFolder(folderName);
                folder.Open(FolderAccess.ReadOnly);
                var results = folder.Search(SearchOptions.All, searchQuery);
                int count = 0;
                foreach (var uniqueId in results.UniqueIds)
                {
                    var message = folder.GetMessage(uniqueId);

                    messages.Add(message.HtmlBody);
                    if (count == 10) break;
                    count++;
                }

                client.Disconnect(true);
            }

            return messages;
        }
    }
}