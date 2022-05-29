using HarmonyLib;

namespace MixMod.Patches
{
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
}
