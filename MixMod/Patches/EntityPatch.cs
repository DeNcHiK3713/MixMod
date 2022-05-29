using HarmonyLib;

namespace MixMod.Patches
{
    [HarmonyPatch(typeof(Entity), nameof(Entity.GetPremiumType))]
    public static class Entity_GetPremiumType
    {
        public static bool DoesDiamondModelExistOnCardDef(this Entity __instance)
        {
            using (DefLoader.DisposableCardDef disposableCardDef = __instance.ShareDisposableCardDef())
            {
                return disposableCardDef is object && disposableCardDef.CardDef is object && !string.IsNullOrEmpty(disposableCardDef.CardDef.m_DiamondModel);
            }
        }
        
        public static bool Prefix(Entity __instance, ref TAG_PREMIUM __result)
        {
            var diamond = MixModConfig.Get().DIAMOND;
            var golden = MixModConfig.Get().GOLDEN;
            if (GameMgr.Get() != null && !GameMgr.Get().IsBattlegrounds() && GameState.Get() != null && GameState.Get().IsGameCreatedOrCreating())
            {
                if (__instance.DoesDiamondModelExistOnCardDef())
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
