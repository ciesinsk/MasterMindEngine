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

            
        }
    }
}
