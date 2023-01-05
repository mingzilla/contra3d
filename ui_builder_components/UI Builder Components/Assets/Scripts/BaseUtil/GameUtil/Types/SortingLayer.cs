using System.Collections.Generic;
using BaseUtil.Base;

namespace BaseUtil.GameUtil.Types
{
    public class SortingLayer
    {
        public static readonly SortingLayer PLAYER = Create("Player");
        public static readonly SortingLayer GROUND = Create("Ground");

        public string name;

        static Dictionary<string, SortingLayer> typeMap = Fn.ListToDictionaryWithKeyFn((x) => x.name, All());

        static List<SortingLayer> All()
        {
            return new List<SortingLayer>()
            {
                PLAYER,
                GROUND,
            };
        }

        private static SortingLayer Create(string name)
        {
            var layer = new SortingLayer
            {
                name = name
            };
            return layer;
        }

        public static SortingLayer GetByName(string name)
        {
            return typeMap[(name)];
        }
    }
}