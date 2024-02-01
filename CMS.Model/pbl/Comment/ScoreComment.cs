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
                if (ScoreCount > 0)
                    return Math.Round((ScoreSum / (5 * ScoreCount)) * 100, 2);
                return 0;
            }
        }
        public double ScoreAvg
        {
            get
            {
                if(ScoreCount >0)
                return Math.Round(ScoreSum / ScoreCount, 2);
                return 0;
            }
        }

        public List<Comment> Comments { get; set; }
    }
}