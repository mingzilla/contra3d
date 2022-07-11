using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;

namespace ProjectContra.Scripts.Types
{
    public class GameLayer
    {
        public static readonly GameLayer PLAYER = Create("PLAYER");
        public static readonly GameLayer PLAYER_SHOT = Create("PLAYER_SHOT");
        public static readonly GameLayer GROUND = Create("GROUND");
        public static readonly GameLayer DESTRUCTIBLE = Create("DESTRUCTIBLE");
        public static readonly GameLayer ENEMY = Create("ENEMY");
        public static readonly GameLayer ENEMY_INSIDE_WALL = Create("ENEMY_INSIDE_WALL"); // enemy that doesn't collide with walls, so that animation is not prevented by wall
        public static readonly GameLayer ENEMY_SHOT = Create("ENEMY_SHOT");
        public static readonly GameLayer ENEMY_GRENADE = Create("ENEMY_GRENADE");
        public static readonly GameLayer ENEMY_DESTROYABLE_SHOT = Create("ENEMY_DESTROYABLE_SHOT");
        public static readonly GameLayer ENEMY_JUMP_POINT = Create("ENEMY_JUMP_POINT");
        public static readonly GameLayer POWER_UP = Create("POWER_UP");
        public static readonly GameLayer POWER_UP_CONTAINER = Create("POWER_UP_CONTAINER");
        public static readonly GameLayer NO_COLLISION = Create("NO_COLLISION");
        public static readonly GameLayer GROUND_COLLIDER = Create("GROUND_COLLIDER"); // Used e.g. as child object to collide with ground, so that parent can be a trigger to player 
        public static readonly GameLayer INVISIBLE_WALL_TO_PLAYER = Create("INVISIBLE_WALL_TO_PLAYER"); // player is blocked, but bullet can pass through
        public static readonly GameLayer WALL = Create("WALL"); // Not allow mods or bullets to go through
        public static readonly GameLayer THIN_GROUND = Create("THIN_GROUND"); // Not allow mods, but bullets can go through
        public static readonly GameLayer REDIRECTION_WALL = Create("REDIRECTION_WALL"); // trigger for enemies to change direction

        public string name;

        private static readonly Dictionary<string, GameLayer> typeMap = Fn.ListToDictionaryWithKeyFn((x) => x.name, All());

        static List<GameLayer> All()
        {
            return new List<GameLayer>()
            {
                PLAYER,
                PLAYER_SHOT,
                GROUND,
                DESTRUCTIBLE,
                ENEMY,
                ENEMY_INSIDE_WALL,
                ENEMY_SHOT,
                ENEMY_GRENADE,
                ENEMY_DESTROYABLE_SHOT,
                ENEMY_JUMP_POINT,
                POWER_UP,
                POWER_UP_CONTAINER,
                NO_COLLISION,
                GROUND_COLLIDER,
                INVISIBLE_WALL_TO_PLAYER,
                WALL,
                THIN_GROUND,
                REDIRECTION_WALL,
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

        public static void InitOnAwake()
        {
            // TagUtil.LogErrorIfLayersIfNotPresent(Fn.Map(it => it.name, All()));
        }

        public int GetLayer()
        {
            return LayerMask.NameToLayer(name);
        }

        public static LayerMask GetGroundLayerMask()
        {
            return GetLayerMask(new List<GameLayer>()
            {
                GROUND,
                DESTRUCTIBLE,
                INVISIBLE_WALL_TO_PLAYER,
                THIN_GROUND,
            });
        }
    }
}