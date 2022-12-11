using System.Collections.Generic;

namespace ProjectContent.Scripts.Types
{
    public class ElementalType
    {
        public static readonly ElementalType NEUTRAL = Create("Neutral");
        public static readonly ElementalType FIRE = Create("Fire");
        public static readonly ElementalType WATER = Create("Water");
        public static readonly ElementalType LIGHT = Create("Light");

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