using System.Collections.Generic;
using System.Linq;
using System;

namespace ProjectContent.Scripts.Types
{
    public class Skill
    {
        public static readonly Skill NEUTRAL = Create("Neutral", ElementalType.NEUTRAL, 1, 2);
        public static readonly Skill NEUTRAL_1 = Create("Neutral", ElementalType.NEUTRAL, 1, 3);
        public static readonly Skill NEUTRAL_2 = Create("Neutral", ElementalType.NEUTRAL, 2, 5);
        public static readonly Skill NEUTRAL_3 = Create("Neutral", ElementalType.NEUTRAL, 3, 10);

        public static readonly Skill FIRE_BALL = Create("Fire Ball", ElementalType.FIRE, 1, 2);
        public static readonly Skill FIRE_BALL_1 = Create("Fire Ball", ElementalType.FIRE, 1, 3);
        public static readonly Skill FIRE_BALL_2 = Create("Fire Ball", ElementalType.FIRE, 2, 5);
        public static readonly Skill FIRE_BALL_3 = Create("Fire Ball", ElementalType.FIRE, 3, 10);

        public static readonly Skill WATER_BALL = Create("Water Ball", ElementalType.WATER, 1, 2);
        public static readonly Skill WATER_BALL_1 = Create("Water Ball", ElementalType.WATER, 1, 3);
        public static readonly Skill WATER_BALL_2 = Create("Water Ball", ElementalType.WATER, 2, 5);
        public static readonly Skill WATER_BALL_3 = Create("Water Ball", ElementalType.WATER, 3, 10);

        public static readonly Skill LIGHTENING = Create("Lightening", ElementalType.LIGHT, 1, 2);
        public static readonly Skill LIGHTENING_1 = Create("Lightening", ElementalType.LIGHT, 1, 3);
        public static readonly Skill LIGHTENING_2 = Create("Lightening", ElementalType.LIGHT, 2, 5);
        public static readonly Skill LIGHTENING_3 = Create("Lightening", ElementalType.LIGHT, 3, 10);

        public string name;
        public ElementalType type;
        public int slots;
        public int manaCost;

        static List<Skill> All()
        {
            return new List<Skill>()
            {
                NEUTRAL,
                NEUTRAL_1,
                NEUTRAL_2,
                NEUTRAL_3,
                FIRE_BALL,
                FIRE_BALL_1,
                FIRE_BALL_2,
                FIRE_BALL_3,
                WATER_BALL,
                WATER_BALL_1,
                WATER_BALL_2,
                WATER_BALL_3,
                LIGHTENING,
                LIGHTENING_1,
                LIGHTENING_2,
                LIGHTENING_3,
            };
        }

        public static Dictionary<ElementalType, List<Skill>> AllGroupByType()
        {
            return All().GroupBy(x => x.type).ToDictionary(entry => entry.Key, entry => entry.ToList());
        }

        public static Dictionary<string, List<Skill>> AllGroupByStringType()
        {
            return All().GroupBy(x => x.type).ToDictionary(entry => entry.Key.name, entry => entry.ToList());
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