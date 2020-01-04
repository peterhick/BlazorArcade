using BreakoutGame.Enums;
using BreakoutGame.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace BreakoutGame.Models
{
    public class Bricks
    {
        public List<List<Brick>> Matrix { get; set; }

        /// <summary>
        /// Initialise matrix of bricks:
        /// 2 layers one with rows of red and orange bricks,
        /// and the other with rows of blue and green bricks.
        /// </summary>
        /// <param name="Xcount"></param>
        /// <param name="Ycount"></param>
        /// <param name="Xgap"></param>
        /// <param name="Ygap"></param>
        /// <param name="CentreXpos"></param>
        /// <param name="BottomYpos"></param>
        /// <param name="PatternType"></param>
        public Bricks(int Xcount, int Ycount, int Xgap, int Ygap, int CentreXpos, int BottomYpos, int PatternType)
        {
            Matrix = new List<List<Brick>>();

            var brickType = BrickTypesEnum.Type1;
            var typeCount = 0;
            var yPos = BottomYpos + ((Ycount - 1) * Ygap);

            for (int yCount = 0; yCount < Ycount; yCount++)
            {
                var row = new List<Brick>();
                var xPos = CentreXpos - ((Xcount - 1) * Xgap / 2);
                var width = Constants.BrickWidth;
                var height = Constants.BrickHeight;
                var score = 0;

                switch (typeCount)
                {
                    case 0:
                    case 1:
                        brickType = BrickTypesEnum.Type4;
                        score = Constants.Brick4Score;
                        break;
                    case 2:
                    case 3:
                        brickType = BrickTypesEnum.Type3;
                        score = Constants.Brick3Score;
                        break;
                    case 4:
                    case 5:
                        brickType = BrickTypesEnum.Type2;
                        score = Constants.Brick2Score;
                        break;
                    default:
                        brickType = BrickTypesEnum.Type1;
                        score = Constants.Brick1Score;
                        break;
                }

                for (int xCount = 0; xCount < Xcount; xCount++)
                {
                    row.Add(new Brick
                    {
                        Status = StatusEnum.Alive,
                        //Status = alive,
                        BrickType = brickType,
                        Xpos = xPos,
                        Ypos = yPos,
                        Width = width,
                        Height = height,
                        Score = score,
                        Visible = true
                    });

                    xPos += Xgap;
                }

                yPos -= Ygap;
                yPos -= (PatternType == 1 && ((yCount+1) % 4 == 0)) ? 3*Ygap : 0;
                typeCount++;
                Matrix.Add(row);
            }
        }

        public int BricksPresent()
        {
            return Matrix.Sum(y => y.Count(x => x.Status == StatusEnum.Alive));
        }
    }
}
