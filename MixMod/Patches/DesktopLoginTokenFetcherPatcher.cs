using Blizzard.T5.Configuration;
using HarmonyLib;
using Hearthstone.Login;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MixMod.Patches
{
    [HarmonyPatch(typeof(DesktopLoginTokenFetcher), "GetTokenFromTokenFetcher")]
    public static class DesktopLoginTokenFetcher_GetTokenFromTokenFetcher
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);

            newInstructions.RemoveAt(0);

            newInstructions.InsertRange(0, new[]
            {
                new CodeInstruction(OpCodes.Ldstr, "Aurora.VerifyWebCredentials"),
                new CodeInstruction(OpCodes.Call, ((Func<string, VarKey>)Vars.Key).Method),
                new CodeInstruction(OpCodes.Ldnull),
                new CodeInstruction(OpCodes.Callvirt, typeof(VarKey).GetMethod("GetStr", BindingFlags.Instance | BindingFlags.Public)),
            });

            return newInstructions;
        }
    }
}
