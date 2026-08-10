using Blizzard.T5.Core;
using HarmonyLib;
using System.Linq;
using System.Reflection;

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

    public static class EntityPatch
    {
        private static readonly MethodInfo handleEntityChangeInfo = typeof(Entity).GetMethod("HandleEntityChange", BindingFlags.Instance | BindingFlags.NonPublic);

        public static void HandleEntityChange(this Entity __instance, Network.Entity netEntity, Entity.LoadCardData data, bool fromShowEntity)
        {
            handleEntityChangeInfo?.Invoke(__instance, [netEntity, data, fromShowEntity]);
        }

        public static void UpdateAllEntities()
        {
            var gameState = GameState.Get();
            if (gameState is null)
            {
                return;
            }

            var entityMap = gameState.GetEntityMap();
            if (entityMap is null || !entityMap.Any())
            {
                return;
            }

            System.Collections.Generic.List<Network.Entity.Tag> tags = [];
            System.Collections.Generic.List<Network.Entity.TagList> tagLists = [];
            System.Collections.Generic.List<Network.Entity.Tag> defTags = [];

            var cardData = new Entity.LoadCardData
            {
                updateActor = true,
                restartStateSpells = true,
                fromChangeEntity = true
            };

            foreach (var entity in entityMap.Values)
            {
                var netEntity = new Network.Entity
                {
                    Tags = tags,
                    TagLists = tagLists,
                    CardID = entity.GetCardId(),
                    DefTags = defTags,
                };

                entity.HandleEntityChange(netEntity, cardData, false);
            }
        }
    }
}
