using System;
using System.Collections.Generic;
using MailKit.Search;
using Models;
using Utils;

namespace gmailReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string userName = Environment.GetEnvironmentVariable("gmailUserName");
            string password = Environment.GetEnvironmentVariable("gmailPassword");

            var mailUtil = new MailUtils("imap.gmail.com", 993, true, userName, password);
            var emails = mailUtil.GetAllMailsByFolderName("DailyCodingProblem", SearchQuery.All);

            List<ReadMailModel> readMailModels = new List<ReadMailModel>();
            foreach (var email in emails)
            {
                try
                {
                    readMailModels.Add(HtmlUtils.ReadHtmlFromText(email));
                }
                catch (System.Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }

        }
    }
}
