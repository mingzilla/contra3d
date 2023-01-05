using System.Collections.Generic;
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

        public GameInputAction GetGamePlayPadAction(GameInputKey key)
        {
            return mapping[GameInputContext.GAME_PLAY][key];
        }

        public GameInputAction GetGamePlayKeyboardAction()
        {
            return GameInputAction.JUMP;
        }
    }
}