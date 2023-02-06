using System.Collections.Generic;
using BaseUtil.Base;

namespace BaseUtil.GameUtil.Types
{
    public class UiItemSelectionState
    {
        public static readonly UiItemSelectionState IS_VISIBLE = Create("IS_VISIBLE"); // normal state - present but not on
        public static readonly UiItemSelectionState IS_INVISIBLE = Create("IS_INVISIBLE"); // not visible but reserves space
        public static readonly UiItemSelectionState IS_ENABLED = Create("IS_ENABLED"); // green - toggled on
        public static readonly UiItemSelectionState IS_DISABLED = Create("IS_DISABLED"); // grey - disabled, not possible to be enabled
        public static readonly UiItemSelectionState IS_SELECTED = Create("IS_SELECTED"); // red box - cursor is on the item

        public string name;

        static Dictionary<string, UiItemSelectionState> typeMap = Fn.ListToDictionaryWithKeyFn((x) => x.name, All());

        static List<UiItemSelectionState> All()
        {
            return new List<UiItemSelectionState>()
            {
                IS_VISIBLE,
                IS_INVISIBLE,
                IS_ENABLED,
                IS_DISABLED,
                IS_SELECTED,
            };
        }

        protected static UiItemSelectionState Create(string name)
        {
            var layer = new UiItemSelectionState
            {
                name = name
            };
            return layer;
        }

        public static UiItemSelectionState GetByName(string name)
        {
            return typeMap[(name)];
        }
    }
}