using UnityEngine.UIElements;

namespace ProjectContent.Scripts.UI.Base.Util
{
    public class UiUtil
    {
        public static void ToggleClass(VisualElement el, string className, bool shouldBeOn)
        {
            if (shouldBeOn)
            {
                el.AddToClassList(className);
            }
            else
            {
                el.RemoveFromClassList(className);
            }
        }
    }
}