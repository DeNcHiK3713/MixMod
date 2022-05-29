using Game.PackOpening;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MixMod.Patches
{
    public static class PackOpeningDirectorPatch
    {
        public static bool m_WaitingForCards;
        public static bool m_WaitingForAllCardsRevealed;
        private static FieldInfo m_hiddenCardsInfo = typeof(PackOpeningDirector).GetField("m_hiddenCards", BindingFlags.NonPublic | BindingFlags.Instance);

        public static IEnumerator ForceRevealAllCards(this PackOpeningDirector __instance)
        {
            if (m_hiddenCardsInfo.GetValue(__instance) is HiddenCards m_hiddenCards)
            {
                m_WaitingForAllCardsRevealed = true;
                while (m_WaitingForCards)
                {
                    yield return new WaitForSeconds(0.05f);
                }
                m_hiddenCards.ForceRevealAllCards();
                m_WaitingForAllCardsRevealed = false;
                yield break;
            }
        }
    }

    [HarmonyPatch(typeof(PackOpeningDirector), "Awake")]
    public static class PackOpeningDirector_Awake
    {
        public static void Postfix()
        {
            PackOpeningDirectorPatch.m_WaitingForCards = true;
        }
    }
    
    [HarmonyPatch(typeof(PackOpeningDirector), "OnSpellFinished")]
    public static class PackOpeningDirector_OnSpellFinished
    {
        public static void Postfix()
        {
            PackOpeningDirectorPatch.m_WaitingForCards = false;
        }
    }
}
