using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMindEngine
{
    public class Hint
    {                
        public Hint(HintColors[] hints)
        {
            Hints = hints;
        }

        public HintColors[] Hints { get; private set; } = new HintColors[Placement.Size];
    }
}