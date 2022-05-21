using NUnit.Framework;
using BaseUtil.Base;

namespace Tests.BaseUtil.Base
{
    public class FnRxTest
    {
        [Test]
        public void TestSubscription()
        {
            int value = 0;
            var subject = new BehaviorSubject<int>();
            subject.AsObservable().Subscribe((int data) => {
                value = data;
            });

            Assert.AreEqual(value, 0);

            subject.Next(1);
            Assert.AreEqual(value, 1);
        }
    }
}