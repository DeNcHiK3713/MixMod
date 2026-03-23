using HarmonyLib;
using PegasusShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MixMod.Patches
{
    [HarmonyPatchCategory("Gifts_DevicePreset")]
    [HarmonyPatch(typeof(Network), "GetPlatformBuilder")]
    public static class Network_GetPlatformBuilder
    {
        public static void Postfix(ref Platform __result)
        {
            OSCategory os;
            ScreenCategory screen;
            string deviceName;
            switch (MixModConfig.Get().DevicePreset)
            {
                case DevicePreset.iPad:
                    os = OSCategory.iOS;
                    screen = ScreenCategory.Tablet;
                    deviceName = "iPad13,11";
                    break;
                case DevicePreset.iPhone:
                    os = OSCategory.iOS;
                    screen = ScreenCategory.Phone;
                    deviceName = "iPhone13,4";
                    break;
                case DevicePreset.Phone:
                    os = OSCategory.Android;
                    screen = ScreenCategory.Phone;
                    deviceName = "SAMSUNG-SM-G930FD";
                    break;
                case DevicePreset.Tablet:
                    os = OSCategory.Android;
                    screen = ScreenCategory.Tablet;
                    deviceName = "SAMSUNG-SM-G920F";
                    break;
                case DevicePreset.HuaweiPhone:
                    os = OSCategory.Android;
                    screen = ScreenCategory.Phone;
                    deviceName = "Huawei Nova 8";
                    break;
                case DevicePreset.Mac:
                    os = OSCategory.Mac;
                    screen = ScreenCategory.PC;
                    deviceName = "MacBookPro11,3";
                    break;
                case DevicePreset.Custom:
                    os = MixModConfig.Get().Os;
                    screen = MixModConfig.Get().Screen;
                    deviceName = MixModConfig.Get().DeviceName;
                    break;
                case DevicePreset.Default:
                default:
                    return;
            }
            __result.Os = (int)os;
            __result.Screen = (int)screen;
            __result.Name = deviceName;
            __result.UniqueDeviceIdentifier = GetUniqueDeviceID(os, screen, deviceName);
        }

        private static string GetMD5(string message)
        {
            byte[] hash = MD5.Create().ComputeHash(Encoding.Default.GetBytes(message));
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        private static string GetUniqueDeviceID(OSCategory os, ScreenCategory screen, string deviceName/*, string operatingSystem*/)
        {
            return os switch
            {
                OSCategory.PC => Crypto.SHA1.Calc(Encoding.Default.GetBytes($"MixModeD{SystemInfo.deviceUniqueIdentifier}{os}{screen}{deviceName}")),//return Crypto.SHA1.Calc(Encoding.Default.GetBytes($"MixModeD{SystemInfo.deviceUniqueIdentifier}{os}{screen}{deviceName}{operatingSystem}"));
                OSCategory.Mac or OSCategory.iOS => new Guid(GetMD5($"MixModeD{SystemInfo.deviceUniqueIdentifier}{os}{screen}{deviceName}")).ToString().ToUpper(),//return new Guid(GetMD5($"MixModeD{SystemInfo.deviceUniqueIdentifier}{os}{screen}{deviceName}{operatingSystem}")).ToString().ToUpper();
                OSCategory.Android => GetMD5($"MixModeD_{SystemInfo.deviceUniqueIdentifier}{os}{screen}{deviceName}"),//return GetMD5($"MixModeD_{SystemInfo.deviceUniqueIdentifier}{os}{screen}{deviceName}{operatingSystem}");
                _ => "n/a",
            };
        }
    }
}
