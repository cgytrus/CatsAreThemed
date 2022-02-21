using BepInEx;

using CalApi.API;

namespace CatsAreThemed;

[BepInPlugin("mod.cgytrus.plugins.calCustomThemes", PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("mod.cgytrus.plugins.calapi", "0.2.6")]
internal class CalCustomThemesPlugin : BaseUnityPlugin {
    private void Awake() {
        CustomThemes.Setup(Logger);

        Logger.LogInfo("Applying patches");
        Util.ApplyAllPatches();

        Logger.LogInfo("Registering prophecies");
        Prophecies.RegisterProphecy<ProphecySystem.ThemeProphecy, CustomThemeProphecy>("cgytrus.theme", "THEME");
    }
}