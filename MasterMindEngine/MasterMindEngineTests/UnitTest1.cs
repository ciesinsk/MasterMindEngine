using MasterMindEngine;

namespace MasterMindEngineTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestEnumerationOfAllPlacements()
        {
            var p = Enumerators.GetPlacements().ToList();
            Assert.AreEqual(24, p.Count);
            p = p.Distinct().ToList();
            Assert.AreEqual(24, p.Count);
            
            var p2 = Enumerators.GetPlacements(EnumOptions.NoRestrictions).ToList();
            Assert.AreEqual(256, p2.Count); // 4^4 = 256
            p2 = p2.Distinct().ToList();
            Assert.AreEqual(256, p2.Count);

                                            // 
            var p3 = Enumerators.GetPlacements(EnumOptions.NoneIsAllowed).ToList();
            Assert.AreEqual(625, p3.Count); // 4^4 = 256 + none allowe
            p3 = p3.Distinct().ToList();
            Assert.AreEqual(625, p3.Count);


            var p4 = Enumerators.GetPlacements(EnumOptions.NoneIsAllowed|EnumOptions.ColorOnlyIUsedOnce).ToList();
            Assert.AreEqual(209, p4.Count); // 4^4 = 256 + none allowe
            p4 = p4.Distinct().ToList();
            Assert.AreEqual(209, p4.Count);
        }

        [TestMethod]
        public void TestEnumerationOfPossibleMatching()
        {
            var p = new Placement(new CodeColors[] { CodeColors.Red, CodeColors.Blue, CodeColors.Yellow, CodeColors.Green });
            var h = new Hint( new HintColors[] { HintColors.White, HintColors.White, HintColors.Black, HintColors.None});

            var placements = Enumerators.GetPossiblePartialNextPlacements(p, h);            

            Assert.AreEqual(6, placements.Count());
        }


        [TestMethod]
        public void TestEnumerationOfMatchingPlacements()
        {
            var p1 = new Placement(new CodeColors[] { CodeColors.Red, CodeColors.None, CodeColors.None, CodeColors.None });

            var p = Enumerators.GetPlacements(p1).ToList();
            Assert.AreEqual(6, p.Count);
        }

        [TestMethod]
        public void TestFitsMethod()
        {
            var p1 = new Placement(new CodeColors[] { CodeColors.Red, CodeColors.None, CodeColors.None, CodeColors.None });
            var p2 = new Placement(new CodeColors[] { CodeColors.Red, CodeColors.Blue, CodeColors.None, CodeColors.None });

            var r1 = p1.Fits(p2);
            var r2 = p2.Fits(p1);

            Assert.IsFalse(r1);  // here we expect false, because p2 requires a blue in the second position, which p1 does not have
            Assert.IsTrue(r2);

            
        }

    }
}