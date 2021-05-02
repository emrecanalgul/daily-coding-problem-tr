using System.Collections.Generic;

namespace Models
{
    public class ReadMailModel
    {
        public Difficulty Difficulty { get; set; }
        public List<string> Paragraph { get; set; }
        public string Company { get; set; }
        public string Html { get; set; }
        public string ParagraphText { get; internal set; }
    }

    public class Difficulty
    {
        public string BadgeUrl { get; set; }
        public DifficultyEnum DifficultyEnum { get; set; }
    }

    public enum DifficultyEnum
    {
        Easy,
        Medium,
        Hard
    }
}