namespace BreakoutGame.Helpers
{
    public class HtmlHelper
    {
        public static bool IsReleaseBuild()
        {
#if DEBUG
            return false;
#else
            return true;
#endif
        }
    }
}
