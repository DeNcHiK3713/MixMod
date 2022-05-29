using Blizzard.GameService.SDK.Client.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MixMod.Patches
{
    public static class SharedPlayerInfoPatch
    {
        private static FieldInfo m_gameAccountIdInfo = typeof(SharedPlayerInfo).GetField("m_gameAccountId", BindingFlags.NonPublic | BindingFlags.Instance);
        
        public static BnetGameAccountId GetGameAccountId(this SharedPlayerInfo __instance)
        {
            return m_gameAccountIdInfo?.GetValue(__instance) as BnetGameAccountId;
        }
    }
}
