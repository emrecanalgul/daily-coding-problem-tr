using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Models;

namespace Utils
{
    public class HtmlUtils
    {
        public const string Easy = "[easy]";
        public const string Medium = "[medium]";
        public const string Hard = "[hard]";


        public static ReadMailModel ReadHtmlFromText(string htmlText)
        {
            ReadMailModel model = new ReadMailModel();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);

            HtmlNode head = doc.DocumentNode.SelectSingleNode("/html/head");
            HtmlNode title = head.ChildNodes.FindFirst("title");
            var difficultyText = title.InnerText.Split(' ').Last();
            if (!difficultyText.ToLower().Contains(Easy) && !difficultyText.ToLower().Contains(Medium) && !difficultyText.ToLower().Contains(Hard))
                throw new Exception("Non Found Difficulty");

            HtmlNode body = doc.DocumentNode.SelectSingleNode("/html/body");
            HtmlNode table = body.ChildNodes.FindFirst("table");
            HtmlNode bodyContent = table.ChildNodes.FirstOrDefault(x => x.Name.Contains("tr") && x.InnerHtml.Contains("email-body"));
            HtmlNodeCollection contents = bodyContent.SelectNodes("td/table/tr/td/*");

            List<string> p = new List<string>();
            string company = string.Empty;
            foreach (var item in contents)
            {
                if (item.Name.Contains("hr")) break;
                if (item.InnerText.Contains("Good morning")) continue;
                if (item.InnerText.Contains("This problem was asked by"))
                {
                    company = item.InnerText.Replace("This problem was asked by ", string.Empty).Replace(".", string.Empty);
                    continue;
                }
                else if (item.InnerText.Contains("This problem was recently asked by"))
                {
                    company = item.InnerText.Replace("This problem was recently asked by ", string.Empty).Replace(".", string.Empty);
                    continue;
                }

                p.Add(item.InnerText);
            }

            model.Difficulty = GetDifficulty(difficultyText);
            model.Paragraph = p;
            model.Company = company;
            model.Html = htmlText;
            return model;
        }

        static Difficulty GetDifficulty(string text)
        {
            if (text.ToLower().Contains(Easy)) return Difficulty.Easy;
            if (text.ToLower().Contains(Medium)) return Difficulty.Medium;
            if (text.ToLower().Contains(Hard)) return Difficulty.Hard;

            throw new Exception("Non Found Difficulty");

        }
    }
}