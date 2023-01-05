namespace ProjectContent.Scripts.Types
{
    public class GameInputContext
    {
        public static readonly GameInputContext TITLE_MENU = Create("TITLE_MENU");
        public static readonly GameInputContext TITLE_CHARACTER_SELECTION = Create("TITLE_CHARACTER_SELECTION");
        public static readonly GameInputContext GAME_PLAY = Create("GAME_PLAY");
        public static readonly GameInputContext IN_GAME_MENU = Create("IN_GAME_MENU");
        public static readonly GameInputContext TRANSITION_SCREEN = Create("TRANSITION_SCREEN");

        public string name;

        private static GameInputContext Create(string name)
        {
            return new GameInputContext
            {
                name = name
            };
        }
    }
}