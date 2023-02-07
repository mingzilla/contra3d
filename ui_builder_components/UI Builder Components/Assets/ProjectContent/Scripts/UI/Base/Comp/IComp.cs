namespace ProjectContent.Scripts.UI.Base.Comp
{
    public interface IComp<in TData>
    {
        public void Init(TData dataIn);
        public void OnUpdate(TData dataIn);
    }
}