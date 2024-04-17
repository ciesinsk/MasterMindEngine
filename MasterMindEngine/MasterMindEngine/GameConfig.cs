using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMindEngine
{
    public static class GameConfig
    {
        public const int MaxTurnsDefault = 10;
        public const int ColorCountDefault = 6;
        public const int CodeLengthDefault = 4;
        public const EnumOptions EnumOptionsDefault = EnumOptions.ColorOnlyUsedOnce;

        public static int MaxTurns {get;private set; } = MaxTurnsDefault;        
        public static int ColorCount {get;private set; } = ColorCountDefault;
        public static int CodeLength {get;private set; } = CodeLengthDefault; 

        public static EnumOptions CodeOptions { get; set; } = EnumOptions.ColorOnlyUsedOnce;

        public static void SetConfig(int maxTurns, int colorCount, int codeLength, EnumOptions enumOptions)
        {
            MaxTurns = maxTurns;
            ColorCount = colorCount;
            CodeLength = codeLength;
            CodeOptions = enumOptions;
        }

        public static void SetConfigToDefault()
        {
            SetConfig(MaxTurnsDefault, ColorCountDefault, CodeLengthDefault, EnumOptionsDefault);
        }

        // The colors are: Red, Green, Blue, Yellow, Orange, Purple, Pink, Brown

        public enum CodeColors { None, Yellow, Blue, Red, Green, Orange, Purple, Pink, Brown, White, Black, Beige};

        public enum HintColors { None, White, Black};

        [Flags]
        public enum EnumOptions {
            NoRestrictions = 0,  // colors are allowd to be used multiple times
            NoneIsAllowed = 1,  // none is allowed as a color during enumeration
            ColorOnlyUsedOnce = 2 // each color is allowed to be used only once
        };

        public static CodeColors[] GetColorValues(bool noneColor = false)
        {
            var colors = ((CodeColors[])Enum.GetValues(typeof(CodeColors))).ToArray();

            if (noneColor == false)
            {
                colors = colors.Where(c => c != CodeColors.None).ToArray();
            }

            if(colors.Length < GameConfig.ColorCount)
            {
                throw new ArgumentException("The number of available colors is less than the number of configured colors");
            }

            return colors.Take(ColorCount).ToArray();;
        }
    }
}
