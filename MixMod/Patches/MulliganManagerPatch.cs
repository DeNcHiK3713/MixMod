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
    [HarmonyPatchCategory("Gameplay_SkipHeroIntro")]
    [HarmonyPatch(typeof(MulliganManager), "HandleGameStart")]
    public static class MulliganManager_HandleGameStart
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindLastIndex(x => x.opcode == OpCodes.Callvirt && (x.operand as MethodInfo).Name == "ShouldSkipMulligan");
            if (index > 0)
            {
                index--;
                newInstructions.RemoveAt(index);
                newInstructions[index] = new CodeInstruction(OpCodes.Ldc_I4_1);
            }
            return newInstructions;
        }
    }
}
