using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using MixMod.Patches;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MixMod
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            MixModConfig.Load(Config);
            MixModConfig.Get().timeScaleEnabledEntry.SettingChanged += (_, _) => TimeScaleMgr.Get().Update();
            MixModConfig.Get().timeScaleEntry.SettingChanged += (_, _) => TimeScaleMgr.Get().Update();
            var harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            TimeScaleMgr.Get().Update();
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded! (Patched {harmony.GetPatchedMethods().Count()} methods)");
        }

        private void Update()
        {
            if (!MixModConfig.Get().EnableShortcuts || !Input.anyKey)
            {
                return;
            }
            if (SoundManager.Get() != null && MixModConfig.Get().SoundMuteShortcut.IsDown())
            {
                SoundManagerPatch.OnMuteKeyPressed();
                return;
            }
            if (MixModConfig.Get().TimeScaleEnabled)
            {
                if (MixModConfig.Get().ResetTimeScaleShortcut.IsDown())
                {
                    MixModConfig.Get().TimeScale = 1f;
                    return;
                }
                if (MixModConfig.Get().MaxTimeScaleShortcut.IsDown())
                {
                    float maxTimeScale = 8f;
                    MixModConfig.Get().TimeScale = maxTimeScale;
                    return;
                }
                if (MixModConfig.Get().DoubleTimeScaleShortcut.IsDown())
                {
                    float timeScale = MixModConfig.Get().TimeScale + 1f;
                    float maxTimeScale = 8f;
                    if (timeScale > maxTimeScale)
                    {
                        timeScale = maxTimeScale;
                    }
                    MixModConfig.Get().TimeScale = timeScale;
                    return;
                }
                if (MixModConfig.Get().DevideTimeScaleShortcut.IsDown())
                {
                    float timeScale = MixModConfig.Get().TimeScale - 1f;
                    float minTimeScale = 1f;
                    if (timeScale < minTimeScale)
                    {
                        timeScale = minTimeScale;
                    }
                    MixModConfig.Get().TimeScale = timeScale;
                    return;
                }
                if (MixModConfig.Get().SimulateDisconnectShortcut.IsDown())
                {
                    Network.Get().SimulateUncleanDisconnectFromGameServer();
                    return;
                }
                if (GameState.Get() == null || GameMgr.Get() == null)
                {
                    return;
                }
                if (GameMgr.Get().IsBattlegrounds() && MixModConfig.Get().ShutUpBobShortcut.IsDown())
                {
                    MixModConfig.Get().ShutUpBob = !MixModConfig.Get().ShutUpBob;
                    return;
                }
                if (GameState.Get().IsGameCreated())
                {
                    if (!GameMgr.Get().IsSpectator())
                    {
                        if (MixModConfig.Get().ConcedeShortcut.IsDown())
                        {
                            GameState.Get().Concede();
                            return;
                        }
                        if (GameState.Get().IsMulliganManagerActive() && MulliganManager.Get().GetMulliganButton() != null && MixModConfig.Get().ContinueMulliganShortcut.IsDown())
                        {
                            MulliganManager.Get().AutomaticContinueMulligan();
                            return;
                        }
                    }
                    if (GameMgr.Get().IsBattlegrounds() && MixModConfig.Get().CopySelectedBattleTagShortcut.IsDown() && PlayerLeaderboardManager.Get() != null && PlayerLeaderboardManager.Get().IsMousedOver())
                    {
                        BnetPlayer currentOpponent = PlayerLeaderboardManager.Get().GetSelectedOpponent();
                        if (currentOpponent != null)
                        {
                            BnetBattleTag battleTag = currentOpponent.GetBattleTag();
                            if (battleTag != null)
                            {
                                string str = battleTag.GetString();
                                ClipboardUtils.CopyToClipboard(str);
                                UIStatus.Get().AddInfo(str);
                            }
                        }
                        return;
                    }
                    if (MixModConfig.Get().CopyBattleTagShortcut.IsDown())
                    {
                        BnetPlayer currentOpponent = null;
                        if (GameMgr.Get().IsBattlegrounds())
                        {
                            if (PlayerLeaderboardManager.Get() != null)
                            {
                                currentOpponent = PlayerLeaderboardManager.Get().GetCurrentOpponent();
                            }
                        }
                        else if (FriendMgr.Get() != null)
                        {
                            currentOpponent = FriendMgr.Get().GetCurrentOpponent();
                        }
                        if (currentOpponent != null)
                        {
                            BnetBattleTag battleTag = currentOpponent.GetBattleTag();
                            if (battleTag != null)
                            {
                                string str = battleTag.GetString();
                                ClipboardUtils.CopyToClipboard(str);
                                UIStatus.Get().AddInfo(str);
                            }
                        }
                        return;
                    }
                    if (GameState.Get().IsMainPhase())
                    {
                        if (MixModConfig.Get().SquelchShortcut.IsDown())
                        {
                            EnemyEmoteHandler.Get().DoSquelchClick();
                            return;
                        }
                        if (GameMgr.Get().IsSpectator())
                        {
                            return;
                        }
                        if (MixModConfig.Get().EndTurnShortcut.IsDown())
                        {
                            InputManager.Get().DoEndTurnButton();
                            return;
                        }
                        if (EmoteHandler.Get() == null)
                        {
                            return;
                        }
                        if (MixModConfig.Get().GreetingsEmoteShortcut.IsDown())
                        {
                            EmoteHandler.Get().HandleKeyboardInput(0, false);
                            return;
                        }
                        if (MixModConfig.Get().WellPlayedEmoteShortcut.IsDown())
                        {
                            EmoteHandler.Get().HandleKeyboardInput(1, false);
                            return;
                        }
                        if (MixModConfig.Get().ThanksEmoteShortcut.IsDown())
                        {
                            EmoteHandler.Get().HandleKeyboardInput(2, false);
                            return;
                        }
                        if (MixModConfig.Get().WowEmoteShortcut.IsDown())
                        {
                            EmoteHandler.Get().HandleKeyboardInput(3, false);
                            return;
                        }
                        if (MixModConfig.Get().OopsEmoteShortcut.IsDown())
                        {
                            EmoteHandler.Get().HandleKeyboardInput(4, false);
                            return;
                        }
                        if (MixModConfig.Get().ThreatenEmoteShortcut.IsDown())
                        {
                            EmoteHandler.Get().HandleKeyboardInput(5, false);
                            return;
                        }
                    }
                }
            }
        }
    }
}
