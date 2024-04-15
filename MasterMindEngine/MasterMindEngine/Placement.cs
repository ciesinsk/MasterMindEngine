
using System.Text;

namespace MasterMindEngine
{
    /// <summary>
    /// This class represents the placement of one codeline in the game.
    /// </summary>
    public class Placement: IEquatable<Placement>
    {
        public const int Size = 4; 
        

        /// <summary>
        /// Create an empty placement
        /// </summary>
        /// <returns></returns>
        public static Placement CreateEmpty()
        {
            var code = new CodeColors[Size];
            for (int i = 0; i < Size; i++)
            {
                code[i] = CodeColors.None;
            }   

            return new Placement(code);
        }

        public Placement(CodeColors[] code)
        {
            Code = code;
        }

        /// <summary>
        /// Check whether the placement is a valid one, i.e, there are no empty slots and no repeated colors
        /// </summary>
        /// <returns>false if the placement is invalid</returns>
        public bool isValid(EnumOptions enumOptions)
        { 
            if(enumOptions.HasFlag(EnumOptions.NoneIsAllowed) == false && Code.Any(c=>c == CodeColors.None))
            {
                return false;
            }   

            if(enumOptions.HasFlag(EnumOptions.ColorOnlyIUsedOnce))
            {
                if(Code.Where(c=>c != CodeColors.None).Distinct().Count() != Code.Where(c=>c != CodeColors.None).Count())
                {
                    return false;
                }    
            }

            return true;
        }

        /// <summary>
        /// Determine whether a placement fits another placement
        /// </summary>
        /// <param name="other"></param>
        /// <returns>True if the placement fits the other placement</returns>
        public bool Fits(Placement other)
        {            
            for (int i = 0; i < Placement.Size; i++)
            {
                if(other.Code[i] != CodeColors.None && other.Code[i] != Code[i])
                {
                    return false;
                }                                
            }
            return true;
        }

        /// <summary>
        /// Deep copy of the placement
        /// </summary>
        /// <returns>A deep copy of the placement</returns>
        public Placement Clone()
        {
            // cloe the code array to avoid by-reference copying
            var newCode = new CodeColors[Size];
            for (int i = 0; i < Size; i++)
            {
                newCode[i] = Code[i];
            }
            
            return new Placement(newCode);
        }

        /// <summary>
        /// The actual placement of the colors, in the original game called "Code"
        /// </summary>
        public CodeColors[] Code { get; private set; } = new CodeColors[Size];

        /// <summary>
        /// Standard override of the ToString method
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach(var c in Code)
            {
                sb.Append(c + " ");                    
            }            

            return sb.ToString();
        }

        /// <summary>
        /// Standard override of the Equals method
        /// </summary>
        public bool Equals(Placement? other)
        {
            if(other == null)
            {
                return false;
            }   

            for (int i = 0; i < Size; i++)
            {
                if(other.Code[i] != Code[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Standard override of the Equals method
        /// </summary>
        public override bool Equals(object? other)
        {
            var otherPlacement = other as Placement;
            return Equals(otherPlacement);
        }

        /// <summary>
        /// Standard override of the GetHashCode method
        /// </summary>
        public override int GetHashCode()
        {
            if (Code == null) return 0;
            unchecked
            {
                int hash = 17;
                for(int i = 0; i < Code.Length; i++)
                    hash = 31 * hash + Code[i].GetHashCode();
                return hash;
            }
        }
    }
}
