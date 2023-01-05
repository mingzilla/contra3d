namespace ProjectContent.Scripts.Types
{
    public class GameInputAction
    {
        public static readonly GameInputAction MOVE = Create("MOVE");
        public static readonly GameInputAction JUMP = Create("JUMP");
        public static readonly GameInputAction DASH = Create("DASH");
        public static readonly GameInputAction ATTACK = Create("ATTACK");

        public string name;

        private static GameInputAction Create(string name)
        {
            return new GameInputAction
            {
                name = name
            };
        }
    }
}