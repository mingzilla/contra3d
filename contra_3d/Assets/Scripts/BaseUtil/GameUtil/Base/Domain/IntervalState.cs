namespace BaseUtil.GameUtil.Base.Domain
{
    public class IntervalState
    {
        public float interval;
        public bool canRun;

        public static IntervalState Create(float interval)
        {
            return new IntervalState()
            {
                interval = interval,
                canRun = true,
            };
        }
    }
}