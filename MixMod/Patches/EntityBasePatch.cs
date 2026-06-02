using HarmonyLib;

namespace MixMod.Patches
{
    [HarmonyPatch(typeof(EntityBase), nameof(EntityBase.GetPremiumType))]
    public static class EntityBase_GetPremiumType
    {
        public static bool Prefix(EntityBase __instance, ref TAG_PREMIUM __result)
        {
            var diamond = MixModConfig.Get().DIAMOND;
            var signature = MixModConfig.Get().SIGNATURE;
            var golden = MixModConfig.Get().GOLDEN;
            if (GameMgr.Get() != null && !GameMgr.Get().IsBattlegrounds() && GameState.Get() != null && GameState.Get().IsGameCreatedOrCreating())
            {
                if (__instance.HasTag(GAME_TAG.HAS_DIAMOND_QUALITY))
                {
                    if (diamond == CardState.All || diamond == CardState.OnlyMy && __instance.IsControlledByFriendlySidePlayer2())
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
                    if (signature == CardState.All || signature == CardState.OnlyMy && __instance.IsControlledByFriendlySidePlayer2())
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
                if (golden == CardState.All || (golden == CardState.OnlyMy && __instance.IsControlledByFriendlySidePlayer2()))
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

        public static bool IsControlledByFriendlySidePlayer2(this EntityBase _this)
        {
            var id = _this?.GetControllerId();
            var gameState = GameState.Get();

            if (!id.HasValue || gameState is null)
            {
                return false;
            }

            var player = gameState.GetPlayer(id.Value);
            return player?.IsFriendlySide() ?? false;
        }
    }
}
