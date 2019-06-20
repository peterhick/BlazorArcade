using InvadersGame.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace InvadersGame.Models
{
    public class Bunker : Base
    {
        public List<List<Brick>> Bricks { get; set; }
        public int BricksWide { get; set; }
        public int BricksHigh { get; set; }
        public new int Width
        {
            get
            {
                return BricksWide * Constants.BrickSize;
            }
        }
        public new int Height
        {
            get
            {
                return BricksHigh * Constants.BrickSize;
            }
        }

        public new bool Visible
        {
            get
            {
                return Bricks.SelectMany(b => b).Any(b => b.Visible);
            }
        }

        public Bunker(int bricksWide, int bricksHigh, int CentreXpos, int BottomYpos)
        {
            //Visible = true;
            BricksWide = bricksWide;
            BricksHigh = bricksHigh;
            Xpos = CentreXpos;
            Ypos = BottomYpos;

            // Construct bunker shape from bricks.
            Bricks = new List<List<Brick>>();

            for (int heightIndex = 0; heightIndex < bricksHigh; heightIndex++)
            {
                var bricksRow = new List<Brick>();

                for (int widthIndex = 0; widthIndex < bricksWide; widthIndex++)
                {
                    if (heightIndex == 0)
                    {
                        if (widthIndex == 0)
                        {
                            bricksRow.Add(new Brick { BrickType = Enums.BricksEnum.TopLeft, Visible = true });
                            continue;
                        }

                        if (widthIndex == bricksWide - 1)
                        {
                            bricksRow.Add(new Brick { BrickType = Enums.BricksEnum.TopRight, Visible = true });
                            continue;
                        }
                    }

                    if (heightIndex >= bricksHigh - 1 && heightIndex <= bricksHigh - 1)
                    {
                        if (widthIndex == bricksWide / 3)
                        {
                            bricksRow.Add(new Brick { BrickType = Enums.BricksEnum.BottomLeft, Visible = true });
                            continue;
                        }

                        if (widthIndex == 2 * bricksWide / 3)
                        {
                            bricksRow.Add(new Brick { BrickType = Enums.BricksEnum.BottomRight, Visible = true });
                            continue;
                        }
                    }

                    if (heightIndex >= bricksHigh - 1 && heightIndex <= bricksHigh - 1)
                    {
                        if (widthIndex >= bricksWide / 3 && widthIndex <= 2 * bricksWide / 3)
                        {
                            bricksRow.Add(new Brick { BrickType = Enums.BricksEnum.Blank, Visible = false });
                            continue;
                        }
                    }

                    bricksRow.Add(new Brick { BrickType = Enums.BricksEnum.Full, Visible = true });
                }

                Bricks.Add(bricksRow);
            }
        }
    }
}
