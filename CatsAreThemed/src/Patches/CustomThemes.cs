using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

using BepInEx.Logging;

using CalApi.Patches;

using HarmonyLib;

using UnityEngine.UI;

namespace CatsAreThemed.Patches;

// ReSharper disable once ClassNeverInstantiated.Global
public class CustomThemes : IPatch {
    internal static ManualLogSource? logger { get; set; }

    public static event EventHandler? initialized;

    public static ThemeSystem? vanillaInstance { get; private set; }
    public static int vanillaCount { get; private set; }
    public static string? currentlyLoadedForWorldpack { get; set; }

    public static string?[] prophecyItems => _prophecyItems;
    private static string?[] _prophecyItems = { };

    private static readonly FieldInfo themesInfo = AccessTools.Field(typeof(ThemeSystem), "themes");
    private static List<ThemeSystem.Theme> themes => (List<ThemeSystem.Theme>)themesInfo.GetValue(vanillaInstance);

    public void Apply() => On.ThemeSystem.Awake += (orig, self) => {
        orig(self);
        vanillaInstance = self;
        vanillaCount = themes.Count;
        initialized?.Invoke(null, EventArgs.Empty);
    };

    public static void Register(string path) {
        string name = Path.GetFileNameWithoutExtension(path);
        logger?.LogInfo($"Parsing modded theme '{name}'");

        try {
            StreamReader stream = File.OpenText(path);
            CustomTheme customTheme = (CustomTheme)new XmlSerializer(typeof(CustomTheme)).Deserialize(stream);
            stream.Close();

            ThemeSystem.Theme baseTheme = themes[customTheme.baseTheme];
            ThemeSystem.Theme theme = CustomTheme.CloneTheme(baseTheme);
            theme.name = name;
            customTheme.ApplyThemeOverrides(ref theme, themes);
            customTheme.ApplyValueOverrides(ref theme);

            Register(theme);
        }
        catch(Exception ex) {
            logger?.LogError($"Failed to parse modded theme '{name}':");
            logger?.LogError(ex);
        }
    }

    private static void Register(ThemeSystem.Theme theme) {
        logger?.LogInfo($"Registering modded theme {theme.name}");
        themes.Add(theme);
    }

    public static void UnregisterAll() {
        logger?.LogInfo("Unregistering all modded themes");

        int removeCount = themes.Count - vanillaCount;
        if(removeCount > 0) themes.RemoveRange(vanillaCount, removeCount);

        currentlyLoadedForWorldpack = null;
    }

    public static void UpdateSelectors() => UpdateSelectors(themes);

    private static void UpdateSelectors(IReadOnlyList<ThemeSystem.Theme> themes) {
        if(!GetThemeDropdown.value) return;

        logger?.LogInfo("Updating theme selectors");

        Array.Resize(ref _prophecyItems, themes.Count);
        GetThemeDropdown.value!.ClearOptions();

        for(int i = 0; i < themes.Count; i++) {
            string name = themes[i].name;
            _prophecyItems[i] = name;
            GetThemeDropdown.value.options.Add(new Dropdown.OptionData(name));
        }

        GetThemeDropdown.value.RefreshShownValue();
    }
}
