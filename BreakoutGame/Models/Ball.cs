namespace BreakoutGame.Models
{
    public class Ball : Base
    {
        public int Dx { get; set; }
        public int Dy { get; set; }
        public int LastXpos { get; set; }
        public int LastYpos { get; set; }
    }
}
