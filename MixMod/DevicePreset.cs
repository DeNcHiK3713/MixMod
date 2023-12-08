using MixMod.Properties;
using System.ComponentModel;

namespace MixMod
{
    public enum DevicePreset
    {
        [LocalizedDescription(typeof(MixModLocalization), "DevicePreset.Default")]
        Default,
        [LocalizedDescription(typeof(MixModLocalization), "DevicePreset.iPad")]
        iPad,
        [LocalizedDescription(typeof(MixModLocalization), "DevicePreset.iPhone")]
        iPhone,
        [LocalizedDescription(typeof(MixModLocalization), "DevicePreset.Phone")]
        Phone,
        [LocalizedDescription(typeof(MixModLocalization), "DevicePreset.Tablet")]
        Tablet,
        [LocalizedDescription(typeof(MixModLocalization), "DevicePreset.HuaweiPhone")]
        HuaweiPhone,
        [LocalizedDescription(typeof(MixModLocalization), "DevicePreset.Mac")]
        Mac,
        [LocalizedDescription(typeof(MixModLocalization), "DevicePreset.Custom")]
        Custom
    }
}
