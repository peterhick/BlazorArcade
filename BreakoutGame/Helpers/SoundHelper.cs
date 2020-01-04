using BreakoutGame.Enums;

namespace BreakoutGame.Helpers
{
    public class SoundHelper
    {
        private const string SoundPath = "sounds/breakout-game/";

        private const string HitEdge = "beep-08b.mp3";
        private const string HitPlayer = "beep-07.mp3";
        private const string HitBrick = "beep-24.mp3";
        private const string LostBall = "beep-03.mp3";
        private const string Debug1 = "beep-09.mp3";

        public static void InitialiseSound()
        {
            JsInterop.InteropSound.ClearSounds();
            JsInterop.InteropSound.InitialiseSound(SoundsEnum.HitEdge, SoundPath + HitEdge);
            JsInterop.InteropSound.InitialiseSound(SoundsEnum.HitPlayer, SoundPath + HitPlayer);
            JsInterop.InteropSound.InitialiseSound(SoundsEnum.HitBrick, SoundPath + HitBrick);
            JsInterop.InteropSound.InitialiseSound(SoundsEnum.LostBall, SoundPath + LostBall);
            JsInterop.InteropSound.InitialiseSound(SoundsEnum.Debug1, SoundPath + Debug1);
        }

        public static void PlaySound(SoundsEnum sound)
        {
            JsInterop.InteropSound.PlaySound(sound);
        }
    }
}
