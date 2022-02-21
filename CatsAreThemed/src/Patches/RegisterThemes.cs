using System.IO;

using CalApi.API;
using CalApi.Patches;

namespace CatsAreThemed.Patches;

// ReSharper disable once UnusedType.Global
public class RegisterThemes : IPatch {
    private const string RootName = "Menus";
    private const string ReadmeName = "CustomThemes-README.txt";
    private const string StartingThemeName = "startingTheme.txt";

    private string? _currentlyLoadedForWorldpack;

    public void Apply() {
        On.PolyMap.MapManager.CallMapLoadedActions += (orig, self, state, mapName) => {
            string? worldpackPath =
                Path.GetDirectoryName(Path.GetDirectoryName(PolyMap.MapManager.LastLoadedPolyMapPath));
            if(_currentlyLoadedForWorldpack == worldpackPath) return orig(self, state, mapName);
            CustomThemes.ReregisterThemes();
            CustomThemes.RegisterCustomThemes(worldpackPath);
            _currentlyLoadedForWorldpack = worldpackPath;
            return orig(self, state, mapName);
        };

        CustomThemes.initialized += (_, _) => {
            _currentlyLoadedForWorldpack = null;
            UpdateProfile();
        };
        CustomizationProfiles.profileChanged += (_, _) => UpdateProfile();

        Directory.CreateDirectory(Path.Combine(CustomizationProfiles.defaultPath, RootName));
        CreateReadme(Path.Combine(CustomizationProfiles.defaultPath, RootName, ReadmeName));
    }

    private static void CreateReadme(string path) {
        if(File.Exists(path)) return;
        File.WriteAllText(path, @"All custom themes are registered when entering the main menu.
startingTheme.txt contains the theme ID or name that would be played in the menus.
Custom themes don't have numeric IDs, they may have any file name with the extension `.theme`, so an example configuration would be:
2 files in the `Menus` folder:
`startingTheme.txt`, which has *only* 'myCustomTheme' written in it
and `myCustomTheme.theme`, this theme would be set in the menus");
    }

    private static void UpdateProfile() {
        string customMenusThemesPath = Path.Combine(CustomizationProfiles.currentPath!, RootName);
        string startingThemePath = Path.Combine(customMenusThemesPath, StartingThemeName);
        if(File.Exists(startingThemePath)) CustomThemes.startingTheme = File.ReadAllText(startingThemePath);
        CustomThemes.ReregisterThemes();
        CustomThemes.RegisterCustomThemes(customMenusThemesPath);
    }
}
