using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MixMod.Patches
{
    public static class PlayerLeaderboardManagerPatch
    {
        private static FieldInfo m_currentlyMousedOverTileInfo = typeof(PlayerLeaderboardManager).GetField("m_currentlyMousedOverTile", BindingFlags.NonPublic | BindingFlags.Instance);
        private static BnetPlayer m_currentOpponent;

        public static void UpdateCurrentOpponent(int opponentPlayerId)
        {
            if (GameState.Get() == null || !GameState.Get().GetPlayerInfoMap().ContainsKey(opponentPlayerId))
            {
                m_currentOpponent = null;
                return;
            }
            var id = GameState.Get().GetPlayerInfoMap()[opponentPlayerId].GetGameAccountId();
            if (id == null)
            {
                m_currentOpponent = null;
                return;
            }
            m_currentOpponent = BnetPresenceMgr.Get().GetPlayer(id);
        }

        public static BnetPlayer GetCurrentOpponent()
        {
            return m_currentOpponent;
        }

        public static BnetPlayer GetSelectedOpponent(this PlayerLeaderboardManager __instance)
        {
            if (m_currentlyMousedOverTileInfo?.GetValue(__instance) is not PlayerLeaderboardCard m_currentlyMousedOverTile || GameState.Get() == null)
            {
                return null;
            }
            int opponentPlayerId = m_currentlyMousedOverTile.Entity.GetTag(GAME_TAG.PLAYER_ID);
            if (!GameState.Get().GetPlayerInfoMap().ContainsKey(opponentPlayerId))
            {
                return null;
            }
            var id = GameState.Get().GetPlayerInfoMap()[opponentPlayerId].GetGameAccountId();
            if (id == null)
            {
                return null;
            }
            return BnetPresenceMgr.Get().GetPlayer(id);
        }
    }

    [HarmonyPatch(typeof(PlayerLeaderboardManager), nameof(PlayerLeaderboardManager.SetNextOpponent))]
    public static class PlayerLeaderboardManager_SetNextOpponent
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindIndex(x => x.opcode == OpCodes.Ret);
            if (index > 0)
            {
                index += 2;
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldarg_1));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Action<int>)PlayerLeaderboardManagerPatch.UpdateCurrentOpponent).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldarg_0));
            }
            return newInstructions;
        }
    }
    
    [HarmonyPatch(typeof(PlayerLeaderboardManager), nameof(PlayerLeaderboardManager.SetCurrentOpponent))]
    public static class PlayerLeaderboardManager_SetCurrentOpponent
    {
        public static void Postfix(int opponentPlayerId)
        {
            if (opponentPlayerId != -1)
            {
                PlayerLeaderboardManagerPatch.UpdateCurrentOpponent(opponentPlayerId);
            }
        }
    }
}
