﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
