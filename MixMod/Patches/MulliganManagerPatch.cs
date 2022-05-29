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
    [HarmonyPatch(typeof(MulliganManager), "HandleGameStart")]
    public static class MulliganManager_HandleGameStart
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindLastIndex(x => x.opcode == OpCodes.Callvirt && (x.operand as MethodInfo).Name == "ShouldSkipMulligan");
            if (index > 0)
            {
                index++;
                var l1 = newInstructions[index].operand;
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Brtrue_S, l1));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("SkipHeroIntro", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
            }
            return newInstructions;
        }
    }
}
