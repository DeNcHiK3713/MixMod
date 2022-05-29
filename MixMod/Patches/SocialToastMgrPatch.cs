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
    [HarmonyPatch(typeof(SocialToastMgr), "AddToast", new[] { typeof(UserAttentionBlocker), typeof(string), typeof(SocialToastMgr.TOAST_TYPE), typeof(float), typeof(bool) })]
    public static class SocialToastMgr_AddToast
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindIndex(x => x.opcode == OpCodes.Ret);
            if (index > 0)
            {
                index++;
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldarg_3));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, typeof(Time).GetProperty("timeScale", BindingFlags.Static | BindingFlags.Public).GetGetMethod()));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Mul));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Starg_S, (byte)3));
            }
            return newInstructions;
        }
    }
}
