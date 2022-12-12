using System.Collections.Generic;

namespace ProjectContent.Scripts.Types
{
    public class ElementalType
    {
        public static readonly ElementalType NEUTRAL = Create("NEUTRAL");
        public static readonly ElementalType FIRE = Create("FIRE");
        public static readonly ElementalType WATER = Create("WATER");
        public static readonly ElementalType LIGHT = Create("LIGHT");

        public string name;

        public static List<ElementalType> All()
        {
            return new List<ElementalType>()
            {
                NEUTRAL,
                FIRE,
                WATER,
                LIGHT,
            };
        }

        private static ElementalType Create(string name)
        {
            ElementalType layer = new()
            {
                name = name
            };
            return layer;
        }
    }
}