using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using static MasterMindEngine.GameConfig;

namespace MasterMindEngine
{
    /// <summary>
    /// Enumerate all possible placements of a codeline
    /// </summary>
    public static class Enumerators
    {        
        /// <summary>
        /// Enumerate all "normal" placements
        /// </summary>
        /// <param name="enumOptions"></param>
        /// <returns></returns>
        public static IEnumerable<Placement> GetPlacements(EnumOptions enumOptions = EnumOptionsDefault)
        {
            var enumValues = GetColorValues();

            return GetPlacements(enumValues, enumOptions);
        }

        /// <summary>
        /// Enumerate all placements with the given colors 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Placement> GetPlacements(CodeColors[] enumValues, EnumOptions enumOptions = EnumOptions.ColorOnlyUsedOnce) 
        {
            if(enumOptions.HasFlag(EnumOptions.NoneIsAllowed))
            {
                enumValues = new CodeColors[] { CodeColors.None }.Concat(enumValues).ToArray();
            }
            else
            {
                if(enumValues.Contains(CodeColors.None))
                {
                    throw new ArgumentException("None is not allowed in the enumValues array");
                }
            }

            enumValues = enumValues.Distinct().ToArray();

            var colorCount = enumValues.Length;

            var indexes = new int[CodeLength];

            while (indexes[0] < colorCount)
            {
                var code = new CodeColors[CodeLength];
                for (int i = 0; i < CodeLength; i++)
                {
                    code[i] = enumValues[indexes[i]];
                }

                var p = new Placement(code);

                if (p.isValid(enumOptions))
                {
                    yield return p;
                }
                
                indexes[CodeLength - 1]++;
                for (int i = CodeLength - 1; i > 0; i--)
                {
                    if (indexes[i] == colorCount)
                    {
                        indexes[i] = 0;
                        indexes[i - 1]++;
                    }
                }
            }            
        }

        /// <summary>
        /// Enumerate all placements that fit the other given placement.
        /// The other placement may contain empty slots (Color == None).
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static IEnumerable<Placement> GetPlacements(Placement other, EnumOptions enumOptions = EnumOptionsDefault)
        {
            foreach (var p in GetPlacements(enumOptions))
            {
                if (p.Fits(other))
                {
                    yield return p;
                }
            }
        }               

        public static IEnumerable<Placement> GetPossibleNextPlacements(Placement placement, Hint hint, EnumOptions enumOptions = EnumOptionsDefault)
        {
            var placements = new List<Placement>(); 
            var partialPlacements = GetPossibleNextPartialPlacements(placement, hint);
            
            foreach(var partialPlacement in partialPlacements)
            {
                var p = GetPlacements(partialPlacement, enumOptions).ToList();

                placements.AddRange(p);
            }   

            return placements.Distinct();
        }

        /// <summary>
        /// Enumerates all possible placements that fit the given placement and hint
        /// 
        /// </summary>
        /// <param name="placement"></param>
        /// <param name="hint"></param>
        /// <returns></returns>
        public static IEnumerable<Placement> GetPossibleNextPartialPlacements(Placement placement, Hint hint)
        {
            var placementUnderConstruction = Placement.CreateEmpty(); // start with an empty placement and the full set of hints
            return GetNextPlacementsRecursion(placement, placementUnderConstruction, hint).Distinct();
        }

        private static IEnumerable<Placement> GetNextPlacementsRecursion(Placement plc, Placement plcConstr,  Hint hnt)
        {
            // generate copies of by-reference parameters (this is a recursive function)
            var placement = plc.Clone();
            var placementConstructing = plcConstr.Clone();
            var hint = hnt.Clone();

            if(hint.Hints.Count(h=>h != HintColors.None) == 0)
            {
                yield return placementConstructing; // construction is finished
                yield break;
            }   

                    
            var hi = Array.FindIndex(hint.Hints, h => h != HintColors.None); // get the first hint that is set               
            var h = hint.Hints[hi];
            hint.Hints[hi] = HintColors.None;    // set this hint to none, so that it is not considered in the next recursion

            for(int i = 0; i < CodeLength; ++i)
            {
                var c = placement.Code[i];
                if(c != CodeColors.None)
                {
                    placement.Code[i] = CodeColors.None; // remove the color from the placement, so that it is not considered in the next recursion

                    if(h == HintColors.White)
                    {
                        // place the color c at all other positions than i in the constructing placement (semantics of white hint)
                        for(int j = 0; j < CodeLength; ++j)
                        {
                            if(j != i)
                            {
                                if(placementConstructing.Code[j] == CodeColors.None)
                                {                                     
                                    placementConstructing.Code[j] = c;
                                    foreach(var p in GetNextPlacementsRecursion(placement, placementConstructing, hint))
                                    {
                                        yield return p;
                                    }            
                                    // take back last color assignment
                                    placementConstructing.Code[j] = CodeColors.None;
                                }                                                                                                            
                            }
                        }
                    }

                    if(h == HintColors.Black)
                    {
                        // place the color c exactly at position i in the constructing placement (semantics of black hint)
                        if(placementConstructing.Code[i] == CodeColors.None)
                        {
                            placementConstructing.Code[i] = c;
                            foreach(var p in GetNextPlacementsRecursion(placement, placementConstructing, hint))
                            {
                                yield return p;
                            }                                        
                            // take back last color assignment
                            placementConstructing.Code[i] = CodeColors.None;
                        }                                                                                                            
                    }                                                 
                }
                placement.Code[i] = c;  // take back color assignment for this recursion level
            }                                                                      
        }

