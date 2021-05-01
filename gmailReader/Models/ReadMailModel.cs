using System.Collections.Generic;

namespace Models
{
    public class ReadMailModel
    {
        public Difficulty Difficulty { get; set; }
        public List<string> Paragraph { get; set; }
        public string Company { get; set; }
        public string Html { get; set; }
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
}