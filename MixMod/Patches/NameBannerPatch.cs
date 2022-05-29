using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MixMod.Patches
{
    [HarmonyPatch(typeof(NameBanner), nameof(NameBanner.Initialize))]
    public static class NameBanner_Initialize
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindIndex(x => x.opcode == OpCodes.Call && (x.operand as MethodInfo).Name == "IsGameTypeRanked");
            if (index > 0)
            {
                index++;
                var l1 = generator.DefineLabel();
                newInstructions[index].labels.Add(l1);
                var l2 = generator.DefineLabel();
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Brtrue_S, l2));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("ShowOpponentRankInGame", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Br_S, l1));
                newInstructions.Insert(index, new CodeInstruction(OpCodes.Ldc_I4_1));
                newInstructions[index].labels.Add(l2);
            }
            return newInstructions;
        }
    }

    [HarmonyPatch(typeof(NameBanner), "UpdateMedalWhenReady", MethodType.Enumerator)]
    public static class NameBanner_UpdateMedalWhenReady
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindIndex(x => x.opcode == OpCodes.Callvirt && (x.operand as MethodInfo).Name == "get_ShowOpponentRankInGame");
            if (index > 0)
            {
                index++;
                var l1 = newInstructions[index].operand;
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Brtrue_S, l1));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                newInstructions.Insert(index, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("ShowOpponentRankInGame", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
            }
            return newInstructions;
        }
    }
}
