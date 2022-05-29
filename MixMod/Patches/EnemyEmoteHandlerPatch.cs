using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MixMod.Patches
{
    public static class EnemyEmoteHandlerPatch
    {
        private static MethodInfo doSquelchClickInfo = typeof(EnemyEmoteHandler).GetMethod("DoSquelchClick", BindingFlags.Instance | BindingFlags.NonPublic);
        private static FieldInfo m_squelchedInfo = typeof(EnemyEmoteHandler).GetField("m_squelched", BindingFlags.Instance | BindingFlags.NonPublic);

        public static void DoSquelchClick(this EnemyEmoteHandler __instance)
        {
            doSquelchClickInfo?.Invoke(__instance, null);
        }        
        
        public static void SquelchPlayer(this EnemyEmoteHandler __instance, int playerId)
        {
            if (m_squelchedInfo.GetValue(__instance) is Map<int, bool> m_squelched)
            {
                m_squelched[playerId] = true;
            }
        }
    }
    
    [HarmonyPatch(typeof(EnemyEmoteHandler), "Awake")]
    public static class EnemyEmoteHandler_Awake
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            generator.DeclareLocal(typeof(bool));
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindLastIndex(x => x.opcode == OpCodes.Stfld && (x.operand as FieldInfo).Name == "m_squelched");
            if (index > 0)
            {
                index++;
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("EmoteSpamBlocker", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                Label label = generator.DefineLabel();
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Brfalse_S, label));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("EmotesBeforeBlock", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldc_I4_0));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ceq));
                Label l2 = generator.DefineLabel();
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Br_S, l2));
                newInstructions.Insert(index, new CodeInstruction(OpCodes.Ldc_I4_0));
                newInstructions[index++].labels.Add(label);
                newInstructions.Insert(index, new CodeInstruction(OpCodes.Stloc_1));
                newInstructions[index].labels.Add(l2);
                index += 9;
                newInstructions[index].opcode = OpCodes.Ldloc_1;
            }
            return newInstructions;
        }
    }
}
