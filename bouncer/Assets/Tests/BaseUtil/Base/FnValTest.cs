using NUnit.Framework;
using BaseUtil.Base;

namespace Tests.BaseUtil.Base
{
    public class FnValTest
    {
        [Test]
        public void TestAsInt()
        {
            Assert.AreEqual(FnVal.AsInt(null), 0);
            Assert.AreEqual(FnVal.AsInt("ABC"), 0);
            Assert.AreEqual(FnVal.AsInt("5"), 5);
        }

        [Test]
        public void TestStartsWith()
        {
            Assert.AreEqual(FnVal.StartsWith("a")("abc"), true);
            Assert.AreEqual(FnVal.StartsWith("")(""), true);
            Assert.AreEqual(FnVal.StartsWith("b")("abc"), false);
            Assert.AreEqual(FnVal.StartsWith("b")(null), false);
            Assert.AreEqual(FnVal.StartsWith("null")(null), false);
            Assert.AreEqual(FnVal.StartsWith(null)("null"), false);
            Assert.AreEqual(FnVal.StartsWith(null)(""), false);
        }

        [Test]
        public void GetNextIndex()
        {
            Assert.AreEqual(FnVal.GetNextIndex(2, 3), 0);
            Assert.AreEqual(FnVal.GetNextIndex(1, 3), 2);
            Assert.AreEqual(FnVal.GetNextIndex(0, 3), 1);
        }

        [Test]
        public void GetPreviousIndex()
        {
            Assert.AreEqual(FnVal.GetPreviousIndex(2, 3), 1);
            Assert.AreEqual(FnVal.GetPreviousIndex(1, 3), 0);
            Assert.AreEqual(FnVal.GetPreviousIndex(0, 3), 2);
        }
    }
}