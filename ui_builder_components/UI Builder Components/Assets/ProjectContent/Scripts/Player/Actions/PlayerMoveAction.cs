using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Util3D;
using ProjectContent.Scripts.Data;
using UnityEngine;

namespace ProjectContent.Scripts.Player.Actions
{
    public class PlayerMoveAction
    {
        private static readonly int IS_MOVING_KEY = Animator.StringToHash("isMoving");
        public MoveSpeedModifier moveSpeedModifier = MoveSpeedModifier.Create();

        private Animator animatorCtrl;
        private Rigidbody rb;

        public static PlayerMoveAction Create(Animator animatorCtrl,  Rigidbody rb)
        {
            return new PlayerMoveAction()
            {
                animatorCtrl = animatorCtrl,
                rb = rb,
            };
        } 

        public PlayerAttribute Perform(PlayerAttribute playerAttribute, UserInput userInput, Transform transform, bool isOnGround)
        {
            PlayerActionHandler3D.FighterMoveXZ(userInput, rb, playerAttribute.moveSpeed, moveSpeedModifier.speedModifier, isOnGround);
            animatorCtrl.SetBool(IS_MOVING_KEY, moveSpeedModifier.CanUseMoveAnimation() && userInput.IsMoving());
            UnitDisplayHandler3D.HandleXZFacing(transform, userInput);
            return playerAttribute;
        }
    }
}