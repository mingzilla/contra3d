using BaseUtil.Base;

namespace ProjectContent.Scripts.UI.Base.Comp
{
    public abstract class AbstractCompData<TData> : ICompData<TData>
    {
        public bool IsTheSameAs(TData dataIn)
        {
            return Fn.AreEqualObjs(this, dataIn);
        }
    }
}