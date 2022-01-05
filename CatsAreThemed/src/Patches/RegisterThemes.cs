using System.IO;

using CalApi.API;
using CalApi.Patches;

namespace CatsAreThemed.Patches;

// ReSharper disable once UnusedType.Global
public class RegisterThemes : IPatch {
    private const string RootName = "Menus";
    private const string ReadmeName = "CustomThemes-README.txt";

    public void Apply() {
        On.PolyMap.MapManager.CallMapLoadedActions += (orig, self, state, mapName) => {
            Register(Path.GetDirectoryName(Path.GetDirectoryName(PolyMap.MapManager.LastLoadedPolyMapPath)), false);
            return orig(self, state, mapName);
        };

        CustomThemes.initialized += (_, _) => UpdateProfile();
        CustomizationProfiles.profileChanged += (_, _) => UpdateProfile();

        Directory.CreateDirectory(Path.Combine(CustomizationProfiles.defaultPath, RootName));
        CreateReadme(Path.Combine(CustomizationProfiles.defaultPath, RootName, ReadmeName));
    }

    private static void CreateReadme(string path) {
        if(File.Exists(path)) return;
        File.WriteAllText(path, @"All custom themes are registered when entering the main menu.
Custom theme IDs start at 31, custom themes may have any file name with the extension `.theme`, so an example configuration would be:
1 file in the `Menus` folder:
`myCustomTheme.theme`, this theme would be loaded under theme ID 31
To set the custom theme you need to use the UI Tweaks mod");
    }

    private static void UpdateProfile() {
        string customMenusMusicPath = Path.Combine(CustomizationProfiles.currentPath!, RootName);
        Register(customMenusMusicPath, true);
    }

    private static void Register(string? worldpackPath, bool force) {
        if(!Directory.Exists(worldpackPath)) return;
        if(!force && CustomThemes.currentlyLoadedForWorldpack == worldpackPath) return;
        CustomThemes.UnregisterAll();
        CustomThemes.logger?.LogInfo($"Registering modded themes for world pack {worldpackPath}");
        foreach(string file in Directory.GetFiles(worldpackPath))
            if(Path.GetExtension(file) == ".theme")
                CustomThemes.Register(file);
        CustomThemes.UpdateSelectors();
        CustomThemes.currentlyLoadedForWorldpack = worldpackPath;
    }
}
