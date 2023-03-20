using HarmonyLib;

namespace MixMod.Patches
{
    [HarmonyPatch(typeof(Entity), nameof(Entity.LoadCard))]
    public static class Entity_LoadCard
    {        
        public static bool Prefix(Entity __instance)
        {
            __instance.SetRealTimePremium(__instance.GetPremiumType());
            return true;
        }
    }

    [HarmonyPatch(typeof(Entity), nameof(Entity.GetPremiumType))]
    public static class Entity_GetPremiumType
    {        
        public static bool Prefix(Entity __instance, ref TAG_PREMIUM __result)
        {
            var diamond = MixModConfig.Get().DIAMOND;
            var signature = MixModConfig.Get().SIGNATURE;
            var golden = MixModConfig.Get().GOLDEN;
            if (GameMgr.Get() != null && !GameMgr.Get().IsBattlegrounds() && GameState.Get() != null && GameState.Get().IsGameCreatedOrCreating())
            {
                if (__instance.HasTag(GAME_TAG.HAS_DIAMOND_QUALITY))
                {
                    if (diamond == CardState.All || diamond == CardState.OnlyMy && __instance.IsControlledByFriendlySidePlayer())
                    {
                        __result = TAG_PREMIUM.DIAMOND;
                        return false;
                    }
                    if (diamond == CardState.Disabled)
                    {
                        __result = TAG_PREMIUM.NORMAL;
                        return false;
                    }
                }
                if (__instance.HasTag(GAME_TAG.HAS_SIGNATURE_QUALITY))
                {
                    if (signature == CardState.All || signature == CardState.OnlyMy && __instance.IsControlledByFriendlySidePlayer())
                    {
                        __result = TAG_PREMIUM.SIGNATURE;
                        return false;
                    }
                    if (signature == CardState.Disabled)
                    {
                        __result = TAG_PREMIUM.NORMAL;
                        return false;
                    }
                }
                if (golden == CardState.All || (golden == CardState.OnlyMy && __instance.IsControlledByFriendlySidePlayer()))
                {
                    __result = TAG_PREMIUM.GOLDEN;
                    return false;
                }
                if (golden == CardState.Disabled)
                {
                    __result = TAG_PREMIUM.NORMAL;
                    return false;
                }
            }
            return true;
        }
    }
}
