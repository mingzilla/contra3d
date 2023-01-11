using System.Collections.Generic;
using BaseUtil.Base;
using ProjectContent.Scripts.Types;

namespace ProjectContent.Scripts.Data
{
    public class GameInputMapping
    {
        private Dictionary<GameInputContext, Dictionary<GameInputKey, GameInputAction>> mapping = new Dictionary<GameInputContext, Dictionary<GameInputKey, GameInputAction>>();

        public static GameInputMapping Create()
        {
            return new GameInputMapping
            {
                mapping = new Dictionary<GameInputContext, Dictionary<GameInputKey, GameInputAction>>()
                {
                    {
                        GameInputContext.GAME_PLAY, new Dictionary<GameInputKey, GameInputAction>()
                        {
                            {GameInputKey.A, GameInputAction.JUMP},
                            {GameInputKey.B, GameInputAction.DASH},
                            {GameInputKey.X, GameInputAction.ATTACK},
                        }
                    },
                }
            };
        }

        public GameInputAction GetGamePlayPadAction(GameInputKey key, bool isHoldingMagicTrigger)
        {
            if (!isHoldingMagicTrigger) return FnVal.SafeGet(null, () => mapping[GameInputContext.GAME_PLAY][key]);
            if (key == GameInputKey.A) return GameInputAction.MAGIC_BOTTOM;
            if (key == GameInputKey.B) return GameInputAction.MAGIC_RIGHT;
            if (key == GameInputKey.X) return GameInputAction.MAGIC_LEFT;
            if (key == GameInputKey.Y) return GameInputAction.MAGIC_TOP;
            return null;
        }

        public GameInputAction GetGamePlayKeyboardAction()
        {
            return GameInputAction.JUMP;
        }
    }
}