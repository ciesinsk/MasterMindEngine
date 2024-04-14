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
        public static IEnumerable<Placement> GetPlacements(EnumOptions enumOptions = EnumOptions.ColorOnlyIUsedOnce) 
        {
            var enumValues = ((CodeColors[]) Enum.GetValues(typeof(CodeColors))).Where(c=>c!=CodeColors.None).ToArray(); 

            return GetPlacements(enumValues, enumOptions);
        }


        /// <summary>
        /// Enumerate all placements
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
        
        public static IEnumerable<Placement> GetPartialPlacements(Placement other, Hint hint)
        {
            // not implemented
            return Enumerable.Empty<Placement>();
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


    }
}
