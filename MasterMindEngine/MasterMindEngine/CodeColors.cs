namespace MasterMindEngine
{
    public enum CodeColors { None, Yellow, Blue, Red, Green};

    public enum HintColors { None, White, Black};

    [Flags]
    public enum EnumOptions {
        NoRestrictions = 0,  // colors are allowd to be used multiple times
        NoneIsAllowed = 1,  // none is allowed as a color during enumeration
        ColorOnlyIUsedOnce = 2 // each color is allowed to be used only once
    };
}
