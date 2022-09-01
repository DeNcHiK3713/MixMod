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
        
        private static void UpdateCurrentOpponent()
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

        public static void OnGameCreated(GameState.CreateGamePhase phase, object userData)
        {
            UpdateCurrentOpponent();
        }
    }

    [HarmonyPatch(typeof(Gameplay), "OnCreateGame")]
    public static class Gameplay_OnCreateGame
    {
        public static void Postfix(GameState.CreateGamePhase phase, object userData)
        {
            GameplayPatch.OnGameCreated(phase, userData);
        }
    }
}
