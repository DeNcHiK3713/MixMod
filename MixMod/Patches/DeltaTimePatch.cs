using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace MixMod.Patches
{
    [HarmonyPatch]
    public static class UpdatePatch
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            yield return typeof(MatchingQueueTab).GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic);
            yield return typeof(ThinkEmoteManager).GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic);
        }
        
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);
            var deltaTimeInfo = typeof(Time).GetProperty("deltaTime", BindingFlags.Static | BindingFlags.Public).GetGetMethod();
            var unscaledDeltaTimeInfo = typeof(Time).GetProperty("unscaledDeltaTime", BindingFlags.Static | BindingFlags.Public).GetGetMethod();

            int index = newInstructions.FindIndex(x => x.opcode == OpCodes.Call && (x.operand as MethodInfo) == deltaTimeInfo);
            if (index > 0)
            {
                newInstructions[index].operand = unscaledDeltaTimeInfo;
            }
            return newInstructions;
        }
    }
}
