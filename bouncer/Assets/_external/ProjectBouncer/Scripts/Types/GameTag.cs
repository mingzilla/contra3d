using System.Collections.Generic;
using BaseUtil.Base;

namespace Types
{
    public class GameTag
    {
        public static GameTag PLAYER = Create("PLAYER");
        public static GameTag OBSTACLE = Create("OBSTACLE");
        public static GameTag STARTING_POINT = Create("STARTING_POINT");
        public static GameTag DESTINATION = Create("DESTINATION");

        public string name;

        static Dictionary<string, GameTag> typeMap = Fn.ListToDictionaryWithKeyFn((x) => x.name, All());

        static List<GameTag> All()
        {
            return new List<GameTag>()
            {
                PLAYER,
                OBSTACLE,
                STARTING_POINT,
                DESTINATION,
            };
        }

        private static GameTag Create(string name)
        {
            GameTag layer = new GameTag();
            layer.name = name;
            return layer;
        }

        public static GameTag GetByName(string name)
        {
            return typeMap[(name)];
        }
    }
}