        public static IEnumerable<Placement> GenerateMandatoryPlacements(IEnumerable<Turn> turns)
        {
            var turnsWithBlackOnlyHints = turns.Where(t => t.Hint.Hints.Count(h => h == HintColors.Black) > 0 && t.Hint.Hints.Any(h => h == HintColors.White) == false).ToList();

            // calculate all possible combinations of colors for this list and return those placements that fit these combinations

            return new List<Placement>();
        }

        public static IEnumerable<Placement> GenerateForbiddenPlacements(IEnumerable<Turn> turns)
        {
            var result = new List<Placement>(); 

            foreach(var turn in turns)
            {
                if(turn.Hint.Hints.Count(h=>h == HintColors.White) == CodeLength)  // all white and no black
                {
                    ApplyAllWhiteRule(turn, ref result);
                }

                if (turn.Hint.Hints.Count(h=>h == HintColors.White) > 0 && turn.Hint.Hints.Count(h=>h == HintColors.White) < CodeLength && turn.Hint.Hints.Count(h=>h == HintColors.Black) == 0)  // only at least one white hint, no black hint
                {
                    ApplyOnlyWhiteRule(turn, ref result);
                }

                if (turn.Hint.Hints.Count(h=>h == HintColors.None) == CodeLength)  // all none
                {
                    ApplyAllNoneRule(turn, ref result);
                }

                if (turn.Hint.Hints.Count(h=>h == HintColors.Black) > 0 && turn.Hint.Hints.Count(h=>h == HintColors.Black) < CodeLength && turn.Hint.Hints.Count(h=>h == HintColors.White) == 0)  // only at least one black hint, no white hint
                {
                    ApplyOnlyBlackRule(turn, ref result);
                }

                if (turn.Hint.Hints.Count(h=>h == HintColors.Black) == CodeLength)  // all black and no white
                {
                    throw new Exception("Impossible, the game would be over with this hint");
                }

                if (turn.Hint.Hints.Count(h=>h == HintColors.Black) > 0 && turn.Hint.Hints.Count(h=>h == HintColors.White) > 0 && turn.Hint.Hints.Count(h=>h == HintColors.None) == CodeLength)  
                {
                    ApplyAllHintsSetRule(turn, ref result);
                }
            } 
            

            return result.Distinct();
        }

        private static void ApplyAllHintsSetRule(Turn turn, ref List<Placement> result)
        {
            var colors = GetColorValues();
            foreach (var color in colors.Where(c => turn.Placement.Code.Contains(c) == false))  // all colors that are not in the placement
            {
                for (int i = 0; i < CodeLength; i++)
                {
                    var p = Placement.CreateEmpty();
                    p.Code[i] = color;
                    result.Add(p);
                }
            }
        }

        private static void ApplyOnlyBlackRule(Turn turn, ref List<Placement> result)
        {
            // think about whether something can be deduced here
        }



        private static void ApplyAllNoneRule(Turn turn, ref List<Placement> result)
        {
            // all colors that are contained in the placement are forbidden
            var colors = GetColorValues();
            foreach (var color in colors.Where(c => turn.Placement.Code.Contains(c)))  // all colors that are in the placement
            {
                for (int i = 0; i < CodeLength; i++)
                {
                    var p = Placement.CreateEmpty();
                    p.Code[i] = color;
                    result.Add(p);
                }                
            }
        }

        private static void ApplyOnlyWhiteRule(Turn turn, ref List<Placement> result)
        {
            // all colors that are contained in the placement are forbidden at their current position
            var colors = GetColorValues();
            foreach (var color in colors.Where(c => turn.Placement.Code.Contains(c)))  // all colors that are in the placement
            {
                var colorIndex = turn.Placement.Code.ToList().FindIndex(c => c == color);
                var p = Placement.CreateEmpty();
                p.Code[colorIndex] = color;

                result.Add(p);
            }
        }

        private static void ApplyAllWhiteRule(Turn turn, ref List<Placement> result)
        {
            // all colors that are contained in the placement are forbidden at their current position and at all other colors are forbidden at all positions
            var colors = GetColorValues();
            foreach (var color in colors.Where(c => turn.Placement.Code.Contains(c) == false))  // all colors that are not in the placement
            {
                for (int i = 0; i < CodeLength; i++)
                {
                    var p = Placement.CreateEmpty();
                    p.Code[i] = color;
                    result.Add(p);
                }
            }

            foreach (var color in colors.Where(c => turn.Placement.Code.Contains(c)))  // all colors that are in the placement
            {
                var colorIndex = turn.Placement.Code.ToList().FindIndex(c => c == color);
                var p = Placement.CreateEmpty();
                p.Code[colorIndex] = color;

                result.Add(p);
            }
        }
    }
}
