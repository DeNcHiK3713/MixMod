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
    [HarmonyPatch(typeof(FiresideGatheringManager), nameof(FiresideGatheringManager.RequestNearbyFSGs))]
    public static class FiresideGatheringManager_RequestNearbyFSGs
    {
        public static bool Prefix(ref bool ___m_isRequestNearbyFSGsPending)
        {
            if (MixModConfig.Get().FiresideGathering)
            {
                ___m_isRequestNearbyFSGsPending = true;
                Network.Get().RequestNearbyFSGs(MixModConfig.Get().Latitude, MixModConfig.Get().Longitude, MixModConfig.Get().GpsAccuracy, null);
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(FiresideGatheringManager), "OnFSGAllowed")]
    public static class FiresideGatheringManager_OnFSGAllowed
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindLastIndex(x => x.opcode == OpCodes.Callvirt && (x.operand as MethodInfo).Name == "get_GPSOrWifiServicesAvailable");
            if (index > 0)
            {
                index++;
                var l1 = newInstructions[index].operand;
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Brtrue_S, l1));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Call, ((Func<MixModConfig>)MixModConfig.Get).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, typeof(MixModConfig).GetProperty("FiresideGathering", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()));
            }
            return newInstructions;
        }
    }
}
