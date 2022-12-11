using System.Collections.Generic;
using System.Linq;
using System;

namespace ProjectContent.Scripts.Types
{
    public class Skill
    {
        public static readonly Skill NEUTRAL = Create("Neutral", ElementalType.NEUTRAL, 1, 2);
        public static readonly Skill FIRE_BALL = Create("Fire Ball", ElementalType.FIRE, 1, 2);
        public static readonly Skill WATER_BALL = Create("Water Ball", ElementalType.WATER, 1, 2);
        public static readonly Skill LIGHTENING = Create("Lightening", ElementalType.LIGHT, 1, 2);

        public string name;
        public ElementalType type;
        public int slots;
        public int manaCost;

        static List<Skill> All()
        {
            return new List<Skill>()
            {
                NEUTRAL,
                FIRE_BALL,
                WATER_BALL,
                LIGHTENING,
            };
        }

        public static Dictionary<ElementalType, List<Skill>> AllGroupByType()
        {
            return All().GroupBy(x => x.type).ToDictionary(entry => entry.Key, entry => entry.ToList());
        }

        private static Skill Create(string name, ElementalType type, int slots, int manaCost)
        {
            Skill layer = new()
            {
                name = name,
                type = type,
                slots = slots,
                manaCost = manaCost,
            };
            return layer;
        }
    }
}