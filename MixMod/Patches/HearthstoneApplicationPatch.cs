using BepInEx.Configuration;
using HarmonyLib;
using Hearthstone;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MixMod.Patches
{
    [HarmonyPatchCategory("Dev_IsInternal")]
    [HarmonyPatch(typeof(HearthstoneApplication), nameof(HearthstoneApplication.GetMode))]
    public static class HearthstoneApplication_GetMode
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindLastIndex(x => x.opcode == OpCodes.Brtrue);
            if (index > 0)
            {
                index++;
                if (newInstructions[index].opcode == OpCodes.Ldc_I4_2)
                {
                    newInstructions[index].opcode = OpCodes.Ldc_I4_1;
                }

            }
            return newInstructions;
        }
    }

    [HarmonyPatchCategory("Dev_IsInternal")]
    [HarmonyPatch(typeof(HearthstoneApplication), "Job_InitializeMode", MethodType.Enumerator)]
    public static class HearthstoneApplication_Job_InitializeMode
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindLastIndex(x => x.opcode == OpCodes.Stsfld);
            if (index > 0)
            {
                index--;
                if (newInstructions[index].opcode == OpCodes.Ldc_I4_2)
                {
                    newInstructions[index].opcode = OpCodes.Ldc_I4_1;
                }
            }
            return newInstructions;
        }
    }
}
