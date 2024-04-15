using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static IEnumerable<Placement> GetPlacements(EnumOptions enumOptions = EnumOptions.ColorOnlyIUsedOnce) 
        {
            var enumValues = ((CodeColors[]) Enum.GetValues(typeof(CodeColors))).Where(c=>c!=CodeColors.None).ToArray(); 

            return GetPlacements(enumValues, enumOptions);
        }

        /// <summary>
        /// Enumerate all placements with the given colors 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Placement> GetPlacements(CodeColors[] enumValues, EnumOptions enumOptions = EnumOptions.ColorOnlyIUsedOnce) 
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

            var indexes = new int[Placement.Size];

            while (indexes[0] < colorCount)
            {
                var code = new CodeColors[Placement.Size];
                for (int i = 0; i < Placement.Size; i++)
                {
                    code[i] = enumValues[indexes[i]];
                }

                var p = new Placement(code);

                if (p.isValid(enumOptions))
                {
                    yield return p;
                }
                
                indexes[Placement.Size - 1]++;
                for (int i = Placement.Size - 1; i > 0; i--)
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
        public static IEnumerable<Placement> GetPlacements(Placement other)
        {
            foreach (var p in GetPlacements())
            {
                if (p.Fits(other))
                {
                    yield return p;
                }
            }
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

            for(int i = 0; i < Placement.Size; ++i)
            {
                var c = placement.Code[i];
                if(c != CodeColors.None)
                {
                    placement.Code[i] = CodeColors.None; // remove the color from the placement, so that it is not considered in the next recursion

                    if(h == HintColors.White)
                    {
                        // place the color c at all other positions than i in the constructing placement (semantics of white hint)
                        for(int j = 0; j < Placement.Size; ++j)
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
