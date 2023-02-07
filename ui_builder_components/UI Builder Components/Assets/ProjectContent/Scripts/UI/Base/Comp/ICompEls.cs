namespace ProjectContent.Scripts.UI.Base.Comp
{
    public interface ICompEls<in TData>
    {
        public void InitStaticEls();

        public void UpdateDynamicEls(TData data);
    }
}