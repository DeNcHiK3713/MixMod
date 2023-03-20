using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace MixMod.Patches
{
    [HarmonyPatch]
    public static class GetPremiumPatch
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            yield return typeof(Card).GetMethod("GetPremium", BindingFlags.Instance | BindingFlags.Public);
            yield return typeof(CollectionDraggableCardVisual).GetMethod("GetPremium", BindingFlags.Instance | BindingFlags.Public);
            yield return typeof(HeroPickerButton).GetMethod("GetPremium", BindingFlags.Instance | BindingFlags.Public);
            yield return typeof(SpawnToDeckSpell).GetMethod("GetPremium", BindingFlags.Instance | BindingFlags.NonPublic);
        }
        
        public static bool Prefix(ref TAG_PREMIUM __result)
        {
            if (MixModConfig.Get().DevEnabled)
            {
                var golden = MixModConfig.Get().GoldenCoin;
                if (GameMgr.Get() != null && !GameMgr.Get().IsBattlegrounds() && GameState.Get() != null && GameState.Get().IsGameCreatedOrCreating())
                {
                    if (golden == CardState.All || golden == CardState.OnlyMy)
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
            }
            return true;
        }
    }
}
