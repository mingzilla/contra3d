using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseUtil.Base;

namespace BaseUtil.GameUtil.Types
{
    public class PlayerInputScheme
    {
        public static readonly PlayerInputScheme MENU_SELECTION = Create("MENU_SELECTION"); // A to proceed, B to go back
        public static readonly PlayerInputScheme PLAYER_PLAY = Create("GAME_PLAY"); // move, jump, dash and so on

        public string name;

        static Dictionary<string, PlayerInputScheme> typeMap = Fn.ListToDictionaryWithKeyFn((x) => x.name, All());

        static List<PlayerInputScheme> All()
        {
            return new List<PlayerInputScheme>()
            {
                MENU_SELECTION,
                PLAYER_PLAY,
            };
        }

        private static PlayerInputScheme Create(string name)
        {
            var layer = new PlayerInputScheme
            {
                name = name
            };
            return layer;
        }

        public static PlayerInputScheme GetByName(string name)
        {
            return typeMap[(name)];
        }
    }
}