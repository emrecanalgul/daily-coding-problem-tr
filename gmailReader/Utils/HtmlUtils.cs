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
            string company = "Others";
            foreach (var item in contents)
            {
                if (item.Name.Contains("hr")) break;
                if (item.InnerText.Contains("Good morning")) continue;

                string companyName = GetCompany(item.InnerText);
                if (companyName != null)
                {
                    company = companyName;
                    continue;
                }

                if (item.HasChildNodes)
                {
                    string innerText = string.Empty;
                    foreach (var child in item.ChildNodes)
                    {
                        string childInnerText = ReplaceText(child.InnerText);

                        if (child.ParentNode.Name.Contains("pre")) innerText += $"```\n{childInnerText}```";
                        else if (child.Name.Contains("code")) innerText += $"`{childInnerText}`";
                        else if (child.Name.Contains("text")) innerText += childInnerText;
                    }

                    p.Add(innerText);
                }
                else
                {
                    p.Add(item.InnerText);
                }
            }

            model.Difficulty = GetDifficulty(difficultyText);
            model.Paragraph = p;
            model.ParagraphText = string.Join(string.Empty, p);
            model.Company = company;
            model.Html = htmlText;
            return model;
        }

        private static string ReplaceText(string innerText)
        {
            innerText = innerText.Replace("&#39;", "'");
            innerText = innerText.Replace("&quot;", "\"");
            return innerText;
        }

        static Difficulty GetDifficulty(string text)
        {
            if (text.ToLower().Contains(Easy))
            {

                return new Difficulty
                {
                    DifficultyEnum = DifficultyEnum.Easy,
                    BadgeUrl = "https://img.shields.io/badge/-EASY-green",
                };
            }
            if (text.ToLower().Contains(Medium))
            {
                return new Difficulty
                {
                    DifficultyEnum = DifficultyEnum.Medium,
                    BadgeUrl = "https://img.shields.io/badge/-MEDIUM-yellow",
                };
            }
            if (text.ToLower().Contains(Hard))
            {
                return new Difficulty
                {
                    DifficultyEnum = DifficultyEnum.Hard,
                    BadgeUrl = "https://img.shields.io/badge/-HARD-red",
                };
            }

            throw new Exception("Non Found Difficulty");

        }

        static string GetCompany(string innerText)
        {
            if (innerText.Contains("This problem was asked by"))
            {
                return innerText.Replace("This problem was asked by ", string.Empty).Replace(".", string.Empty);
            }
            else if (innerText.Contains("This problem was recently asked by"))
            {
                return innerText.Replace("This problem was recently asked by ", string.Empty).Replace(".", string.Empty);
            }
            else if (innerText.Contains("This problem was asked "))
            {
                return innerText.Replace("This problem was asked ", string.Empty).Replace(".", string.Empty);
            }
            else if (innerText.Contains("This question was asked by "))
            {
                return innerText.Replace("This question was asked by ", string.Empty).Replace(".", string.Empty);
            }

            return null;
        }

    }
}