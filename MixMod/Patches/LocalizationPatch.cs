using HarmonyLib;
using MixMod.Properties;

namespace MixMod.Patches
{
    [HarmonyPatch(typeof(Localization), "SetPegLocaleName")]
    public static class Localization_SetPegLocaleName
    {
        public static void Postfix()
        {
            if (MixModConfig.Get().Language == AvailableLanguages.Default)
            {
                MixModConfig.Get().ReloadLocalization();
            }
        }
    }
}