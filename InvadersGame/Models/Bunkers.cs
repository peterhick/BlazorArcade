using InvadersGame.Helpers;
using System.Collections.Generic;

namespace InvadersGame.Models
{
    public class Bunkers
    {
        public List<Bunker> BunkerList { get; set; }

        public Bunkers(int bunkerCount)
        {
            int xGap = Constants.GameAreaWidth / (bunkerCount + 1);
            int xPos = xGap;

            BunkerList = new List<Bunker>();

            for (int bunkerIndex = 0; bunkerIndex < bunkerCount; bunkerIndex++)
            {
                BunkerList.Add(new Bunker(Constants.BunkerWidth, Constants.BunkerHeight, xPos, Constants.BunkerBottomYpos));

                xPos += xGap;
            }
        }
    }
}
