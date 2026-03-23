using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MixMod.Patches
{
    [HarmonyPatchCategory("Gameplay_DisableThinkEmotes")]
    [HarmonyPatch(typeof(ThinkEmoteManager), "Update")]
    public static class ThinkEmoteManagerPatch
    {
        public static bool Prefix()
        {
            return false;
        }
    }
}
