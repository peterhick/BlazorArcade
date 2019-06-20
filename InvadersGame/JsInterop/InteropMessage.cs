using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace InvadersGame.JsInterop
{
    public class InteropMessage
    {
        public static Task<string> Prompt(string message)
        {
            // Implemented in exampleJsInterop.js
            return JSRuntime.Current.InvokeAsync<string>(
                "JsFunctions.showPrompt",
                message);
        }

        public static Task<bool> Alert(string message)
        {
            return JSRuntime.Current.InvokeAsync<bool>(
                "JsFunctions.showAlert", message);
        }
    }
}
