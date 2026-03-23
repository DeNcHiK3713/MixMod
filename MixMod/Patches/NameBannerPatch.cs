using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MixMod.Patches
{
    [HarmonyPatchCategory("Gameplay_ShowOpponentRankInGame")]
    [HarmonyPatch(typeof(NameBanner), nameof(NameBanner.Initialize))]
    public static class NameBanner_Initialize
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindIndex(x => x.opcode == OpCodes.Call && (x.operand as MethodInfo).Name == "IsGameTypeRanked");
            if (index > 0)
            {
                newInstructions[index] = new CodeInstruction(OpCodes.Ldc_I4_1);
            }
            return newInstructions;
        }
    }

    [HarmonyPatchCategory("Gameplay_ShowOpponentRankInGame")]
    [HarmonyPatch(typeof(NameBanner), "UpdateMedalWhenReady", MethodType.Enumerator)]
    public static class NameBanner_UpdateMedalWhenReady
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindIndex(x => x.opcode == OpCodes.Callvirt && (x.operand as MethodInfo).Name == "get_ShowOpponentRankInGame");
            if (index > 0)
            {
                index -= 5;
                newInstructions.RemoveRange(index, 6);
                newInstructions[index].opcode = OpCodes.Br_S;
            }
            return newInstructions;
        }
    }
}
