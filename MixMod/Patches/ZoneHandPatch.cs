using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MixMod.Patches
{
    [HarmonyPatch(typeof(ZoneHand), nameof(ZoneHand.GetCardPosition), new[] { typeof(int), typeof(int) })]
    public static class ZoneHand_GetCardPosition
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindLastIndex(x => x.opcode == OpCodes.Callvirt && (x.operand as MethodInfo).Name == "IsRevealed");
            if (index > 0)
            {
                index++;
                var l1 = newInstructions[index++].operand;
                var l2 = generator.DefineLabel();
                var l3 = generator.DefineLabel();
                var l4 = generator.DefineLabel();
                newInstructions[index].labels.Add(l2);
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("MoveEnemyCards", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Brfalse_S, l2));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldarg_0));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldflda, typeof(ZoneHand).GetField("m_centerOfHand", BindingFlags.NonPublic | BindingFlags.Instance)));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldfld, typeof(Vector3).GetField("z", BindingFlags.Public | BindingFlags.Instance)));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldarg_1));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldloc_0));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldc_I4_2));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Div));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Sub));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<int, int>)Mathf.Abs).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Conv_R4));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldc_R4, 2f));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<float, float, float>)Mathf.Pow).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldc_I4_4));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldloc_0));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Mul));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Conv_R4));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Div));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldloc_1));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Brtrue_S, l3));
                newInstructions.Insert(index, new CodeInstruction(OpCodes.Ldc_R4, 0f));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Br_S, l4));
                newInstructions.Insert(index, new CodeInstruction(OpCodes.Ldc_R4, 1f));
                newInstructions[index++].labels.Add(l3);
                newInstructions.Insert(index, new CodeInstruction(OpCodes.Mul));
                newInstructions[index++].labels.Add(l4);
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Sub));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldloc_S, (byte)6));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Sub));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldc_R4, 0.6f));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Sub));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Stloc_S, (byte)5));
                newInstructions.Insert(index, new CodeInstruction(OpCodes.Br_S, l1));
            }
            return newInstructions;
        }
    }

    [HarmonyPatch(typeof(ZoneHand), nameof(ZoneHand.GetCardRotation), new[] { typeof(int), typeof(int) })]
    public static class ZoneHand_GetCardRotation
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindLastIndex(x => x.opcode == OpCodes.Callvirt && (x.operand as MethodInfo).Name == "IsRevealed");
            if (index > 0)
            {
                index++;
                var l1 = newInstructions[index++].operand;
                var l2 = generator.DefineLabel();
                newInstructions[index].labels.Add(l2);
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("MoveEnemyCards", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Brfalse_S, l2));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldloc_0));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldarg_1));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Conv_R4));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Mul));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldloc_1));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Add));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Stloc_3));
                newInstructions.Insert(index, new CodeInstruction(OpCodes.Br_S, l1));
            }
            return newInstructions;
        }
    }
    
    [HarmonyPatch(typeof(ZoneHand), nameof(ZoneHand.GetCardScale))]
    public static class ZoneHand_GetCardScale
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindIndex(x => x.opcode == OpCodes.Ldfld && (x.operand as FieldInfo).Name == "enemyHand");
            if (index > 0)
            {
                index++;
                var l1 = newInstructions[index++].operand;
                var l2 = generator.DefineLabel();
                newInstructions[index].labels.Add(l2);
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("MoveEnemyCards", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Brfalse_S, l2));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldarg_0));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldfld, typeof(ZoneHand).GetField("m_controller", BindingFlags.NonPublic | BindingFlags.Instance)));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(Player).GetMethod("IsRevealed", BindingFlags.Public | BindingFlags.Instance)));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Brfalse_S, l2));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldc_R4, 0.41f));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldc_R4, 0.085f));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldc_R4, 0.41f));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Newobj,
                        typeof(Vector3).GetConstructor(BindingFlags.Instance | BindingFlags.Public,
                            null, new[] { typeof(float), typeof(float), typeof(float) }, null)));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ret));
                newInstructions.Insert(index, new CodeInstruction(OpCodes.Br_S, l1));
            }
            return newInstructions;
        }
    }
}
