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

        /// <summary>
        /// Check whether the placement is a valid one
        /// </summary>
        /// <returns>false if the placement is invalid</returns>
        public bool isValid()
        {
            if(Code.Any(c=>c == CodeColors.None))
            {
                return false;
            }   

            if(Code.Distinct().Count() == Code.Length)
            {
                return true;
            }   

            return false;
        }

        public CodeColors[] Code { get; private set; } = new CodeColors[Size];
    }
}
