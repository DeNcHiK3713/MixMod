using BepInEx;
using BepInEx.Configuration;
using Blizzard.T5.Core.Time;
using HarmonyLib;
using Hearthstone.Commerce;
using MixMod.Patches;
using MixMod.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MixMod
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Manually load all resource files as they are not loaded by default for an unknown reason.
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"BepInEx\plugins\");
            var files = Directory.GetFiles(path, @"MixMod.resources.dll", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                _ = Assembly.LoadFile(file);
            }

            MixModConfig.Load(Config);
            MixModConfig.Get().timeScaleEnabledEntry.SettingChanged += (_, _) => TimeScaleMgr.Get().Update();
            MixModConfig.Get().devicePresetEntry.SettingChanged += (_, _) =>
            {
                if (MixModConfig.Get().DevicePreset == DevicePreset.Custom)
                {
                    if (MixModConfig.Get().osEntry is null)
                    {
                        MixModConfig.Get().osEntry = Config.Bind("Gifts", "OS", PlatformSettings.OS, MixModLocalization.GiftsOS);
                    }
                    if (MixModConfig.Get().screenEntry is null)
                    {
                        MixModConfig.Get().screenEntry = Config.Bind("Gifts", "Screen", PlatformSettings.Screen, MixModLocalization.GiftsScreen);
                    }
                    if (MixModConfig.Get().deviceNameEntry is null)
                    {
                        MixModConfig.Get().deviceNameEntry = Config.Bind("Gifts", "DeviceName", PlatformSettings.DeviceName, MixModLocalization.GiftsDeviceName);
                    }
                    //if (MixModConfig.Get().operatingSystemEntry is null)
                    //{
                    //    MixModConfig.Get().operatingSystemEntry = Config.Bind("Gifts", "OperatingSystem", SystemInfo.operatingSystem, new ConfigDescription("Версия операционной системы для эмуляции", new AcceptableValueList<string>("Android OS 10 / API-29 (QP1A.190711.020.T860XXU1BTD1)", "Android OS 11 / API-30 (RP1A.200720.012/N986BXXS1DUC1)", "Android OS 10 / API-29 (HUAWEIELS-N29/10.1.0.176C432)", "iOS 14.6")));
                    //}
                }
                else
                {
                    if (MixModConfig.Get().osEntry is not null)
                    {
                        Config.Remove(MixModConfig.Get().osEntry.Definition);
                        MixModConfig.Get().osEntry = null;
                    }
                    if (MixModConfig.Get().screenEntry is not null)
                    {
                        Config.Remove(MixModConfig.Get().screenEntry.Definition);
                        MixModConfig.Get().screenEntry = null;
                    }
                    if (MixModConfig.Get().deviceNameEntry is not null)
                    {
                        Config.Remove(MixModConfig.Get().deviceNameEntry.Definition);
                        MixModConfig.Get().deviceNameEntry = null;
                    }
                    //if (MixModConfig.Get().operatingSystemEntry is not null)
                    //{
                    //    Config.Remove(MixModConfig.Get().operatingSystemEntry.Definition);
                    //    MixModConfig.Get().operatingSystemEntry = null;
                    //}
                }
            };
            MixModConfig.Get().timeScaleEntry.SettingChanged += (_, _) => TimeScaleMgr.Get().Update();
            MixModConfig.Get().timeScaleInGameOnlyEntry.SettingChanged += (_, _) => TimeScaleMgr.Get().Update();
            MixModConfig.Get().languageEntry.SettingChanged += (_, _) => MixModConfig.Get().ReloadLocalization();
            var harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            TimeScaleMgr.Get().Update();
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded! (Patched {harmony.GetPatchedMethods().Count()} methods)");
        }

        private void Update()
        {
            if (!MixModConfig.Get().EnableShortcuts || !Input.anyKey)
            {
                return;
            }
            //if (MixModConfig.Get().PackIdToBuy != 0 && MixModConfig.Get().BuyPackShortcut.IsDown() && StoreManager.Get() != null && Shop.Get() != null)
            //{
            //    var productByPmtId = StoreManager.Get().Catalog.GetProductByPmtId(ProductId.CreateFromValidated(MixModConfig.Get().PackIdToBuy));
            //    var priceDataModel = productByPmtId.Prices.FirstOrDefault(p => p.Currency == CurrencyType.GOLD);
            //    Shop.Get().AttemptToPurchaseProduct(productByPmtId, priceDataModel);
            //    return;
            //}
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
                if (MixModConfig.Get().DivideTimeScaleShortcut.IsDown())
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
                            currentOpponent = PlayerLeaderboardManagerPatch.GetCurrentOpponent();
                        }
                        else
                        {
                            currentOpponent = GameplayPatch.GetCurrentOpponent();
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
