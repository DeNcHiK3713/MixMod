using System.ComponentModel;

namespace MixMod
{
    public enum DevicePreset
    {
        [Description("Стандартные настройки")]
        Default,
        [Description("Настройки для iPad")]
        iPad,
        [Description("Настройки для iPhone")]
        iPhone,
        [Description("Настройки для телефона Android")]
        Phone,
        [Description("Настройки для планшета Android")]
        Tablet,
        [Description("Настройки для телефона Huawei")]
        HuaweiPhone,
        [Description("Свои настройки")]
        Custom
    }
}
