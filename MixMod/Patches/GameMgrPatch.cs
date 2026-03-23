using Blizzard.T5.Core.Time;
using HarmonyLib;

namespace MixMod.Patches
{
    [HarmonyPatchCategory("Dev_Board")]
    [HarmonyPatch(typeof(GameMgr), "ChangeBoardIfNecessary")]
    public static class GameMgr_ChangeBoardIfNecessary
    {
        public static bool Prefix(Network.GameSetup ___m_gameSetup)
        {
            int board = MixModConfig.Get().Board;
            if (MixModConfig.Get().DevEnabled && board != 0)
            {
                ___m_gameSetup.Board = board;
                return false;
            }
            return true;
        }
    }

    public static class GameMgrPatch
    {
        public static bool GameStarted { get; set; }
    }

    [HarmonyPatchCategory("Global_TimeScaleInGameOnly")]
    [HarmonyPatch(typeof(GameMgr), "OnGameSetup")]
    public static class GameMgr_OnGameSetup
    {
        public static void Postfix()
        {
            GameMgrPatch.GameStarted = true;
            TimeScaleMgr.Get().Update();
        }
    }

    [HarmonyPatchCategory("Global_TimeScaleInGameOnly")]
    [HarmonyPatch(typeof(GameMgr), "OnGameCanceled")]
    [HarmonyPatch(typeof(GameMgr), "OnGameEnded")]
    public static class GameMgr_OnGameEnded
    {
        public static void Postfix()
        {
            GameMgrPatch.GameStarted = false;
            TimeScaleMgr.Get().Update();
        }
    }
}
