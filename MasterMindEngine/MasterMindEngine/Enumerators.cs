using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMindEngine
{
    public static class Enumerators
    {
        public static IEnumerable<Placement> GetPlacements() 
        {
            var enumValues = ((CodeColors[]) Enum.GetValues(typeof(CodeColors))).Where(c=>c!=CodeColors.None).ToArray();            
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

                if (p.isValid())
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
    }
}
