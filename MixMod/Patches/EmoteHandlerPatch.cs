using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace MixMod.Patches
{
    public static class EmoteHandlerPatch
    {
        private static readonly FieldInfo m_totalEmotesInfo = typeof(EmoteHandler).GetField("m_totalEmotes", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo m_availableEmotesInfo = typeof(EmoteHandler).GetField("m_availableEmotes", BindingFlags.Instance | BindingFlags.NonPublic);
        private static List<EmoteOption> m_FoundedEmotes;

        public static void HandleKeyboardInput(this EmoteHandler __instance, int EmoteIndex, bool useExtended = false)
        {
            if (EmoteHandler.Get().EmoteSpamBlocked())
            {
                return;
            }
            var m_availableEmotes = m_availableEmotesInfo.GetValue(__instance) as List<EmoteOption>;
            if (useExtended)
            {
                if (!MixModConfig.Get().UseExtendedEmotes || EmoteIndex + 1 > m_FoundedEmotes.Count)
                {
                    return;
                }
                m_totalEmotesInfo.SetValue(__instance, (int)m_totalEmotesInfo.GetValue(__instance) + 1);
                if (MixModConfig.Get().DisableRandomForEmotes || !GameState.Get().GetGameEntity().HasTag(GAME_TAG.ALL_TARGETS_RANDOM))
                {
                    m_FoundedEmotes[EmoteIndex].DoClick();
                    return;
                }
                var list = new List<EmoteOption>();
                foreach (EmoteOption emoteOption in m_availableEmotes.Concat(__instance.m_HiddenEmotes))
                {
                    if (emoteOption.CanPlayerUseEmoteType(GameState.Get().GetFriendlySidePlayer()))
                    {
                        list.Add(emoteOption);
                    }
                }
                foreach (EmoteOption emoteOption in m_FoundedEmotes)
                {
                    if (emoteOption != null)
                    {
                        list.Add(emoteOption);
                    }
                }
                if (list.Count > 0)
                {
                    list[UnityEngine.Random.Range(0, list.Count)].DoClick();
                }
            }
            else
            {
                if (EmoteIndex + 1 > m_availableEmotes.Count)
                {
                    return;
                }
                m_totalEmotesInfo.SetValue(__instance, (int)m_totalEmotesInfo.GetValue(__instance) + 1);
                if (MixModConfig.Get().DisableRandomForEmotes || !GameState.Get().GetGameEntity().HasTag(GAME_TAG.ALL_TARGETS_RANDOM))
                {
                    m_availableEmotes[EmoteIndex].DoClick();
                    return;
                }
                var list = new List<EmoteOption>();
                foreach (EmoteOption emoteOption in m_availableEmotes.Concat(__instance.m_HiddenEmotes))
                {
                    if (emoteOption.CanPlayerUseEmoteType(GameState.Get().GetFriendlySidePlayer()))
                    {
                        list.Add(emoteOption);
                    }
                }
                if (list.Count > 0)
                {
                    list[UnityEngine.Random.Range(0, list.Count)].DoClick();
                }
            }
        }

        public static void DetermineFoundedEmotes(this EmoteHandler __instance)
        {
            if (m_FoundedEmotes == null || m_FoundedEmotes.Count == 0)
            {
                m_FoundedEmotes = new List<EmoteOption>(11);
                EmoteOption emote = __instance.m_EmoteOverrides.FirstOrDefault(x => x.m_EmoteType == EmoteType.HAPPY_NEW_YEAR) ?? new EmoteOption
                {
                    m_EmoteType = EmoteType.HAPPY_NEW_YEAR,
                    m_StringTag = "GAMEPLAY_EMOTE_LABEL_GREETINGS"
                };
                m_FoundedEmotes.Add(emote);
                emote = __instance.m_EmoteOverrides.FirstOrDefault(x => x.m_EmoteType == EmoteType.HAPPY_NEW_YEAR_LUNAR) ?? new EmoteOption
                {
                    m_EmoteType = EmoteType.HAPPY_NEW_YEAR_LUNAR,
                    m_StringTag = "GAMEPLAY_EMOTE_LABEL_GREETINGS"
                };
                m_FoundedEmotes.Add(emote);
                emote = __instance.m_EmoteOverrides.FirstOrDefault(x => x.m_EmoteType == EmoteType.HAPPY_HOLIDAYS) ?? new EmoteOption
                {
                    m_EmoteType = EmoteType.HAPPY_HOLIDAYS,
                    m_StringTag = "GAMEPLAY_EMOTE_LABEL_GREETINGS"
                };
                m_FoundedEmotes.Add(emote);
                emote = __instance.m_EmoteOverrides.FirstOrDefault(x => x.m_EmoteType == EmoteType.HAPPY_HALLOWEEN) ?? new EmoteOption
                {
                    m_EmoteType = EmoteType.HAPPY_HALLOWEEN,
                    m_StringTag = "GAMEPLAY_EMOTE_LABEL_GREETINGS"
                };
                m_FoundedEmotes.Add(emote);
                emote = __instance.m_EmoteOverrides.FirstOrDefault(x => x.m_EmoteType == EmoteType.HAPPY_NOBLEGARDEN) ?? new EmoteOption
                {
                    m_EmoteType = EmoteType.HAPPY_NOBLEGARDEN,
                    m_StringTag = "GAMEPLAY_EMOTE_LABEL_GREETINGS"
                };
                m_FoundedEmotes.Add(emote);
                emote = __instance.m_EmoteOverrides.FirstOrDefault(x => x.m_EmoteType == EmoteType.FIRE_FESTIVAL_FIREWORKS_RANK_ONE) ?? new EmoteOption
                {
                    m_EmoteType = EmoteType.FIRE_FESTIVAL_FIREWORKS_RANK_ONE,
                    m_StringTag = "GAMEPLAY_EMOTE_LABEL_FIREWORKS"
                };
                m_FoundedEmotes.Add(emote);
                emote = __instance.m_EmoteOverrides.FirstOrDefault(x => x.m_EmoteType == EmoteType.FIRE_FESTIVAL_FIREWORKS_RANK_TWO) ?? new EmoteOption
                {
                    m_EmoteType = EmoteType.FIRE_FESTIVAL_FIREWORKS_RANK_TWO,
                    m_StringTag = "GAMEPLAY_EMOTE_LABEL_FIREWORKS"
                };
                m_FoundedEmotes.Add(emote);
                emote = __instance.m_EmoteOverrides.FirstOrDefault(x => x.m_EmoteType == EmoteType.FIRE_FESTIVAL_FIREWORKS_RANK_THREE) ?? new EmoteOption
                {
                    m_EmoteType = EmoteType.FIRE_FESTIVAL_FIREWORKS_RANK_THREE,
                    m_StringTag = "GAMEPLAY_EMOTE_LABEL_FIREWORKS"
                };
                m_FoundedEmotes.Add(emote);
                emote = __instance.m_EmoteOverrides.FirstOrDefault(x => x.m_EmoteType == EmoteType.FROST_FESTIVAL_FIREWORKS_RANK_ONE) ?? new EmoteOption
                {
                    m_EmoteType = EmoteType.FROST_FESTIVAL_FIREWORKS_RANK_ONE,
                    m_StringTag = "GAMEPLAY_EMOTE_LABEL_FIREWORKS"
                };
                m_FoundedEmotes.Add(emote);
                emote = __instance.m_EmoteOverrides.FirstOrDefault(x => x.m_EmoteType == EmoteType.FROST_FESTIVAL_FIREWORKS_RANK_TWO) ?? new EmoteOption
                {
                    m_EmoteType = EmoteType.FROST_FESTIVAL_FIREWORKS_RANK_TWO,
                    m_StringTag = "GAMEPLAY_EMOTE_LABEL_FIREWORKS"
                };
                m_FoundedEmotes.Add(emote);
                emote = __instance.m_EmoteOverrides.FirstOrDefault(x => x.m_EmoteType == EmoteType.FROST_FESTIVAL_FIREWORKS_RANK_THREE) ?? new EmoteOption
                {
                    m_EmoteType = EmoteType.FROST_FESTIVAL_FIREWORKS_RANK_THREE,
                    m_StringTag = "GAMEPLAY_EMOTE_LABEL_FIREWORKS"
                };
                m_FoundedEmotes.Add(emote);
                foreach (EmoteOption emoteOption in m_FoundedEmotes)
                {
                    emoteOption.UpdateEmoteType();
                }
            }
        }
    }

    [HarmonyPatch(typeof(EmoteHandler), "DetermineAvailableEmotes")]
    public static class EmoteHandler_DetermineAvailableEmotes
    {
        public static void Postfix(EmoteHandler __instance)
        {
            __instance.DetermineFoundedEmotes();
        }
    }

    [HarmonyPatchCategory("Gameplay_DisableRandomForEmotes")]
    [HarmonyPatch(typeof(EmoteHandler), nameof(EmoteHandler.HandleInput))]
    public static class EmoteHandler_HandleInput
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindLastIndex(x => x.opcode == OpCodes.Callvirt && (x.operand as MethodInfo).Name == "HasTag");
            if (index > 0)
            {
                newInstructions[index] = new CodeInstruction(OpCodes.Ldc_I4_0);
                index -= 3;
                newInstructions.RemoveRange(index, 3);
            }
            return newInstructions;
        }
    }

    [HarmonyPatchCategory("Global_TimeScaleEnabled")]
    [HarmonyPatch(typeof(EmoteHandler), nameof(EmoteHandler.EmoteSpamBlocked))]
    public static class EmoteHandler_EmoteSpamBlocked_TimeScaleEnabled
    {
        private static readonly FieldInfo m_timeSinceLastEmoteInfo = typeof(EmoteHandler).GetField("m_timeSinceLastEmote", BindingFlags.Instance | BindingFlags.NonPublic);

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = new List<CodeInstruction>(instructions);
            int index = 2;
            newInstructions[index++].opcode = OpCodes.Brtrue_S;
            newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldarg_0));
            newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldfld, m_timeSinceLastEmoteInfo));
            newInstructions.Insert(index++, new CodeInstruction(OpCodes.Ldc_R4, 1.5f));
            var l1 = generator.DefineLabel();
            newInstructions.Insert(index++, new CodeInstruction(OpCodes.Bge_Un_S, l1));
            newInstructions[index].MoveLabelsFrom(newInstructions[index + 2]);
            index += 2;
            newInstructions[index].labels.Add(l1);
            return newInstructions;
        }
    }

    [HarmonyPatchCategory("Gameplay_ExtendedBM")]
    [HarmonyPatch(typeof(EmoteHandler), nameof(EmoteHandler.EmoteSpamBlocked))]
    public static class EmoteHandler_EmoteSpamBlocked_ExtendedBM
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = new List<CodeInstruction>(instructions);
            int index = newInstructions.FindIndex(x => x.opcode == OpCodes.Callvirt && (x.operand as MethodInfo).Name == "IsFriendly");
            if (index > 0)
            {
                index--;
                var newInstruction = new CodeInstruction(OpCodes.Ldc_I4_0);
                newInstructions[index].MoveLabelsTo(newInstruction);
                newInstructions[index++] = newInstruction;
                newInstructions[index++] = new CodeInstruction(OpCodes.Ret);
                newInstructions.RemoveRange(index, newInstructions.Count - index);
            }
            return newInstructions;
        }
    }
}
