using System.Collections.Generic;
using BaseUtil.Base;

namespace BaseUtil.GameUtil.Domain
{
    public class UnitInvincibility
    {
        public static readonly UnitInvincibility WHEN_TAKING_DAMAGE = Create("WHEN_TAKING_DAMAGE", 1f, 0.1f); // e.g. player
        public static readonly UnitInvincibility NEVER = Create("NEVER", 0f, 0f); // e.g. mods
        public static readonly UnitInvincibility WHEN_TRANSFORMING = Create("WHEN_TRANSFORMING", 5f, 0f); // e.g. boss

        public string name;
        public float invincibilityDuration;
        public float flashInterval;

        private static readonly Dictionary<string, UnitInvincibility> typeMap = Fn.ListToDictionaryWithKeyFn((x) => x.name, All());

        private UnitInvincibility() { }

        static List<UnitInvincibility> All()
        {
            return new List<UnitInvincibility>()
            {
                WHEN_TAKING_DAMAGE, NEVER, WHEN_TRANSFORMING
            };
        }

        public static UnitInvincibility Create(string name, float invincibilityDuration, float flashInterval)
        {
            return new UnitInvincibility
            {
                name = name,
                invincibilityDuration = invincibilityDuration,
                flashInterval = flashInterval
            };
        }

        public static UnitInvincibility GetByName(string name)
        {
            return typeMap[(name)];
        }

        public static UnitInvincibility GetByNameWithDefault(string name)
        {
            return typeMap.ContainsKey(name) ? GetByName(name) : NEVER;
        }

        public bool CanFlash()
        {
            return flashInterval > 0;
        }
    }
}