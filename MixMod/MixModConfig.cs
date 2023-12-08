using BepInEx;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MixMod
{
    public class MixModConfig
    {
        private static MixModConfig _mixModConfig;
        private static PropertyInfo orphanedEntriesInfo = typeof(ConfigFile).GetProperty("OrphanedEntries", BindingFlags.NonPublic | BindingFlags.Instance);
        private readonly ConfigFile _config;

        internal ConfigEntry<bool> devEnabledEntry;
        internal ConfigEntry<bool> isInternalEntry;
        internal ConfigEntry<int> boardEntry;
        internal ConfigEntry<CardState> goldenCoinEntry;
        internal ConfigEntry<bool> enableShortcutsEntry;
        internal ConfigEntry<bool> timeScaleEnabledEntry;
        internal ConfigEntry<bool> timeScaleInGameOnlyEntry;
        internal ConfigEntry<float> timeScaleEntry;
        internal ConfigEntry<bool> skipHeroIntroEntry;
        internal ConfigEntry<bool> shutUpBobEntry;
        internal ConfigEntry<bool> extendedBMEntry;
        internal ConfigEntry<bool> disableRandomForEmotesEntry;
        internal ConfigEntry<bool> useExtendedEmotesEntry;
        internal ConfigEntry<bool> emoteSpamBlockerEntry;
        internal ConfigEntry<int> emotesBeforeBlockEntry;
        internal ConfigEntry<bool> disableThinkEmotesEntry;
        internal ConfigEntry<CardState> goldenEntry;
        internal ConfigEntry<CardState> diamondEntry;
        internal ConfigEntry<CardState> signatureEntry;
        internal ConfigEntry<bool> showOpponentRankInGameEntry;
        internal ConfigEntry<bool> moveEnemyCardsEntry;
        internal ConfigEntry<int> packIdToBuyEntry;
        internal ConfigEntry<DevicePreset> devicePresetEntry;
        internal ConfigEntry<OSCategory> osEntry;
        internal ConfigEntry<ScreenCategory> screenEntry;
        internal ConfigEntry<string> deviceNameEntry;
        internal ConfigEntry<string> operatingSystemEntry;
        internal ConfigEntry<KeyboardShortcut> testShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> soundMuteShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> resetTimeScaleShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> maxTimeScaleShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> doubleTimeScaleShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> devideTimeScaleShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> concedeShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> continueMulliganShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> squelchShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> shutUpBobShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> endTurnShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> greetingsEmoteShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> wellPlayedEmoteShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> thanksEmoteShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> wowEmoteShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> oopsEmoteShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> threatenEmoteShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> copyBattleTagShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> copySelectedBattleTagShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> simulateDisconnectShortcutEntry;
        internal ConfigEntry<KeyboardShortcut> buyPackShortcutEntry;

        public MixModConfig(ConfigFile config)
        {
            _config = config;
#if DEBUG
            devEnabledEntry = config.Bind("Dev", "DevEnabled", false, "Режим разработчика мода");
            isInternalEntry = config.Bind("Dev", "IsInternal", false, "Режим разработчика игры");
            boardEntry = config.Bind("Dev", "Board", 0, "Номер игрового поля для замены");
            goldenCoinEntry = config.Bind("Dev", "GoldenCoin", CardState.Default, "Изменения для монеток");
#else
            var orphanedEntries = orphanedEntriesInfo?.GetValue(config) as Dictionary<ConfigDefinition, string>;
            if (orphanedEntries is not null)
            {
                if (orphanedEntries.Any(x => x.Key.Section == "Dev" && x.Key.Key == "DevEnabled"))
                {
                    devEnabledEntry = config.Bind("Dev", "DevEnabled", false, "Режим разработчика мода");
                }
                if (orphanedEntries.Any(x => x.Key.Section == "Dev" && x.Key.Key == "IsInternal"))
                {
                    isInternalEntry = config.Bind("Dev", "IsInternal", false, "Режим разработчика игры");
                }
                if (orphanedEntries.Any(x => x.Key.Section == "Dev" && x.Key.Key == "Board"))
                {
                    boardEntry = config.Bind("Dev", "Board", 0, "Номер игрового поля для замены");
                }
                if (orphanedEntries.Any(x => x.Key.Section == "Dev" && x.Key.Key == "GoldenCoin"))
                {
                    goldenCoinEntry = config.Bind("Dev", "GoldenCoin", CardState.Default, "Изменения для монеток");
                }
            }
#endif
            enableShortcutsEntry = config.Bind("Global", "EnableShortcuts", false, "Включить горячие клавиши");
            timeScaleEnabledEntry = config.Bind("Global", "TimeScaleEnabled", false, "Включить ускорение анимации");
            timeScaleInGameOnlyEntry = config.Bind("Global", "TimeScaleInGameOnly", false, "Ускорение анимации только в игре");
            timeScaleEntry = config.Bind("Global", "TimeScale", 1f, new ConfigDescription("Значение для ускорения анимации", new AcceptableValueRange<float>(1f, 8f)));
            skipHeroIntroEntry = config.Bind("Gameplay", "SkipHeroIntro", false, "Отключить анимацию представления героев");
            shutUpBobEntry = config.Bind("Gameplay", "ShutUpBob", false, "Отключить комментарии Боба на полях сражений");
            extendedBMEntry = config.Bind("Gameplay", "ExtendedBM", false, "Убрать ограничение на использование эмоций");
            disableRandomForEmotesEntry = config.Bind("Gameplay", "DisableRandomForEmotes", false, "Отключить использование случайных эмоции, если все цели выбираются случайно");
            //UseExtendedEmotesEntry = config.Bind("Gameplay", "UseExtendedEmotes", false, "Включить использование расширенных эмоций");
            emoteSpamBlockerEntry = config.Bind("Gameplay", "EmoteSpamBlocker", false, "Включить блокировщик спама эмоциями");
            emotesBeforeBlockEntry = config.Bind("Gameplay", "EmotesBeforeBlock", 0, "Количество эмоции оппонента за 10 секунд, которое оппонент может сказать. При превышении этого числа, его эмоции будут отключены\n0 для отключения эмоций в начале матча");
            disableThinkEmotesEntry = config.Bind("Gameplay", "DisableThinkEmotes", false, "Отключить эмоции раздумья героев");
            goldenEntry = config.Bind("Gameplay", "GOLDEN", CardState.Default, "Изменения для золотых карт");
            diamondEntry = config.Bind("Gameplay", "DIAMOND", CardState.Default, "Изменения для бриллиантовых карт");
            signatureEntry = config.Bind("Gameplay", "SIGNATURE", CardState.Default, "Изменения для сигнатурных карт");
            showOpponentRankInGameEntry = config.Bind("Gameplay", "ShowOpponentRankInGame", false, "Включить отображение ранга текущего противника");
            moveEnemyCardsEntry = config.Bind("Others", "MoveEnemyCards", false, "Развернуть карты в руке оппонента в режиме зрителя");
            //packIdToBuyEntry = config.Bind("Others", "PackIdToBuy", 0, "ID пака для покупки");
            devicePresetEntry = config.Bind("Gifts", "DevicePreset", DevicePreset.Default, "Имитация устройств");
#if DEBUG
            testShortcutEntry = config.Bind("Shortcuts", "TestShortcut", new KeyboardShortcut(KeyCode.U, KeyCode.LeftControl), "Клавиша для тестов");
#else
            if (orphanedEntries is not null)
            {
                if (orphanedEntries.Any(x => x.Key.Section == "Shortcuts" && x.Key.Key == "TestShortcut"))
                {
                    testShortcutEntry = config.Bind("Shortcuts", "TestShortcut", new KeyboardShortcut(KeyCode.U, KeyCode.LeftControl), "Клавиша для тестов");
                }
            }
#endif
            soundMuteShortcutEntry = config.Bind("Shortcuts", "SoundMute", new KeyboardShortcut(KeyCode.V));
            resetTimeScaleShortcutEntry = config.Bind("Shortcuts", "ResetTimeScale", new KeyboardShortcut(KeyCode.LeftArrow));
            maxTimeScaleShortcutEntry = config.Bind("Shortcuts", "MaxTimeScale", new KeyboardShortcut(KeyCode.RightArrow));
            doubleTimeScaleShortcutEntry = config.Bind("Shortcuts", "DoubleTimeScale", new KeyboardShortcut(KeyCode.UpArrow));
            devideTimeScaleShortcutEntry = config.Bind("Shortcuts", "DevideTimeScale", new KeyboardShortcut(KeyCode.DownArrow));
            concedeShortcutEntry = config.Bind("Shortcuts", "Concede", new KeyboardShortcut(KeyCode.Space, KeyCode.LeftControl));
            continueMulliganShortcutEntry = config.Bind("Shortcuts", "ContinueMulligan", new KeyboardShortcut(KeyCode.Space));
            squelchShortcutEntry = config.Bind("Shortcuts", "Squelch", new KeyboardShortcut(KeyCode.C));
            shutUpBobShortcutEntry = config.Bind("Shortcuts", "ShutUpBob", new KeyboardShortcut(KeyCode.B));
            endTurnShortcutEntry = config.Bind("Shortcuts", "EndTurn", new KeyboardShortcut(KeyCode.Space));
            greetingsEmoteShortcutEntry = config.Bind("Shortcuts", "Greetings", new KeyboardShortcut(KeyCode.Z));
            wellPlayedEmoteShortcutEntry = config.Bind("Shortcuts", "WellPlayed", new KeyboardShortcut(KeyCode.A));
            thanksEmoteShortcutEntry = config.Bind("Shortcuts", "ThanksEmote", new KeyboardShortcut(KeyCode.Q));
            wowEmoteShortcutEntry = config.Bind("Shortcuts", "WowEmote", new KeyboardShortcut(KeyCode.W));
            oopsEmoteShortcutEntry = config.Bind("Shortcuts", "OopsEmote", new KeyboardShortcut(KeyCode.S));
            threatenEmoteShortcutEntry = config.Bind("Shortcuts", "ThreatenEmote", new KeyboardShortcut(KeyCode.X));
            copyBattleTagShortcutEntry = config.Bind("Shortcuts", "CopyBattleTag", new KeyboardShortcut(KeyCode.C, KeyCode.LeftControl));
            copySelectedBattleTagShortcutEntry = config.Bind("Shortcuts", "CopySelectedBattleTag", new KeyboardShortcut(KeyCode.Mouse0));
            simulateDisconnectShortcutEntry = config.Bind("Shortcuts", "SimulateDisconnect", new KeyboardShortcut(KeyCode.D, KeyCode.LeftControl));
            //buyPackShortcutEntry = config.Bind("Shortcuts", "BuyPack", new KeyboardShortcut(KeyCode.B, KeyCode.LeftControl));
        }

        public static void Load(ConfigFile config)
        {
            _mixModConfig = new MixModConfig(config);
        }

        public static MixModConfig Get()
        {
            return _mixModConfig;
        }

        public bool DevEnabled
        {
            get => devEnabledEntry?.Value ?? false;
            set
            {
                if (devEnabledEntry is null)
                {
                    devEnabledEntry = _config.Bind("Dev", "DevEnabled", false, "Режим разработчика мода");
                }
                devEnabledEntry.Value = value;
            }
        }
        public bool IsInternal
        {
            get => isInternalEntry?.Value ?? false;
            set
            {
                if (isInternalEntry is null)
                {
                    isInternalEntry = _config.Bind("Dev", "IsInternal", false, "Режим разработчика игры");
                }
                isInternalEntry.Value = value;
            }
        }
        public int Board
        {
            get => boardEntry?.Value ?? 0;
            set
            {
                if (boardEntry is null)
                {
                    boardEntry = _config.Bind("Dev", "Board", 0, "Номер игрового поля для замены");
                }
                boardEntry.Value = value;
            }
        }
        public CardState GoldenCoin
        {
            get => goldenCoinEntry?.Value ?? CardState.Default;
            set
            {
                if (goldenCoinEntry is null)
                {
                    goldenCoinEntry = _config.Bind("Dev", "GoldenCoin", CardState.Default, "Изменения для монеток");
                }
                goldenCoinEntry.Value = value;
            }
        }
        public bool EnableShortcuts { get => enableShortcutsEntry.Value; set => enableShortcutsEntry.Value = value; }
        public bool TimeScaleEnabled { get => timeScaleEnabledEntry.Value; set => timeScaleEnabledEntry.Value = value; }
        public bool TimeScaleInGameOnly { get => timeScaleInGameOnlyEntry.Value; set => timeScaleInGameOnlyEntry.Value = value; }
        public float TimeScale { get => timeScaleEntry.Value; set => timeScaleEntry.Value = value; }
        public bool SkipHeroIntro { get => skipHeroIntroEntry.Value; set => skipHeroIntroEntry.Value = value; }
        public bool ShutUpBob { get => shutUpBobEntry.Value; set => shutUpBobEntry.Value = value; }
        public bool ExtendedBM { get => extendedBMEntry.Value; set => extendedBMEntry.Value = value; }
        public bool DisableRandomForEmotes { get => disableRandomForEmotesEntry.Value; set => disableRandomForEmotesEntry.Value = value; }
        public bool UseExtendedEmotes { get => useExtendedEmotesEntry.Value; set => useExtendedEmotesEntry.Value = value; }
        public bool EmoteSpamBlocker { get => emoteSpamBlockerEntry.Value; set => emoteSpamBlockerEntry.Value = value; }
        public int EmotesBeforeBlock { get => emotesBeforeBlockEntry.Value; set => emotesBeforeBlockEntry.Value = value; }
        public bool DisableThinkEmotes { get => disableThinkEmotesEntry.Value; set => disableThinkEmotesEntry.Value = value; }
        public CardState GOLDEN { get => goldenEntry.Value; set => goldenEntry.Value = value; }
        public CardState DIAMOND { get => diamondEntry.Value; set => diamondEntry.Value = value; }
        public CardState SIGNATURE { get => signatureEntry.Value; set => signatureEntry.Value = value; }
        public bool ShowOpponentRankInGame { get => showOpponentRankInGameEntry.Value; set => showOpponentRankInGameEntry.Value = value; }
        public bool MoveEnemyCards { get => moveEnemyCardsEntry.Value; set => moveEnemyCardsEntry.Value = value; }
        public int PackIdToBuy { get => packIdToBuyEntry.Value; set => packIdToBuyEntry.Value = value; }
        public DevicePreset DevicePreset { get => devicePresetEntry.Value; set => devicePresetEntry.Value = value; }
        public OSCategory Os { get => osEntry.Value; set => osEntry.Value = value; }
        public ScreenCategory Screen { get => screenEntry.Value; set => screenEntry.Value = value; }
        public string DeviceName { get => deviceNameEntry.Value; set => deviceNameEntry.Value = value; }
        public string OperatingSystem { get => operatingSystemEntry.Value; set => operatingSystemEntry.Value = value; }


        public KeyboardShortcut TestShortcut
        {
            get => testShortcutEntry?.Value ?? KeyboardShortcut.Empty;
            set
            {
                if (testShortcutEntry is null)
                {
                    testShortcutEntry = _config.Bind("Shortcuts", "TestShortcut", new KeyboardShortcut(KeyCode.U, KeyCode.LeftControl), "Клавиша для тестов");
                }
                testShortcutEntry.Value = value;
            }
        }
        public KeyboardShortcut SoundMuteShortcut { get => soundMuteShortcutEntry.Value; set => soundMuteShortcutEntry.Value = value; }
        public KeyboardShortcut ResetTimeScaleShortcut { get => resetTimeScaleShortcutEntry.Value; set => resetTimeScaleShortcutEntry.Value = value; }
        public KeyboardShortcut MaxTimeScaleShortcut { get => maxTimeScaleShortcutEntry.Value; set => maxTimeScaleShortcutEntry.Value = value; }
        public KeyboardShortcut DoubleTimeScaleShortcut { get => doubleTimeScaleShortcutEntry.Value; set => doubleTimeScaleShortcutEntry.Value = value; }
        public KeyboardShortcut DevideTimeScaleShortcut { get => devideTimeScaleShortcutEntry.Value; set => devideTimeScaleShortcutEntry.Value = value; }
        public KeyboardShortcut ConcedeShortcut { get => concedeShortcutEntry.Value; set => concedeShortcutEntry.Value = value; }
        public KeyboardShortcut ContinueMulliganShortcut { get => continueMulliganShortcutEntry.Value; set => continueMulliganShortcutEntry.Value = value; }
        public KeyboardShortcut SquelchShortcut { get => squelchShortcutEntry.Value; set => squelchShortcutEntry.Value = value; }
        public KeyboardShortcut ShutUpBobShortcut { get => shutUpBobShortcutEntry.Value; set => shutUpBobShortcutEntry.Value = value; }
        public KeyboardShortcut EndTurnShortcut { get => endTurnShortcutEntry.Value; set => endTurnShortcutEntry.Value = value; }
        public KeyboardShortcut GreetingsEmoteShortcut { get => greetingsEmoteShortcutEntry.Value; set => greetingsEmoteShortcutEntry.Value = value; }
        public KeyboardShortcut WellPlayedEmoteShortcut { get => wellPlayedEmoteShortcutEntry.Value; set => wellPlayedEmoteShortcutEntry.Value = value; }
        public KeyboardShortcut ThanksEmoteShortcut { get => thanksEmoteShortcutEntry.Value; set => thanksEmoteShortcutEntry.Value = value; }
        public KeyboardShortcut WowEmoteShortcut { get => wowEmoteShortcutEntry.Value; set => wowEmoteShortcutEntry.Value = value; }
        public KeyboardShortcut OopsEmoteShortcut { get => oopsEmoteShortcutEntry.Value; set => oopsEmoteShortcutEntry.Value = value; }
        public KeyboardShortcut ThreatenEmoteShortcut { get => threatenEmoteShortcutEntry.Value; set => threatenEmoteShortcutEntry.Value = value; }
        public KeyboardShortcut CopyBattleTagShortcut { get => copyBattleTagShortcutEntry.Value; set => copyBattleTagShortcutEntry.Value = value; }
        public KeyboardShortcut CopySelectedBattleTagShortcut { get => copySelectedBattleTagShortcutEntry.Value; set => copySelectedBattleTagShortcutEntry.Value = value; }
        public KeyboardShortcut SimulateDisconnectShortcut { get => simulateDisconnectShortcutEntry.Value; set => simulateDisconnectShortcutEntry.Value = value; }
        public KeyboardShortcut BuyPackShortcut { get => buyPackShortcutEntry.Value; set => buyPackShortcutEntry.Value = value; }
    }
}
