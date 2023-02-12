namespace BaseUtil.GameUtil.Base
{
    public class PauseHandler
    {
        private static bool IS_PAUSED = false;

        public static void PauseGame()
        {
            IS_PAUSED = true;
        }

        public static void ResumeGame()
        {
            IS_PAUSED = false;
        }

        public static bool IsPaused()
        {
            return IS_PAUSED;
        }
    }
}