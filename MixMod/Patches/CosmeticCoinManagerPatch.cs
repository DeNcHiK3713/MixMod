using HarmonyLib;
using Mono.Cecil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MixMod.Patches
{
    [HarmonyPatch(typeof(CosmeticCoinManager), "InitCoinDataWhenReady", MethodType.Enumerator)]
    public static class CosmeticCoinManager_InitCoinDataWhenReady
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindLastIndex(x => x.opcode == OpCodes.Stfld);
            var tag = generator.DeclareLocal(typeof(TAG_PREMIUM));
            if (index > 0)
            {
                var l1 = generator.DefineLabel();
                var l2 = generator.DefineLabel();
                var l3 = generator.DefineLabel();
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("DevEnabled", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Brfalse_S, l1));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("GoldenCoin", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldc_I4_1));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Beq_S, l2));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("GoldenCoin", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldc_I4_2));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Beq_S, l2));
                newInstructions.Insert(index, new CodeInstruction(OpCodes.Ldc_I4_0));
                newInstructions[index++].labels.Add(l1);
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Br_S, l3));
                newInstructions.Insert(index, new CodeInstruction(OpCodes.Ldc_I4_1));
                newInstructions[index++].labels.Add(l2);
                newInstructions.Insert(index, new CodeInstruction(OpCodes.Stloc_S, tag));
                newInstructions[index].labels.Add(l3);
            }
            index = newInstructions.FindLastIndex(x => x.opcode == OpCodes.Newobj && (x.operand as ConstructorInfo).Name == "CollectibleCard");
            if (index > 0)
            {
                index--;
                newInstructions[index].opcode = OpCodes.Ldloc_S;
                newInstructions[index].operand = tag;
            }
            return newInstructions;
        }
    }

    [HarmonyPatch]
    public static class CosmeticCoinManager_ShowCoinPreviewDelegate
    {
        public static MethodBase TargetMethod()
        {
            var method = typeof(CosmeticCoinManager).GetMethod(nameof(CosmeticCoinManager.ShowCoinPreview), BindingFlags.Instance | BindingFlags.Public);
            var instructions = method.ToDefinition().Body.Instructions;
            var targetMethod = instructions.Last(x => x.OpCode == Mono.Cecil.Cil.OpCodes.Ldftn).Operand as MethodDefinition;
            var methodInfo = typeof(CosmeticCoinManager).Module.ResolveMethod(targetMethod.MetadataToken.ToInt32());
            return methodInfo;
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindLastIndex(x => x.opcode == OpCodes.Ldc_I4_0);
            if (index > 0)
            {
                var tag = generator.DeclareLocal(typeof(TAG_PREMIUM));
                newInstructions[index].opcode = OpCodes.Ldloc_S;
                newInstructions[index].operand = tag;
                index = 0;
                var l1 = generator.DefineLabel();
                var l2 = generator.DefineLabel();
                var l3 = generator.DefineLabel();
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("DevEnabled", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Brfalse_S, l1));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("GoldenCoin", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldc_I4_1));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Beq_S, l2));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("GoldenCoin", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldc_I4_2));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Beq_S, l2));
                newInstructions.Insert(index, new CodeInstruction(OpCodes.Ldc_I4_0));
                newInstructions[index++].labels.Add(l1);
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Br_S, l3));
                newInstructions.Insert(index, new CodeInstruction(OpCodes.Ldc_I4_1));
                newInstructions[index++].labels.Add(l2);
                newInstructions.Insert(index, new CodeInstruction(OpCodes.Stloc_S, tag));
                newInstructions[index].labels.Add(l3);
            }
            return newInstructions;
        }
    }
}
