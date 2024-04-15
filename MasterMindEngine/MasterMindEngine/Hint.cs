using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMindEngine
{
    public class Hint
    {                
        public Hint Clone()
        {
            var hintsCopy = new HintColors[Placement.Size];
            for (int i = 0; i < Placement.Size; i++)
            {
                hintsCopy[i] = Hints[i];
            }

            return new Hint(hintsCopy);
        }


        public Hint(HintColors[] hints)
        {
            
            Hints = hints;
        }

        public HintColors[] Hints { get; private set; } = new HintColors[Placement.Size];

        
    }
}