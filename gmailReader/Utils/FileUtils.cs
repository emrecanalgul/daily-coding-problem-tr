using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models;

namespace Utils
{
    public class FileUtils
    {
        public const string Root = "../";
        public const string MainFolderName = "../DailyCodingProblem";


        public static void CreateCompanyReadmeFile(List<ReadMailModel> readMailModels)
        {
            foreach (var item in readMailModels)
            {
                string folderPath = $"{MainFolderName}/{item.Company}";
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                string[] files = Directory.GetFiles(folderPath);
                int filesCount = files == null ? 0 : files.Length;

                List<string> fileLines = new List<string>();
                string title = $"# {item.Company} ![Difficulty]({item.Difficulty.BadgeUrl})";
                fileLines.Add(title);
                fileLines.Add("\t");

                foreach (var p in item.Paragraph)
                {
                    fileLines.Add(p);
                    fileLines.Add("\t");
                }

                string filePath = folderPath + $"/{filesCount + 1}.md";
                var createFile = File.Create(filePath);
                createFile.Dispose();

                File.WriteAllLines(filePath, fileLines);

            }
        }

        public static void CreateMainReadmeFile(List<ReadMailModel> readMailModels)
        {
            string filePath = $"{Root}/README.md";
            if (!File.Exists(filePath))
            {
                var file = File.Create(filePath);
                file.Dispose();
            }

            List<string> lines = new List<string>();
            string title = "# Daily Coding Problem";
            lines.Add(title);
            lines.Add("\t");

            string subTitle = "## :office: Company";
            lines.Add(subTitle);
            lines.Add("\t");

            var groupList = readMailModels.OrderBy(x => x.Company).GroupBy(t => t.Company)
                   .Select(g => new { Name = g.Key, Count = g.Count() });
            foreach (var item in groupList)
                lines.Add($"- [{item.Name}](DailyCodingProblem/{item.Name.Replace(" ", "%20")})  `{item.Count}`");

            File.WriteAllLines(filePath, lines);
        }


    }
}