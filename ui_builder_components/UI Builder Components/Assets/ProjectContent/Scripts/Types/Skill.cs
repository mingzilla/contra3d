using System.Collections.Generic;
using System.Linq;
using System;

namespace ProjectContent.Scripts.Types
{
    public class Skill
    {
        public static readonly Skill NEUTRAL = Create("NEUTRAL", "Neutral", ElementalType.NEUTRAL, 1, 2, 1, 1f, 20f, true, 3f);
        public static readonly Skill NEUTRAL_1 = Create("NEUTRAL_1", "Neutral", ElementalType.NEUTRAL, 1, 3, 1, 1f, 20f, true, 3f);
        public static readonly Skill NEUTRAL_2 = Create("NEUTRAL_2", "Neutral", ElementalType.NEUTRAL, 2, 5, 1, 1f, 20f, true, 3f);
        public static readonly Skill NEUTRAL_3 = Create("NEUTRAL_3", "Neutral", ElementalType.NEUTRAL, 3, 10, 1, 1f, 20f, true, 3f);

        public static readonly Skill FIRE_BALL = Create("FIRE_BALL", "Fire Ball", ElementalType.FIRE, 1, 2, 1, 1f, 5f, true, 3f);
        public static readonly Skill FIRE_BALL_1 = Create("FIRE_BALL_1", "Fire Ball", ElementalType.FIRE, 1, 3, 1, 1f, 20f, true, 3f);
        public static readonly Skill FIRE_BALL_2 = Create("FIRE_BALL_2", "Fire Ball", ElementalType.FIRE, 2, 5, 1, 1f, 20f, true, 3f);
        public static readonly Skill FIRE_BALL_3 = Create("FIRE_BALL_3", "Fire Ball", ElementalType.FIRE, 3, 10, 1, 1f, 20f, true, 3f);

        public static readonly Skill WATER_BALL = Create("WATER_BALL", "Water Ball", ElementalType.WATER, 1, 2, 1, 1f, 20f, true, 3f);
        public static readonly Skill ICE_SPLASH = Create("ICE_SPLASH", "Water Ball", ElementalType.WATER, 1, 3, 1, 1f, 0f, false, 2f);
        public static readonly Skill WATER_BALL_2 = Create("WATER_BALL_2", "Water Ball", ElementalType.WATER, 2, 5, 1, 1f, 20f, true, 3f);
        public static readonly Skill WATER_BALL_3 = Create("WATER_BALL_3", "Water Ball", ElementalType.WATER, 3, 10, 1, 1f, 20f, true, 3f);

        public static readonly Skill LIGHTENING = Create("LIGHTENING", "Lightening", ElementalType.LIGHT, 1, 2, 1, 1f, 2f, false, 0.5f);
        public static readonly Skill LIGHTENING_1 = Create("LIGHTENING_1", "Lightening", ElementalType.LIGHT, 1, 3, 1, 1f, 20f, true, 3f);
        public static readonly Skill LIGHTENING_2 = Create("LIGHTENING_2", "Lightening", ElementalType.LIGHT, 2, 5, 1, 1f, 20f, true, 3f);
        public static readonly Skill LIGHTENING_3 = Create("LIGHTENING_3", "Lightening", ElementalType.LIGHT, 3, 10, 1, 1f, 20f, true, 3f);

        public string name;
        public string text;
        public ElementalType type;
        public int slots;
        public int manaCost;

        public int damage;
        public float blastRange;
        public float moveSpeed;
        public bool destroyWhenHit;
        public float autoDestroyTime;

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
                ICE_SPLASH,
                WATER_BALL_2,
                WATER_BALL_3,
                LIGHTENING,
                LIGHTENING_1,
                LIGHTENING_2,
                LIGHTENING_3,
            };
        }

        public static Dictionary<string, Skill> ItemsMap()
        {
            return All().ToDictionary(x => x.name, x => x);
        }

        public static Dictionary<ElementalType, List<Skill>> AllGroupByType()
        {
            return All().GroupBy(x => x.type).ToDictionary(entry => entry.Key, entry => entry.ToList());
        }

        public static Dictionary<string, List<Skill>> AllGroupByStringType()
        {
            return All().GroupBy(x => x.type).ToDictionary(entry => entry.Key.name, entry => entry.ToList());
        }

        private static Skill Create(string name, string text, ElementalType type, int slots, int manaCost,
                                    int damage,
                                    float blastRange,
                                    float moveSpeed,
                                    bool destroyWhenHit,
                                    float autoDestroyTime)
        {
            Skill layer = new()
            {
                name = name,
                text = text,
                type = type,
                slots = slots,
                manaCost = manaCost,
                damage = damage,
                blastRange = blastRange,
                moveSpeed = moveSpeed,
                destroyWhenHit = destroyWhenHit,
                autoDestroyTime = autoDestroyTime,
            };
            return layer;
        }
    }
}