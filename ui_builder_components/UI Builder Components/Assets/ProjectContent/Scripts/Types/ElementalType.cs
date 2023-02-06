using System.Collections.Generic;
using System.Linq;

namespace ProjectContent.Scripts.Types
{
    public class ElementalType
    {
        public static readonly ElementalType NOT_SET = Create("NOT_SET", "elemental-type__image__not-set", "not-set");
        public static readonly ElementalType FIRE = Create("FIRE", "elemental-type__image____fire", "panel-bg-2");
        public static readonly ElementalType WATER = Create("WATER", "elemental-type__image____water", "panel-bg-3");
        public static readonly ElementalType LIGHT = Create("LIGHT", "elemental-type__image____light", "panel-bg-1");

        public string name;
        public string imageCssClass;
        public string bgColorCssClass;

        public static List<ElementalType> All()
        {
            return new List<ElementalType>()
            {
                NOT_SET,
                FIRE,
                WATER,
                LIGHT,
            };
        }

        public static Dictionary<string, ElementalType> ItemsMap()
        {
            return All().ToDictionary(x => x.name, x => x);
        }

        private static ElementalType Create(string name, string imageCssClass, string bgColorCssClass)
        {
            ElementalType layer = new()
            {
                name = name,
                imageCssClass = imageCssClass,
                bgColorCssClass = bgColorCssClass,
            };
            return layer;
        }
    }
}