﻿namespace ProjectContent.Scripts.UI.Base.Comp
{
    public interface ICompEls<in TData>
    {
        public void InitStaticEls(TData data);

        public void UpdateDataDrivenEls(TData data);
    }
}