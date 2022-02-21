using System.Collections.Generic;

using CalApi.Patches;

using HarmonyLib;

using Language;

using UnityEngine.UI;

using CalDataEditor = DataEditor;

namespace CatsAreThemed.Patches;

// ReSharper disable once UnusedType.Global
internal class DontChangeThemeInDropdown : IPatch {
    public void Apply() => On.RSSystem.RoomSettingsUI.Awake += (orig, self) => {
        orig(self);
        Dropdown themeDropdown =
            (Dropdown)AccessTools.Field(typeof(RSSystem.RoomSettingsUI), "themeDropdown").GetValue(self);
        ThemeSystem themeSystem =
            (ThemeSystem)AccessTools.Field(typeof(ThemeSystem), "instance").GetValue(null);
        List<ThemeSystem.Theme> themes =
            (List<ThemeSystem.Theme>)AccessTools.Field(typeof(ThemeSystem), "themes").GetValue(themeSystem);
        if(themeDropdown.options.Count >= themes.Count + 1) return;
        themeDropdown.options.Add(new Dropdown.OptionData("Don't change"));
        themeDropdown.GetComponent<UILanguageSetter>().keys.Add("");
    };
}
