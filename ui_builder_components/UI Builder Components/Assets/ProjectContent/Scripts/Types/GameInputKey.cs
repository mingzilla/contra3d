namespace ProjectContent.Scripts.Types
{
    /// <summary>
    /// Defines the keys in xbox controller representation.
    /// The whole game uses this set of inputs, the actual inputs for keyboard, controller and so on are mapped to this set of inputs to control the game 
    /// </summary>
    public class GameInputKey
    {
        public static readonly GameInputKey X = Create("X");
        public static readonly GameInputKey Y = Create("Y");
        public static readonly GameInputKey A = Create("A");
        public static readonly GameInputKey B = Create("B");

        public static readonly GameInputKey LB = Create("LB");
        public static readonly GameInputKey RB = Create("RB");
        public static readonly GameInputKey LT = Create("LT");
        public static readonly GameInputKey RT = Create("RT");

        public string name;

        private static GameInputKey Create(string name)
        {
            GameInputKey layer = new()
            {
                name = name
            };
            return layer;
        }
    }
}