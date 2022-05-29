using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace MixMod.Patches
{
    [HarmonyPatch(typeof(PackOpening), "AutomaticallyOpenPack")]
    public static class PackOpening_AutomaticallyOpenPack
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindLastIndex(x => x.opcode == OpCodes.Ret);
            if (index > 0)
            {
                var l1 = generator.DefineLabel();
                newInstructions[index].labels.Add(l1);
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("QuickPackOpeningEnabled", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Brfalse_S, l1));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldarg_0));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldarg_0));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldfld, typeof(PackOpening).GetField("m_director", BindingFlags.NonPublic | BindingFlags.Instance)));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, ((Func<PackOpeningDirector, IEnumerator>)PackOpeningDirectorPatch.ForceRevealAllCards).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call,
                    typeof(MonoBehaviour).GetMethod("StartCoroutine", BindingFlags.Public | BindingFlags.Instance, null,
                        new[] { typeof(IEnumerator) }, null)));
                newInstructions.Insert(index, new CodeInstruction(OpCodes.Pop));
            }
            return newInstructions;
        }
    }
    
    [HarmonyPatch(typeof(PackOpening), nameof(PackOpening.HandleKeyboardInput))]
    public static class PackOpening_HandleKeyboardInput
    {

        public static bool Prefix(ref bool __result)
        {
            if (PackOpeningDirectorPatch.m_WaitingForAllCardsRevealed)
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
