namespace ProjectContent.Scripts.UI.Base.Comp
{
    public interface ICompData<in TData>
    {
        public bool IsTheSameAs(TData dataIn);
    }
}