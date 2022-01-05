using CalApi.Patches;

using HarmonyLib;

using UnityEngine.UI;

namespace CatsAreThemed.Patches;

// ReSharper disable once ClassNeverInstantiated.Global
internal class GetThemeDropdown : IPatch {
    public static Dropdown? value { get; private set; }
    public static int vanillaOptionsCount { get; private set; }

    public void Apply() => On.RSSystem.RoomSettingsUI.Awake += (orig, self) => {
        orig(self);
        value = (Dropdown)AccessTools.Field(typeof(RSSystem.RoomSettingsUI), "themeDropdown").GetValue(self);
        vanillaOptionsCount = value.options.Count;
    };
}