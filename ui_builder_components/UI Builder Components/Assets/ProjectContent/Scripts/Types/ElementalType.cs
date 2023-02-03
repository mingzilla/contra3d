using System.Collections.Generic;
using System.Linq;

namespace ProjectContent.Scripts.Types
{
    public class ElementalType
    {
        public static readonly ElementalType NEUTRAL = Create("NEUTRAL", "panel-for-skill__skill-type__physical");
        public static readonly ElementalType FIRE = Create("FIRE", "panel-for-skill__skill-type__fire");
        public static readonly ElementalType WATER = Create("WATER", "panel-for-skill__skill-type__water");
        public static readonly ElementalType LIGHT = Create("LIGHT", "panel-for-skill__skill-type__light");

        public string name;
        public string imageCssClass;

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

        public static Dictionary<string, ElementalType> ItemsMap()
        {
            return All().ToDictionary(x => x.name, x => x);
        }

        private static ElementalType Create(string name, string imageCssClass)
        {
            ElementalType layer = new()
            {
                name = name,
                imageCssClass = imageCssClass,
            };
            return layer;
        }
    }
}