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
        


        public static IEnumerable<Placement> GetPlacements(Placement other, Hint hint)
        {
            foreach (var p in GetPlacements())
            {
                if (p.Fits(other))
                {
                    yield return p;
                }
            }
        }

        // enuerate possible future placements that respect a placement and a hint

        /// <summary>
        /// Enumerates all possible placements that fit the given placement and hint
        /// 
        /// </summary>
        /// <param name="placement"></param>
        /// <param name="hint"></param>
        /// <returns></returns>
        public static IEnumerable<Placement> GetPossiblePartialNextPlacements(Placement placement, Hint hint)
        {
            var placementUnderConstruction = Placement.CreateEmpty(); // start with an empty placement and the full set of hints
            return GetPossiblePartialNextPlacementsInternal(placement, placementUnderConstruction, hint);
        }

        private static IEnumerable<Placement> GetPossiblePartialNextPlacementsInternal(Placement placement, Placement placementUnderConstruction,  Hint hint)
        {
            var hintCopy = hint.Clone();

            while(hintCopy.Hints.Count(h=>h != HintColors.None) > 0)
            {            
                var k = Array.FindIndex(hintCopy.Hints, h => h != HintColors.None); // get the first hint that is not None                
                var h = hintCopy.Hints[k];
                hintCopy.Hints[k] = HintColors.None;    // "erase" that entry since it is used here                

                for(int i = 0; i < Placement.Size; ++i)
                {
                    var c = placement.Code[i];
                    if(c != CodeColors.None)
                    {
                        if(h == HintColors.White)
                        {
                            // place the color c at all other positions than i
                            for(int j = 0; j < Placement.Size; ++j)
                            {
                                if(j != i)
                                {
                                    if(placementUnderConstruction.Code[j] == CodeColors.None)
                                    {                                     
                                        placementUnderConstruction.Code[j] = c;
                                        foreach(var p in GetPossiblePartialNextPlacementsInternal(placement, placementUnderConstruction, hintCopy))
                                        {
                                            yield return p;
                                        }                                        
                                    }                                                                                                            
                                }
                            }
                        }

                        if(h == HintColors.Black)
                        {
                            // place the color c at position i
                            if(placementUnderConstruction.Code[i] == CodeColors.None)
                            {
                                placementUnderConstruction.Code[i] = c;
                                foreach(var p in GetPossiblePartialNextPlacementsInternal(placement, placementUnderConstruction, hintCopy))
                                {
                                    yield return p;
                                }                                        
                            }                                                                                                            
                        }   
                    }
                }                             
            }
            yield break;
        }
    }
}
