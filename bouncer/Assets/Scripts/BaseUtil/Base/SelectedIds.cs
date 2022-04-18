using System;
using System.Collections.Generic;

namespace BaseUtil.Base
{
    public class SelectedIds
    {
        private FnOldDict<string, string> selectedIds;

        public SelectedIds()
        {
            this.selectedIds = new FnOldDict<string, string>();
        }

        public static SelectedIds Create()
        {
            return new SelectedIds();
        }

        public SelectedIds ToggleId(string id)
        {
            this.selectedIds = this.IsSelected(id) ?
                this.selectedIds.Remove(id) :
                this.selectedIds.SetItem(id, id);
            return this;
        }

        public SelectedIds SelectId(string id)
        {
            this.selectedIds = this.selectedIds.SetItem(id, id);
            return this;
        }

        public SelectedIds RemoveId(string id)
        {
            this.selectedIds = this.selectedIds.Remove(id);
            return this;
        }

        public SelectedIds SelectAll(List<string> ids)
        {
            var selectedIds = this.selectedIds;
            ids.ForEach((id) => { selectedIds = selectedIds.SetItem(id, id); });
            this.selectedIds = selectedIds;
            return this;
        }

        public SelectedIds RemoveAll(List<string> ids)
        {
            var selectedIds = this.selectedIds;
            ids.ForEach((id) => { selectedIds = selectedIds.Remove(id); });
            this.selectedIds = selectedIds;
            return this;
        }

        public SelectedIds SetAll(List<string> ids, bool include)
        {
            return include ? this.SelectAll(ids) : this.RemoveAll(ids);
        }

        public SelectedIds RemoveAllByPrefix(string idPrefix)
        {
            var ids = Fn.Filter<string>(FnVal.StartsWith(idPrefix), this.GetIds());
            return this.RemoveAll(ids);
        }

        public SelectedIds Reset()
        {
            return SelectedIds.Create();
        }

        public bool IsSelected(string id)
        {
            return this.selectedIds.ContainsKey(id);
        }

        public bool IsEmpty()
        {
            return this.selectedIds.IsEmpty();
        }

        public List<string> GetIds()
        {
            return this.selectedIds.GetKeys();
        }

        public int GetCount()
        {
            return this.selectedIds.GetCount();
        }

        public List<int> GetNumericIds()
        {
            return Fn.Map<string, int>(FnVal.AsInt, this.GetIds());
        }
    }
}