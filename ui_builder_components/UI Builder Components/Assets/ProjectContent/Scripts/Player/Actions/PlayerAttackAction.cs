using BaseUtil.GameUtil;
using ProjectContent.Scripts.Bullet;
using ProjectContent.Scripts.Data;
using ProjectContent.Scripts.Types;
using UnityEngine;

namespace ProjectContent.Scripts.Player.Actions
{
    public class PlayerAttackAction
    {
        private static readonly int TRIGGER_SWING_KEY = Animator.StringToHash("triggerSwing");
        private static readonly int TRIGGER_MAGIC_KEY = Animator.StringToHash("triggerMagic");

        public PlayerWeaponState playerWeaponState = PlayerWeaponState.NONE;

        private Animator animatorCtrl;
        private Rigidbody rb;

        private GameObject swordMesh;
        private GameObject staffMesh;

        private GameObject fireballPrefab;
        private GameObject lighteningPrefab;
        private GameObject iceSplashPrefab;

        public static PlayerAttackAction Create(Animator animatorCtrl, Rigidbody rb,
                                                GameObject swordMesh, GameObject staffMesh,
                                                GameObject fireballPrefab, GameObject lighteningPrefab, GameObject iceSplashPrefab)
        {
            return new PlayerAttackAction()
            {
                animatorCtrl = animatorCtrl,
                rb = rb,
                swordMesh = swordMesh,
                staffMesh = staffMesh,
                fireballPrefab = fireballPrefab,
                lighteningPrefab = lighteningPrefab,
                iceSplashPrefab = iceSplashPrefab,
            };
        }

        public PlayerAttribute Perform(PlayerAttribute playerAttribute, Transform playerTransform, Vector3 magicPositionDelta, UserInput userInput)
        {
            PlayerWeaponState.HandleWeaponVisibility(playerWeaponState, swordMesh, staffMesh);
            if (userInput.swing) animatorCtrl.SetTrigger(TRIGGER_SWING_KEY);
            if (userInput.IsUsingMagic())
            {
                animatorCtrl.SetTrigger(TRIGGER_MAGIC_KEY);
                Skill skill = PlayerSkillAllocation.GetSkill(playerAttribute.skillAllocation, userInput, true);
                CastMagic(skill, playerTransform, magicPositionDelta);
            }
            return playerAttribute;
        }

        private void CastMagic(Skill skill, Transform playerTransform, Vector3 magicPositionDelta)
        {
            if (skill == Skill.FIRE_BALL)
            {
                BulletMono.SpawnXZ(fireballPrefab, playerTransform, magicPositionDelta, skill);
            }
            if (skill == Skill.LIGHTENING)
            {
                BulletMono.SpawnXZ(lighteningPrefab, playerTransform, magicPositionDelta, skill);
            }
            if (skill == Skill.ICE_SPLASH)
            {
                BulletMono.SpawnXZ(iceSplashPrefab, playerTransform, magicPositionDelta, skill);
            }
        }
    }
}