using BreakoutGame.Enums;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BreakoutGame.JsInterop
{
    public class InteropSound
    {
        public static Task<bool> ClearSounds()
        {
            return JSRuntime.Current.InvokeAsync<bool>(
                "JsFunctions.clearSounds");
        }

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
