using HarmonyLib;

namespace MixMod.Patches
{
    [HarmonyPatch(typeof(Actor), nameof(Actor.GetPremium))]
    public static class Actor_GetPremium
    {
        public static bool Prefix(Actor __instance, Entity ___m_entity, ref TAG_PREMIUM __result)
        {
            var diamond = MixModConfig.Get().DIAMOND;
            var signature = MixModConfig.Get().SIGNATURE;
            var golden = MixModConfig.Get().GOLDEN;
            if (GameMgr.Get() != null && !GameMgr.Get().IsBattlegrounds() && GameState.Get() != null && GameState.Get().IsGameCreatedOrCreating())
            {
                if (__instance.DoesDiamondModelExistOnCardDef())
                {
                    if (diamond == CardState.All || diamond == CardState.OnlyMy && ___m_entity.IsControlledByFriendlySidePlayer2())
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
                if (__instance.HasSignaturePortraitTexture())
                {
                    if (signature == CardState.All || signature == CardState.OnlyMy && ___m_entity.IsControlledByFriendlySidePlayer2())
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
                if (golden == CardState.All || (golden == CardState.OnlyMy && ___m_entity.IsControlledByFriendlySidePlayer2()))
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