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
    [HarmonyPatch(typeof(TB_BaconShop), "HandleGameOverWithTiming", MethodType.Enumerator)]
    public static class TB_BaconShop_HandleGameOverWithTiming
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindIndex(x => x.opcode == OpCodes.Callvirt && (x.operand as MethodInfo).Name == "UpdateLayout");
            if (index > 0)
            {
                index++;
                var l1 = generator.DefineLabel();
                newInstructions[index].labels.Add(l1);
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("ShutUpBob", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Brfalse_S, l1));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldc_I4_0));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ret));
            }
            return newInstructions;
        }
    }

    [HarmonyPatch(typeof(TB_BaconShop), "PlayBobLineWithoutText", MethodType.Enumerator)]
    public static class TB_BaconShop_PlayBobLineWithoutText
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindIndex(x => x.opcode == OpCodes.Stfld && (x.operand as FieldInfo).Name == "<>1__state");
            if (index > 0)
            {
                index++;
                var l1 = generator.DefineLabel();
                newInstructions[index].labels.Add(l1);
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("ShutUpBob", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Brfalse_S, l1));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldc_I4_0));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ret));
            }
            return newInstructions;
        }
    }
}
