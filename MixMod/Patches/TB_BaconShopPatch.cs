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
    [HarmonyPatchCategory("Gameplay_ShutUpBob")]
    [HarmonyPatch(typeof(TB_BaconShop), "HandleGameOverWithTiming", MethodType.Enumerator)]
    public static class TB_BaconShop_HandleGameOverWithTiming
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindIndex(x => x.opcode == OpCodes.Callvirt && (x.operand as MethodInfo).Name == "UpdateLayout");
            if (index > 0)
            {
                index++;
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldc_I4_0));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ret));
            }
            return newInstructions;
        }
    }

    [HarmonyPatchCategory("Gameplay_ShutUpBob")]
    [HarmonyPatch(typeof(TB_BaconShop), "PlayBobLineWithoutText", MethodType.Enumerator)]
    public static class TB_BaconShop_PlayBobLineWithoutText
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindIndex(x => x.opcode == OpCodes.Stfld && (x.operand as FieldInfo).Name == "<>1__state");
            if (index > 0)
            {
                index++;
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldc_I4_0));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ret));
            }
            return newInstructions;
        }
    }
}
