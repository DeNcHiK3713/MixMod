using HarmonyLib;

namespace MixMod.Patches
{
    [HarmonyPatchCategory("TAG_PREMIUM")]
    [HarmonyPatch(typeof(Entity), nameof(Entity.LoadCard))]
    public static class Entity_LoadCard
    {        
        public static bool Prefix(Entity __instance)
        {
            __instance.SetRealTimePremium(__instance.GetPremiumType());
            return true;
        }
    }
}
