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
    public static class FriendMgrPatch
    {
        private static BnetPlayer m_currentOpponent;

        public static BnetPlayer GetCurrentOpponent(this FriendMgr __instance)
        {
            return m_currentOpponent;
        }
        
        private static void UpdateCurrentOpponent()
        {
            if (GameState.Get() == null)
            {
                m_currentOpponent = null;
                return;
            }
            Player opposingSidePlayer = GameState.Get().GetOpposingSidePlayer();
            if (opposingSidePlayer == null)
            {
                m_currentOpponent = null;
                return;
            }
            m_currentOpponent = BnetPresenceMgr.Get().GetPlayer(opposingSidePlayer.GetGameAccountId());
        }

        public static void OnGameCreated(GameState.CreateGamePhase phase, object userData)
        {
            GameState.Get().UnregisterCreateGameListener(new GameState.CreateGameCallback(OnGameCreated), null);
            UpdateCurrentOpponent();
        }
    }

    [HarmonyPatch(typeof(FriendMgr), "OnSceneLoaded")]
    public static class FriendMgr_OnSceneLoaded
    {
        private static MethodInfo registerCreateGameListenerInfo = typeof(GameState).GetMethod("RegisterCreateGameListener", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(GameState.CreateGameCallback), typeof(object) }, null);

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindLastIndex(x => x.opcode == OpCodes.Ldftn && (x.operand as MethodInfo).Name == "OnGameOver");
            if (index > 0)
            {
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldftn, ((Action<GameState.CreateGamePhase, object>)FriendMgrPatch.OnGameCreated).Method));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Newobj,
                    typeof(GameState.CreateGameCallback).GetConstructor(BindingFlags.Instance | BindingFlags.Public,
                        null, new[] { typeof(object), typeof(IntPtr) }, null)));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldnull));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Callvirt, registerCreateGameListenerInfo));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Pop));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldloc_0));
                newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldarg_0));
            }
            return newInstructions;
        }
    }
}
