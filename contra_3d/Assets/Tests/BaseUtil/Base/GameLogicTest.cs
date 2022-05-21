using NUnit.Framework;
using BaseUtil.Base;

namespace Tests.BaseUtil.Base
{
    public class GameLogicTest
    {
        [Test]
        public void TestHandleJumpFromGroundOrDoubleJump()
        {
            bool runJumpFn1 = false;
            bool isInDoubleJumpStatus1 = GameLogic.HandleJumpFromGroundOrDoubleJump(true, true, () => runJumpFn1 = true); // impossible case
            Assert.AreEqual(isInDoubleJumpStatus1, false);
            Assert.AreEqual(runJumpFn1, true);

            bool runJumpFn2 = false;
            bool isInDoubleJumpStatus2 = GameLogic.HandleJumpFromGroundOrDoubleJump(true, false, () => runJumpFn2 = true);
            Assert.AreEqual(isInDoubleJumpStatus2, false);
            Assert.AreEqual(runJumpFn2, true);

            bool runJumpFn3 = false;
            bool isInDoubleJumpStatus3 = GameLogic.HandleJumpFromGroundOrDoubleJump(false, true, () => runJumpFn3 = true);
            Assert.AreEqual(isInDoubleJumpStatus3, true);
            Assert.AreEqual(runJumpFn3, false);

            bool runJumpFn4 = false;
            bool isInDoubleJumpStatus4 = GameLogic.HandleJumpFromGroundOrDoubleJump(false, false, () => runJumpFn4 = true);
            Assert.AreEqual(isInDoubleJumpStatus4, true);
            Assert.AreEqual(runJumpFn4, true);
        }
    }
}