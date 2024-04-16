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

            var placements = Enumerators.GetPossibleNextPartialPlacements(p, h);            

            Assert.AreEqual(36, placements.Count());

            p = new Placement(new CodeColors[] { CodeColors.Red, CodeColors.Blue, CodeColors.Yellow, CodeColors.Green });
            h = new Hint( new HintColors[] { HintColors.Black, HintColors.None, HintColors.None, HintColors.None});

            placements = Enumerators.GetPossibleNextPartialPlacements(p, h);            

            Assert.AreEqual(4, placements.Count());

            p = new Placement(new CodeColors[] { CodeColors.Red, CodeColors.Blue, CodeColors.Yellow, CodeColors.Green });
            h = new Hint( new HintColors[] { HintColors.White, HintColors.None, HintColors.None, HintColors.None});

            placements = Enumerators.GetPossibleNextPartialPlacements(p, h);       
            
            Assert.AreEqual(12, placements.Count());

            p = new Placement(new CodeColors[] { CodeColors.Red, CodeColors.Blue, CodeColors.Yellow, CodeColors.Green });
            h = new Hint( new HintColors[] { HintColors.Black, HintColors.Black, HintColors.Black, HintColors.Black});

            placements = Enumerators.GetPossibleNextPartialPlacements(p, h);       
            
            Assert.AreEqual(1, placements.Count());
        }

        [TestMethod]
        public void TestIEquality()
        {
            var p1 = new Placement(new CodeColors[] { CodeColors.Red, CodeColors.None, CodeColors.None, CodeColors.None });
            var p2 = new Placement(new CodeColors[] { CodeColors.Red, CodeColors.None, CodeColors.None, CodeColors.None });

            
            Assert.AreEqual(p1, p2);


            var list = new List<Placement>{p1, p2};

            var list2 = list.Distinct().ToList();

            Assert.AreEqual(list2.Count, 1);
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


        [TestMethod]
        public void TestIntersectOfTwoPlacementEnumerations()
        {
            var p1 = new Placement(new CodeColors[] { CodeColors.Red, CodeColors.None, CodeColors.None, CodeColors.None });

            var placements1 = Enumerators.GetPlacements(p1).ToList();
            Assert.AreEqual(6, placements1.Count);

            var p2 = new Placement(new CodeColors[] { CodeColors.Red, CodeColors.Blue, CodeColors.None, CodeColors.None });

            var placements2 = Enumerators.GetPlacements(p2).ToList();
            Assert.AreEqual(2, placements2.Count);

            var placements3 = placements1.Intersect(placements2).ToList();

            Assert.AreEqual(2, placements3.Count);
        }


        
        [TestMethod]
        public void TestCompletionOfPartialEnumeration()
        {
            var p1 = new Placement(new CodeColors[] { CodeColors.Red, CodeColors.Yellow, CodeColors.Green, CodeColors.Blue });

            var nextMoves = new List<Placement>();
            var nextPartialMoves = Enumerators.GetPossibleNextPartialPlacements(p1, new Hint(new HintColors[] { HintColors.Black, HintColors.White, HintColors.None, HintColors.None }));

            foreach(var p in nextPartialMoves)
            {
                var completePlacements = Enumerators.GetPlacements(p);
                nextMoves.AddRange(completePlacements);
            }   

            nextMoves = nextMoves.Distinct().ToList();

            Assert.AreEqual(14, nextMoves.Count);


            nextMoves = Enumerators.GetPossibleNextPlacements(p1, new Hint(new HintColors[] { HintColors.Black, HintColors.White, HintColors.None, HintColors.None })).ToList();

            Assert.AreEqual(14, nextMoves.Count);
        }
    }
}