namespace BaseUtil.GameUtil.Domain
{
    public class DashCounter
    {
        public static float dashingCooldownTime = 0.25f;
        public static float dashingImageLifeTime = 0.5f;
        public static float dashingImageCloneDelay = 0.04f;

        public float dashProgressCounter = 0f; // > 0 means currently is dashing
        public float dashingImageCloneCounter = 0f; // delay for creating clones of dash images
        public float dashingCooldownCounter = 0f; // to prevent unlimited dash
        public bool isDashingActivated = false; // to make sure we can trigger event when activated, not constantly while dashing

        public static DashCounter Create(float dashProgressCounter, float dashingImageCloneCounter, float dashingCooldownCounter, bool isDashingActivated)
        {
            DashCounter dashCounter = new DashCounter();
            dashCounter.dashProgressCounter = dashProgressCounter;
            dashCounter.dashingImageCloneCounter = dashingImageCloneCounter;
            dashCounter.dashingCooldownCounter = dashingCooldownCounter;
            dashCounter.isDashingActivated = isDashingActivated;
            return dashCounter;
        }

        public static DashCounter CreateInitial()
        {
            return Create(0f, 0f, 0f, false);
        }

        public static DashCounter UpdateDashingStatus(bool inputFire2, DashCounter dashCounter, float deltaTime, float playerDashTime, bool hasDashAbility)
        {
            if (!hasDashAbility) return dashCounter;

            float dashProgressCounter = dashCounter.dashProgressCounter;
            float dashingCooldownCounter = dashCounter.dashingCooldownCounter;
            bool isDashingActivated = false;

            dashingCooldownCounter -= deltaTime;
            if (inputFire2)
            {
                if (dashingCooldownCounter <= 0)
                {
                    dashProgressCounter = playerDashTime;
                    isDashingActivated = true;
                }
            }

            bool isInDashingStatus = dashProgressCounter > 0;

            if (isInDashingStatus)
            {
                dashProgressCounter = dashProgressCounter - deltaTime;
                dashingCooldownCounter = dashingCooldownTime;
            }

            return DashCounter.Create(dashProgressCounter, dashCounter.dashingImageCloneCounter, dashingCooldownCounter, isDashingActivated);
        }

        public bool IsInDashingStatus()
        {
            return dashProgressCounter > 0;
        }
    }
}