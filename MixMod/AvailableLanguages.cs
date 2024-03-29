﻿using MixMod.Properties;
using System.ComponentModel;

namespace MixMod
{
    public enum AvailableLanguages
    {
        [LocalizedDescription(typeof(MixModLocalization), "AvailableLanguages.Default")]
        Default,
        [Description("English")]
        enUS,
        [Description("Русский")]
        ruRU,
        [Description("中文(简体)")]
        zhCN
    }
}
