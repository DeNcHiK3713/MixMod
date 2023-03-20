using Blizzard.T5.Core.Time;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace MixMod.Patches
{
    public static class TimeScaleMgrPatch
    {
        private static MethodInfo updateInfo = typeof(TimeScaleMgr).GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic);
        
        public static void Update(this TimeScaleMgr __instance)
        {
            updateInfo?.Invoke(__instance, null);
        }
    }
    
    [HarmonyPatch(typeof(TimeScaleMgr), "Update")]
    public static class TimeScaleMgr_Update
    {
        public static bool Prefix(float ___m_timeScaleMultiplier, float ___m_gameTimeScale)
        {
            if (!MixModConfig.Get().TimeScaleEnabled || (MixModConfig.Get().TimeScaleInGameOnly && !GameMgrPatch.GameStarted))
            {
                return true;
            }

            float timeScale = MixModConfig.Get().TimeScale;
            Time.timeScale = timeScale > ___m_timeScaleMultiplier
                ? (timeScale + (___m_timeScaleMultiplier - 1f) * 0.5f) * ___m_gameTimeScale
                : (___m_timeScaleMultiplier + (timeScale - 1f) * 0.5f) * ___m_gameTimeScale;
            return false;
        }
    }
}
