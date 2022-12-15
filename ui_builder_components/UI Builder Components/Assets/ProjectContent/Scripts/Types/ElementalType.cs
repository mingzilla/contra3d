using System.Collections.Generic;
using System.Linq;

namespace ProjectContent.Scripts.Types
{
    public class ElementalType
    {
        public static readonly ElementalType NEUTRAL = Create("NEUTRAL", "skill_equip_panel__skill-type__physical");
        public static readonly ElementalType FIRE = Create("FIRE", "skill_equip_panel__skill-type__fire");
        public static readonly ElementalType WATER = Create("WATER", "skill_equip_panel__skill-type__water");
        public static readonly ElementalType LIGHT = Create("LIGHT", "skill_equip_panel__skill-type__light");

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