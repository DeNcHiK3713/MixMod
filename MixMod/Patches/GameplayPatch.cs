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
    public static class GameplayPatch
    {
        private static BnetPlayer m_currentOpponent;

        public static BnetPlayer GetCurrentOpponent()
        {
            return m_currentOpponent;
        }

        public static void UpdateCurrentOpponent()
        {
            if (GameState.Get() == null)
            {
                m_currentOpponent = null;
                return;
            }
            Player opposingSidePlayer = GameState.Get().GetOpposingSidePlayer();
            if (opposingSidePlayer == null)
            {
                m_currentOpponent = null;
                return;
            }
            m_currentOpponent = BnetPresenceMgr.Get().GetPlayer(opposingSidePlayer.GetGameAccountId());
        }
    }

    [HarmonyPatchCategory("Default")]
    [HarmonyPatch(typeof(Gameplay), "OnCreateGame")]
    public static class Gameplay_OnCreateGame
    {
        public static void Postfix()
        {
            GameplayPatch.UpdateCurrentOpponent();
        }
    }
}
