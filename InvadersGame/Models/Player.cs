﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvadersGame.Enums;
using InvadersGame.Helpers;

namespace InvadersGame.Models
{
    public class Player : BaseActor
    {
        public Player(int Xpos, int Ypos)
        {
            Visible = true;
            this.Xpos = Xpos;
            this.Ypos = Ypos;
            Width = Constants.PlayerWidth;
            Height = Constants.PlayerHeight;
        }

        public bool AnimateDying()
        {
            if (Status == StatusEnum.Dying)
            {
                AnimationTimer--;

                if (AnimationTimer <= 0)
                {
                    return true;
                }
            }

            return false;
        }

        public void Animate(GameEnum gameStatus)
        {
            if (gameStatus == GameEnum.Prepare)
            {
                Visible = !Visible;
            }
        }

        public void Hit()
        {
            Hit(20);
        }
    }
}
