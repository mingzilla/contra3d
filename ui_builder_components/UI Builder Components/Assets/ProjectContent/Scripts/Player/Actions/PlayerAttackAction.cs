using BaseUtil.GameUtil;
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

        public static PlayerAttackAction Create(Animator animatorCtrl, Rigidbody rb, GameObject swordMesh, GameObject staffMesh)
        {
            return new PlayerAttackAction()
            {
                animatorCtrl = animatorCtrl,
                rb = rb,
                swordMesh = swordMesh,
                staffMesh = staffMesh,
            };
        }

        public PlayerAttribute Perform(PlayerAttribute playerAttribute, UserInput userInput)
        {
            PlayerWeaponState.HandleWeaponVisibility(playerWeaponState, swordMesh, staffMesh);
            if (userInput.swing) animatorCtrl.SetTrigger(TRIGGER_SWING_KEY);
            if (userInput.IsUsingMagic()) animatorCtrl.SetTrigger(TRIGGER_MAGIC_KEY);
            return playerAttribute;
        }
    }
}