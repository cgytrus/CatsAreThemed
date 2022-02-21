using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

using BepInEx.Logging;

using HarmonyLib;

using Mono.Cecil.Cil;

using MonoMod.Cil;

using UnityEngine;

namespace CatsAreThemed;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
public static class CustomThemes {

    public static event EventHandler? initialized;

    public static string startingTheme { get; set; } = "";

    private static readonly Dictionary<string, ThemeSystem.Theme> themes = new();

    private static ManualLogSource? _logger;
    private static ThemeSystem? _themeSystem;

    internal static void Setup(ManualLogSource logger) {
        _logger = logger;
        _logger.LogInfo("Setting up");

        IL.ThemeSystem.Awake += il => {
            ILCursor cursor = new(il);
            cursor.GotoNext(code => code.MatchCall<ThemeSystem>(nameof(ThemeSystem.ApplyTheme)));
            int endIndex = cursor.Index;
            cursor.GotoPrev(code => code.MatchLdlen());
            cursor.Index -= 3;
            cursor.RemoveRange(endIndex - cursor.Index + 1);
        };

        On.ThemeSystem.Awake += (orig, self) => {
            orig(self);
            _themeSystem = self;
            Initialize();
            initialized?.Invoke(null, EventArgs.Empty);
            TryApplyStartingTheme();
        };

        On.ThemeSystem.ApplyTheme += (orig, theme) => {
            _logger.LogInfo($"[Vanilla ({nameof(ThemeSystem)})] Applying theme ID {theme}");
            orig(theme);
        };

        IL.ThemeSystem.ApplyThemeToItem += il => {
            ILCursor cursor = new(il);

            while(cursor.TryGotoNext(code => code.MatchCallvirt<Renderer>($"get_{nameof(Renderer.sharedMaterial)}")))
                cursor.Next.Operand = AccessTools.PropertyGetter(typeof(Renderer), nameof(Renderer.material));
        };
    }

    private static void Initialize() {
        _logger?.LogInfo("Initializing");

        themes.Clear();
    }

    private static void RegisterVanillaThemes() {
        _logger?.LogInfo("Registering vanilla themes");

        List<ThemeSystem.Theme> themes =
            (List<ThemeSystem.Theme>)AccessTools.Field(typeof(ThemeSystem), "themes").GetValue(_themeSystem);
        foreach(ThemeSystem.Theme theme in themes) RegisterTheme(theme);
    }

    public static void RegisterCustomThemes(string? path) {
        if(!Directory.Exists(path)) return;
        _logger?.LogInfo($"Registering custom themes at {path}");
        foreach(string file in Directory.GetFiles(path))
            if(Path.GetExtension(file) == ".theme")
                RegisterTheme(file);
    }

    public static void ReregisterThemes() {
        _logger?.LogInfo("Reregistering themes");
        UnregisterAllThemes();
        RegisterVanillaThemes();
    }

    private static void UnregisterAllThemes() {
        _logger?.LogInfo("Unregistering themes");
        themes.Clear();
        UpdateProphecyDropdown();
    }

    public static bool TryApplyStartingTheme() {
        if(TryApplyTheme(startingTheme)) return true;
        int[] themes = { 1, 2, 3, 4 };
        return TryApplyTheme(themes[UnityEngine.Random.Range(0, themes.Length)]);
    }

    public static bool TryApplyTheme(string name, PolyMap.Item? item = null) {
        if(!TryGetTheme(name, out ThemeSystem.Theme theme)) {
            _logger?.LogWarning($"Theme {name} not found!");
            return false;
        }
        ApplyTheme(theme, item);
        return true;
    }

    public static void ApplyTheme(ThemeSystem.Theme theme, PolyMap.Item? item = null) {
        List<ThemeSystem.Theme> themes =
            (List<ThemeSystem.Theme>)AccessTools.Field(typeof(ThemeSystem), "themes").GetValue(_themeSystem);
        themes.Add(theme);
        TryApplyTheme(themes.Count - 1, item);
        themes.RemoveAt(themes.Count - 1);
    }

    public static bool TryApplyTheme(int id, PolyMap.Item? item = null) {
        if(!TryGetTheme(id, out ThemeSystem.Theme theme)) {
            _logger?.LogWarning($"Theme {id.ToString()} not found!");
            return false;
        }

        if(item is null) {
            _logger?.LogInfo($"Applying theme {theme.name}");
            ThemeSystem.ApplyTheme(id);
        }
        else {
            _logger?.LogInfo(
                $"Applying theme {theme.name} to item {item.name} (ID {item.data.id}, GUID {item.data.guid})");
            PropertyInfo currentTheme = AccessTools.Property(typeof(ThemeSystem), nameof(ThemeSystem.CurrentTheme));
            ThemeSystem.Theme savedTheme = (ThemeSystem.Theme)currentTheme.GetValue(null);
            currentTheme.SetValue(null, theme);
            ThemeSystem.ApplyCurrentThemeToItem(item);
            currentTheme.SetValue(null, savedTheme);
        }
        return true;
    }

    public static void RegisterTheme(string path) {
        string name = Path.GetFileNameWithoutExtension(path);
        _logger?.LogInfo($"Parsing custom theme '{name}'");

        try {
            StreamReader stream = File.OpenText(path);
            CustomTheme customTheme = (CustomTheme)new XmlSerializer(typeof(CustomTheme)).Deserialize(stream);
            stream.Close();

            if(!TryGetTheme(customTheme.baseTheme, out ThemeSystem.Theme baseTheme))
                throw new InvalidDataException($"Theme {customTheme.baseTheme} doesn't exist");
            ThemeSystem.Theme theme = CustomTheme.CloneTheme(baseTheme);
            theme.name = name;
            customTheme.ApplyThemeOverrides(ref theme, themes);
            customTheme.ApplyValueOverrides(ref theme);

            RegisterTheme(theme);
        }
        catch(Exception ex) {
            _logger?.LogError($"Failed to parse custom theme '{name}':");
            _logger?.LogError(ex);
        }
    }

    public static void RegisterTheme(ThemeSystem.Theme theme) => RegisterTheme(theme.name, theme);
    public static void RegisterTheme(string name, ThemeSystem.Theme theme) {
        _logger?.LogInfo($"Registering theme {name}");
        themes.Add(name, theme);
        UpdateProphecyDropdown();
    }

    public static bool TryGetTheme(string name, out ThemeSystem.Theme theme) =>
        themes.TryGetValue(name, out theme) || int.TryParse(name, out int id) && TryGetTheme(id, out theme);

    public static bool TryGetTheme(int id, out ThemeSystem.Theme theme) {
        List<ThemeSystem.Theme> themes =
            (List<ThemeSystem.Theme>)AccessTools.Field(typeof(ThemeSystem), "themes").GetValue(_themeSystem);
        if(id >= 0 && id < themes.Count) {
            theme = themes[id];
            return true;
        }

        theme = default;
        return false;
    }

    private static void UpdateProphecyDropdown() {
        CustomThemeProphecy.themeName_Options = themes.Keys.ToArray();
        CustomThemeProphecy.themeName_DisplayValues = CustomThemeProphecy.themeName_Options;
    }
}
