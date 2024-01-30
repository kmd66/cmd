using System.Text.Json;

namespace CMS.Model
{
    public class ScoreComment
    {
        public double ScoreCount { get; set; }
        public double ScoreSum{ get; set; }
        public double ScorePercent
        {
            get
            {
                return Math.Round((ScoreSum / (5 * ScoreCount)) * 100, 2);
            }
        }
        public double ScoreAvg
        {
            get
            {
                return Math.Round(ScoreSum / ScoreCount, 2);
            }
        }

        public List<Comment> Comments { get; set; }
    }
}