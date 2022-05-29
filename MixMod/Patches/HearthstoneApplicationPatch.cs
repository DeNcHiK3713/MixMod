using BepInEx.Configuration;
using HarmonyLib;
using Hearthstone;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MixMod.Patches
{
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
                var l1 = generator.DefineLabel();
                newInstructions[index].labels.Add(l1);
                if (newInstructions[index].opcode == OpCodes.Ldc_I4_2)
                {
                    newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                    newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("DevEnabled", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                    newInstructions.Insert(index++, new CodeInstruction(OpCodes.Brfalse_S, l1));
                    newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                    newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("IsInternal", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                    Label l3 = generator.DefineLabel();
                    newInstructions.Insert(index, new CodeInstruction(OpCodes.Brtrue_S, l3));
                    index += 3;
                    newInstructions.Insert(index, new CodeInstruction(OpCodes.Ldc_I4_1));
                    newInstructions[index++].labels.Add(l3);
                    newInstructions.Insert(index, new CodeInstruction(OpCodes.Ret));
                }

            }
            return newInstructions;
        }
    }

    [HarmonyPatch(typeof(HearthstoneApplication), "Job_InitializeMode", MethodType.Enumerator)]
    public static class HearthstoneApplication_Job_InitializeMode
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindLastIndex(x => x.opcode == OpCodes.Brfalse);
            if (index > 0 && newInstructions[index].operand is Label l1)
            {
                Label l2 = generator.DefineLabel();
                newInstructions[index].operand = l2;
                index += 3;
                if (newInstructions[index].opcode == OpCodes.Ldc_I4_2)
                {
                    newInstructions.Insert(index, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                    newInstructions[index++].labels.Add(l2);
                    newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("DevEnabled", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                    newInstructions.Insert(index++, new CodeInstruction(OpCodes.Brfalse_S, l1));
                    newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                    newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("IsInternal", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                    Label l3 = generator.DefineLabel();
                    newInstructions.Insert(index, new CodeInstruction(OpCodes.Brtrue_S, l3));
                    index += 2;
                    Label l4 = generator.DefineLabel();
                    newInstructions.Insert(index++, new CodeInstruction(OpCodes.Br_S, l4));
                    newInstructions.Insert(index, new CodeInstruction(OpCodes.Ldc_I4_1));
                    newInstructions[index++].labels.Add(l3);
                    newInstructions[index].labels.Add(l4);
                }

            }
            return newInstructions;
        }
    }
}
