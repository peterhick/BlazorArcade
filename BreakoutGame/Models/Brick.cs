using BreakoutGame.Enums;

namespace BreakoutGame.Models
{
    public class Brick : BaseActor
    {
        //public bool Visible { get; set; }
        //public BricksEnum BrickType { get; set; }
        //public int Xpos { get; set; }
        //public int Ypos { get; set; }
        //public int Width { get; set; }
        //public int Height { get; set; }

        public BrickTypesEnum BrickType { get; set; }
        public bool Animation { get; set; }
        public bool Render { get; set; }
        public int Score { get; set; }

        public void Hit()
        {
            //Hit(5);
            this.Visible = false;
        }
    }
}
