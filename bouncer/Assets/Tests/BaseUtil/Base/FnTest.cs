using NUnit.Framework;
using System.Collections.Generic;
using BaseUtil.Base;

namespace Tests.BaseUtil.Base
{
    public class FnTest
    {
        [Test]
        public void TestAll()
        {
            List<int> list1 = new List<int> {1, 2};
            bool result1 = Fn.All((x) => x > 0, list1);
            Assert.AreEqual(result1, true);

            List<int> list2 = new List<int> {1, 2};
            bool result2 = Fn.All((x) => x > 1, list1);
            Assert.AreEqual(result2, false);
        }

        [Test]
        public void TestConcatAll()
        {
            List<int> list1 = new List<int> {1, 2};
            List<int> list2 = new List<int> {2, 3};
            List<int> list3 = new List<int> {4, 5};
            List<int> result = Fn.ConcatAll(new List<List<int>> {list1, list2, list3});

            Assert.AreEqual(result, new List<int> {1, 2, 2, 3, 4, 5});
        }
    }
}