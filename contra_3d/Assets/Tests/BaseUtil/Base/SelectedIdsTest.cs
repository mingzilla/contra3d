using NUnit.Framework;
using System.Collections.Generic;
using BaseUtil.Base;

namespace Tests.BaseUtil.Base
{
    public class SelectedIdsTest
    {
        [Test]
        public void Test_select_empty()
        {
            var ids = SelectedIds.Create().SelectAll(new List<string> {"", "  "});

            Assert.AreEqual(ids.IsSelected(""), true);
            Assert.AreEqual(ids.IsSelected("  "), true);
        }

        [Test]
        public void Test_select_all()
        {
            var ids = SelectedIds.Create().SelectAll(new List<string> {"1", "2", "3"});
            Assert.AreEqual(ids.GetIds(), new List<string> {"1", "2", "3"});
            Assert.AreEqual(ids.IsSelected("1"), true);
            Assert.AreEqual(ids.IsSelected("2"), true);
            Assert.AreEqual(ids.IsSelected("3"), true);

            ids = SelectedIds.Create().SelectAll(new List<string> {"1", "2", ""});
            Assert.AreEqual(ids.GetIds(), new List<string> {"1", "2", ""});
            Assert.AreEqual(ids.IsSelected("1"), true);
            Assert.AreEqual(ids.IsSelected("2"), true);
            Assert.AreEqual(ids.IsSelected(""), true);
        }

        [Test]
        public void Test_toggle_id()
        {
            var ids = SelectedIds.Create();

            ids = ids.SelectId("1");
            Assert.AreEqual(ids.IsSelected("1"), true);
            ids = ids.ToggleId("1");
            Assert.AreEqual(ids.IsSelected("1"), false);

            ids = ids.SelectId("");
            Assert.AreEqual(ids.IsSelected(""), true);
            ids = ids.ToggleId("");
            Assert.AreEqual(ids.IsSelected(""), false);
        }

        [Test]
        public void Test_remove()
        {
            var ids = SelectedIds.Create().SelectAll(new List<string> {"1", ""});
            Assert.AreEqual(ids.IsSelected("1"), true);
            Assert.AreEqual(ids.IsSelected(""), true);

            ids = ids.RemoveId("1");
            Assert.AreEqual(ids.IsSelected("1"), false);
            Assert.AreEqual(ids.IsSelected(""), true);


            ids = ids.RemoveId("");
            Assert.AreEqual(ids.IsSelected("1"), false);
            Assert.AreEqual(ids.IsSelected(""), false);
        }

        [Test]
        public void Test_remove_all()
        {
            var ids = SelectedIds.Create().SelectAll(new List<string> {"1", ""});
            Assert.AreEqual(ids.IsSelected("1"), true);
            Assert.AreEqual(ids.IsSelected(""), true);

            ids = ids.RemoveAll(new List<string> {"1", ""});
            Assert.AreEqual(ids.IsSelected("1"), false);
            Assert.AreEqual(ids.IsSelected(""), false);
        }

        [Test]
        public void Test_get_numeric_ids()
        {
            var ids = SelectedIds.Create().SelectAll(new List<string> {"1", "2"});
            Assert.AreEqual(ids.GetNumericIds(), new List<int> {1, 2});
        }
    }
}