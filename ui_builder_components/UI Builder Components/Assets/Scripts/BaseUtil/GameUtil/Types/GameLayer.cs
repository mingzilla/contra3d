using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseUtil.Base;

namespace BaseUtil.GameUtil.Types
{
    public class GameLayer
    {
        public static readonly GameLayer PLAYER = Create("Player");
        public static readonly GameLayer GROUND = Create("Ground");
        public static readonly GameLayer PLAYER_SHOT = Create("Player Shot");
        public static readonly GameLayer NO_COLLISION = Create("No Collision");
        public static readonly GameLayer DESTRUCTIBLE = Create("Destructible");
        public static readonly GameLayer ENEMY = Create("Enemy"); // normal bullet can't shoot through
        public static readonly GameLayer ENEMY_DETECTION_RANGE = Create("Enemy Detection Range"); // bullet can shoot through
        public static readonly GameLayer POWER_UP = Create("Power Up");

        public string name;

        static Dictionary<string, GameLayer> typeMap = Fn.ListToDictionaryWithKeyFn((x) => x.name, All());

        static List<GameLayer> All()
        {
            return new List<GameLayer>()
            {
                PLAYER,
                GROUND,
                PLAYER_SHOT,
                NO_COLLISION,
                DESTRUCTIBLE,
                ENEMY,
                ENEMY_DETECTION_RANGE,
                POWER_UP,
            };
        }

        private static GameLayer Create(string name)
        {
            var layer = new GameLayer
            {
                name = name
            };
            return layer;
        }

        public static GameLayer GetByName(string name)
        {
            return typeMap[(name)];
        }

        public static LayerMask GetLayerMask(List<GameLayer> gameLayers)
        {
            var names = Fn.Map((it) => it.name, gameLayers);
            return LayerMask.GetMask(names.ToArray());
        }

        public static bool Matches(int layer, GameLayer gameLayer)
        {
            return layer == LayerMask.NameToLayer(gameLayer.name);
        }
    }
}