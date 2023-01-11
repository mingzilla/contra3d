using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Util3D;
using ProjectContent.Scripts.Data;
using UnityEngine;

namespace ProjectContent.Scripts.Player.Actions
{
    public class PlayerJumpAction
    {
        private static readonly int IS_ON_GROUND_KEY = Animator.StringToHash("isOnGround");
        private static readonly int TRIGGER_JUMP_KEY = Animator.StringToHash("triggerJump");

        private Animator animatorCtrl;
        private Rigidbody rb;

        public static PlayerJumpAction Create(Animator animatorCtrl, Rigidbody rb)
        {
            return new PlayerJumpAction()
            {
                animatorCtrl = animatorCtrl,
                rb = rb,
            };
        }

        public PlayerAttribute Perform(PlayerAttribute playerAttribute, UserInput userInput, bool isOnGround)
        {
            animatorCtrl.SetBool(IS_ON_GROUND_KEY, isOnGround);
            if (userInput.jump && isOnGround) animatorCtrl.SetTrigger(TRIGGER_JUMP_KEY);
            if (userInput.jump) PlayerActionHandler3D.HandleJumpFromGround(isOnGround, rb, playerAttribute.jumpForce);
            PlayerActionHandler3D.HandleGravityModification(rb, playerAttribute.gravityMultiplier);
            return playerAttribute;
        }
    }
}