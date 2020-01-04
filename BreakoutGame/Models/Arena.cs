using BreakoutGame.Helpers;

namespace BreakoutGame.Models
{
    public class Arena
    {
        public int LeftWidth { get; set; }
        public int RightWidth { get; set; }
        public int TopWidth { get; set; }
        public int LeftLength { get; set; } = Constants.GameAreaHeight;
        public int RightLength { get; set; } = Constants.GameAreaHeight;
        public int TopLength { get; set; } = Constants.GameAreaWidth;
    }
}
