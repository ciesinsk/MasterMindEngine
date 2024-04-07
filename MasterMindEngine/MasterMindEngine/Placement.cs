using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMindEngine
{
    public class Placement
    {
        public const int Size = 4;
        
        public Placement(CodeColors[] code)
        {
            Code = code;
        }

        public CodeColors[] Code { get; private set; } = new CodeColors[Size];
    }
}
