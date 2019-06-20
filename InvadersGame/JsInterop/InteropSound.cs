using InvadersGame.Enums;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace InvadersGame.JsInterop
{
    public class InteropSound
    {
        public static Task<bool> InitialiseSound(SoundsEnum id, string path, bool loop = false)
        {
            return JSRuntime.Current.InvokeAsync<bool>(
                "JsFunctions.initialiseSound", new { id, path, loop });
        }


        public static Task<bool> PlaySound(SoundsEnum id)
        {
            return JSRuntime.Current.InvokeAsync<bool>(
                "JsFunctions.playSound", id);
        }
    }
}
