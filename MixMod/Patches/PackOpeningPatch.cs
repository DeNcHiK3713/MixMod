using HarmonyLib;

namespace MixMod.Patches
{
    [HarmonyPatchCategory("Others_DisableMassPackOpening")]
    [HarmonyPatch(typeof(PackOpening), nameof(PackOpening.MassPackOpeningPackLimit))]
    public static class MassPackOpeningPackLimit
    {
        public static bool Prefix(ref int __result)
        {
            __result = int.MaxValue;
            return false;
        }
    }
}