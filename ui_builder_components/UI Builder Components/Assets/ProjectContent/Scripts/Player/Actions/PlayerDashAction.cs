using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContent.Scripts.Data;
using UnityEngine;

namespace ProjectContent.Scripts.Player.Actions
{
    public class PlayerDashAction
    {
        private static readonly int TRIGGER_ROLL_KEY = Animator.StringToHash("triggerRoll");
        public readonly IntervalState dashInterval = IntervalState.Create(0.2f);

        private Animator animatorCtrl;
        private Rigidbody rb;

        public static PlayerDashAction Create(Animator animatorCtrl, Rigidbody rb)
        {
            return new PlayerDashAction()
            {
                animatorCtrl = animatorCtrl,
                rb = rb,
            };
        }

        public PlayerAttribute Perform(PlayerAttribute playerAttribute, UserInput userInput)
        {
            if (userInput.dash) UnityFn.HandleDash(rb, playerAttribute.dashForce);
            if (userInput.dash) animatorCtrl.SetTrigger(TRIGGER_ROLL_KEY);
            return playerAttribute;
        }
    }
}