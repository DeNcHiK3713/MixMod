using MixMod.Properties;
using System.ComponentModel;

namespace MixMod
{
    public enum CardState
    {
        [LocalizedDescription(typeof(MixModLocalization), "CardState.Default")]
        Default,
        [LocalizedDescription(typeof(MixModLocalization), "CardState.OnlyMy")]
        OnlyMy,
        [LocalizedDescription(typeof(MixModLocalization), "CardState.All")]
        All,
        [LocalizedDescription(typeof(MixModLocalization), "CardState.Disabled")]
        Disabled
    }
}
