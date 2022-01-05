using BepInEx;

using CalApi.API;

namespace CatsAreThemed;

[BepInPlugin("mod.cgytrus.plugins.calCustomThemes", PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
internal class CalCustomThemesPlugin : BaseUnityPlugin {
    public CalCustomThemesPlugin() => Patches.CustomThemes.logger = Logger;

    private void Awake() {
        Logger.LogInfo("Applying patches");
        Util.ApplyAllPatches();
    }
}