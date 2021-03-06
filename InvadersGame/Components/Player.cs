﻿using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvadersGame.Components
{
    public class PlayerModel : BlazorComponent
    {
        [Parameter]
        internal int PlayerX { get; set; } = 100;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
        }

        /*
        protected override void OnInit()
        {
            base.OnInit();
            RunGame().ConfigureAwait(false);

            Console.WriteLine(nameof(Player) + "_" + nameof(OnInit));
        }

        private async Task RunGame()
        {
            while (true)
            {
                await Task.Delay(1000 / 60);
                PlayerX++;

                //StateHasChanged();
                Console.WriteLine(nameof(Player) + "_" + nameof(RunGame));
            }
        }
        */
    }
}
