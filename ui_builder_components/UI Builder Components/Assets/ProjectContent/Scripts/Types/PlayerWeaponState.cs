using BaseUtil.GameUtil.Base;
using UnityEngine;

namespace ProjectContent.Scripts.Types
{
    public class PlayerWeaponState
    {
        public static readonly PlayerWeaponState NONE = Create("NONE");
        public static readonly PlayerWeaponState SWORD = Create("SWORD");
        public static readonly PlayerWeaponState STAFF = Create("STAFF");

        public string name;

        private static PlayerWeaponState Create(string name)
        {
            return new PlayerWeaponState
            {
                name = name,
            };
        }

        public static void HandleWeaponVisibility(PlayerWeaponState state, GameObject sword, GameObject staff)
        {
            UnityFn.FastSetActive(sword, state == SWORD);
            UnityFn.FastSetActive(staff, state == STAFF);
        }
    }
}