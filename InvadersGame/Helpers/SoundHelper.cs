using InvadersGame.Enums;

namespace InvadersGame.Helpers
{
    public class SoundHelper
    {
        private const string SoundPath = "sounds/invaders-game/";

        private const string Explosion = "explosion.wav";
        private const string Inv1 = "fastinvader1.wav";
        private const string Inv2 = "fastinvader2.wav";
        private const string Inv3 = "fastinvader3.wav";
        private const string Inv4 = "fastinvader4.wav";
        private const string InvKilled = "invaderkilled.wav";
        private const string Shoot = "shoot.wav";
        private const string UfoHigh = "ufo_highpitch.wav";
        private const string UfoLow = "ufo_lowpitch.wav";

        public static void InitialiseSound()
        {
            JsInterop.InteropSound.InitialiseSound(SoundsEnum.Explosion, SoundPath + Explosion);
            JsInterop.InteropSound.InitialiseSound(SoundsEnum.Inv1, SoundPath + Inv1);
            JsInterop.InteropSound.InitialiseSound(SoundsEnum.Inv2, SoundPath + Inv2);
            JsInterop.InteropSound.InitialiseSound(SoundsEnum.Inv3, SoundPath + Inv3);
            JsInterop.InteropSound.InitialiseSound(SoundsEnum.Inv4, SoundPath + Inv4);
            JsInterop.InteropSound.InitialiseSound(SoundsEnum.InvKilled, SoundPath + InvKilled);
            JsInterop.InteropSound.InitialiseSound(SoundsEnum.Shoot, SoundPath + Shoot);
            JsInterop.InteropSound.InitialiseSound(SoundsEnum.UfoHigh, SoundPath + UfoHigh);
            JsInterop.InteropSound.InitialiseSound(SoundsEnum.UfoLow, SoundPath + UfoLow);
        }

        public static void PlaySound(SoundsEnum sound)
        {
            JsInterop.InteropSound.PlaySound(sound);
        }
    }
}
