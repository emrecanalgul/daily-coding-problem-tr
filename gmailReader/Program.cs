using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                    ReadMailModel emailModel = HtmlUtils.ReadHtmlFromText(email);
                    if (readMailModels.Any(x => x.ParagraphText == emailModel.ParagraphText))
                        continue;

                    readMailModels.Add(emailModel);
                }
                catch (System.Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }

            FileUtils.CreateCompanyReadmeFile(readMailModels);
            FileUtils.CreateMainReadmeFile(readMailModels);

        }
    }
}
