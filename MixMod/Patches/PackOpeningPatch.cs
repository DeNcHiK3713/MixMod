using HarmonyLib;

namespace MixMod.Patches
{
    [HarmonyPatch(typeof(PackOpening), nameof(PackOpening.MassPackOpeningPackLimit))]
    public static class MassPackOpeningPackLimit
    {
        public static bool Prefix(ref int __result)
        {
            if (MixModConfig.Get().DisableMassPackOpening)
            {
                __result = int.MaxValue;
                return false;
            }
            return true;
        }
    }
}