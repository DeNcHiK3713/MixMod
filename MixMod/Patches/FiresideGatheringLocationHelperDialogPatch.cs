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
    [HarmonyPatch(typeof(FiresideGatheringLocationHelperDialog), "Start")]
    public static class FiresideGatheringLocationHelperDialog_Start
    {
        private static MethodInfo ChangeStateInfo = typeof(FiresideGatheringLocationHelperDialog).GetMethod("ChangeState", BindingFlags.Instance | BindingFlags.NonPublic);

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindLastIndex(x => x.opcode == OpCodes.Ldfld && (x.operand as FieldInfo).Name == "m_isCheckInFailure");
            if (index > 0)
            {
                index--;
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("FiresideGathering", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                var l1 = generator.DefineLabel();
                newInstructions[index].labels.Add(l1);
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Brfalse_S, l1));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldarg_0));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldc_I4_3));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ChangeStateInfo));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ret));
            }
            return newInstructions;
        }
    }
}
