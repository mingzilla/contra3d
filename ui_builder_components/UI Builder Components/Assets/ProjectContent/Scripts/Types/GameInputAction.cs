using BaseUtil.GameUtil;

namespace ProjectContent.Scripts.Types
{
    public class GameInputAction
    {
        public static readonly GameInputAction MOVE = Create("MOVE", false);
        public static readonly GameInputAction JUMP = Create("JUMP", false);
        public static readonly GameInputAction DASH = Create("DASH", false);
        public static readonly GameInputAction ATTACK = Create("ATTACK", false);

        public static readonly GameInputAction MAGIC_TOP = Create("MAGIC_TOP", true);
        public static readonly GameInputAction MAGIC_BOTTOM = Create("MAGIC_BOTTOM", true);
        public static readonly GameInputAction MAGIC_LEFT = Create("MAGIC_LEFT", true);
        public static readonly GameInputAction MAGIC_RIGHT = Create("MAGIC_RIGHT", true);

        public string name;
        public bool isMagic;

        private static GameInputAction Create(string name, bool isMagic)
        {
            return new GameInputAction
            {
                name = name,
                isMagic = isMagic,
            };
        }

        public static UserInput UpdateMagicCommand(UserInput userInput, GameInputAction action)
        {
            if (action == MAGIC_TOP) userInput.magicTop = true;
            if (action == MAGIC_BOTTOM) userInput.magicBottom = true;
            if (action == MAGIC_LEFT) userInput.magicLeft = true;
            if (action == MAGIC_RIGHT) userInput.magicRight = true;
            return userInput;
        }
    }
}