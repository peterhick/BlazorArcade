using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvadersGame.Components
{
    public class InvaderModel : BlazorComponent
    {
        [Parameter]
        internal string Image { get; set; }
        [Parameter]
        internal int Xpos { get; set; }
        [Parameter]
        internal int Ypos { get; set; }

    }
}
