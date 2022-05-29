using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MixMod.Patches
{
    public static class SoundManagerPatch
    {
        public static bool MuteKeyPressed = false;
        private static MethodInfo updateAppMuteInfo = typeof(SoundManager).GetMethod("UpdateAppMute", BindingFlags.Instance | BindingFlags.NonPublic);
        public static void OnMuteKeyPressed()
        {
            MuteKeyPressed = !MuteKeyPressed;
            SoundManager instance = SoundManager.Get();
            if (instance != null)
            {
                updateAppMuteInfo?.Invoke(instance, null);
            }
        }
    }

    [HarmonyPatch(typeof(SoundManager), "UpdateAllMutes")]
    public static class SoundManager_UpdateAllMutes
    {
        public static bool Prefix(ref bool ___m_mute)
        {
            ___m_mute = SoundManagerPatch.MuteKeyPressed || ___m_mute;
            return true;
        }
    }
}
