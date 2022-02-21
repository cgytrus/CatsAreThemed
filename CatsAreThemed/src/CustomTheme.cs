using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.PostProcessing;
// ReSharper disable UnassignedField.Global

namespace CatsAreThemed;

[Serializable]
public struct CustomTheme {
    public string baseTheme;
    public CustomThemeThemeOverrides? themeOverrides;
    public CustomThemeValueOverrides? valueOverrides;

    public static ThemeSystem.Theme CloneTheme(ThemeSystem.Theme theme) {
        ThemeSystem.Theme newTheme = new() {
            name = theme.name,
            profile = ScriptableObject.CreateInstance<PostProcessingProfile>(),

            primaryColor = theme.primaryColor,
            secondaryColor = theme.secondaryColor,
            tertiaryColor = theme.tertiaryColor,

            primaryTexture = theme.primaryTexture,
            primaryTextureTiling = theme.primaryTextureTiling,

            useLine = theme.useLine,
            lineTexture = theme.lineTexture,
            lineTiling = theme.lineTiling,
            lineOffset = theme.lineOffset,
            lineColor = theme.lineColor,
            lineWidth = theme.lineWidth,
            lineOrderInLayer = theme.lineOrderInLayer,

            useSecondaryLine = theme.useSecondaryLine,
            secondaryLineTexture = theme.secondaryLineTexture,
            secondaryLineTiling = theme.secondaryLineTiling,
            secondaryLineOffset = theme.secondaryLineOffset,
            secondaryLineColor = theme.secondaryLineColor,
            secondaryLineWidth = theme.secondaryLineWidth,
            secondaryLineOrderInLayer = theme.secondaryLineOrderInLayer,

            mainShapeRepeatableObjectId = theme.mainShapeRepeatableObjectId,
            mainShapeRepeatableObjectRepeateDistance = theme.mainShapeRepeatableObjectRepeateDistance,
            mainShapeRepeatableObjectSpawnChance = theme.mainShapeRepeatableObjectSpawnChance,

            backgroundColor = theme.backgroundColor,
            fogColor1 = theme.fogColor1,
            fogColor2 = theme.fogColor2,
            gradientColor = theme.gradientColor,
            disableGradientSpin = theme.disableGradientSpin,
            backgroundParticles = theme.backgroundParticles
        };

        // creating a new model object only for models that can be customized
        newTheme.profile.debugViews = theme.profile.debugViews;
        newTheme.profile.fog = theme.profile.fog;
        newTheme.profile.antialiasing = theme.profile.antialiasing;
        newTheme.profile.ambientOcclusion = theme.profile.ambientOcclusion;
        newTheme.profile.screenSpaceReflection = theme.profile.screenSpaceReflection;
        newTheme.profile.depthOfField = theme.profile.depthOfField;
        newTheme.profile.motionBlur = theme.profile.motionBlur;
        newTheme.profile.eyeAdaptation = theme.profile.eyeAdaptation;
        newTheme.profile.bloom = new BloomModel {
            enabled = theme.profile.bloom.enabled,
            settings = theme.profile.bloom.settings
        };
        newTheme.profile.colorGrading = new ColorGradingModel {
            enabled = theme.profile.colorGrading.enabled,
            settings = theme.profile.colorGrading.settings
        };
        newTheme.profile.userLut = theme.profile.userLut;
        newTheme.profile.chromaticAberration = new ChromaticAberrationModel {
            enabled = theme.profile.chromaticAberration.enabled,
            settings = theme.profile.chromaticAberration.settings
        };
        newTheme.profile.grain = new GrainModel {
            enabled = theme.profile.grain.enabled,
            settings = theme.profile.grain.settings
        };
        newTheme.profile.vignette = new VignetteModel {
            enabled = theme.profile.vignette.enabled,
            settings = theme.profile.vignette.settings
        };
        newTheme.profile.dithering = theme.profile.dithering;

        return newTheme;
    }

    // ReSharper disable once CognitiveComplexity
    public void ApplyThemeOverrides(ref ThemeSystem.Theme theme, IReadOnlyDictionary<string, ThemeSystem.Theme> themes) {
        if(!this.themeOverrides.HasValue) return;
        CustomThemeThemeOverrides themeOverrides = this.themeOverrides.Value;

        if(themeOverrides.primaryColor is not null)
            theme.primaryColor = themes[themeOverrides.primaryColor].primaryColor;
        if(themeOverrides.secondaryColor is not null)
            theme.secondaryColor = themes[themeOverrides.secondaryColor].secondaryColor;
        if(themeOverrides.tertiaryColor is not null)
            theme.tertiaryColor = themes[themeOverrides.tertiaryColor].tertiaryColor;

        if(themeOverrides.primaryTexture is not null)
            theme.primaryTexture = themes[themeOverrides.primaryTexture].primaryTexture;
        if(themeOverrides.primaryTextureTiling is not null)
            theme.primaryTextureTiling = themes[themeOverrides.primaryTextureTiling].primaryTextureTiling;

        if(themeOverrides.useLine is not null)
            theme.useLine = themes[themeOverrides.useLine].useLine;
        if(themeOverrides.lineTexture is not null)
            theme.lineTexture = themes[themeOverrides.lineTexture].lineTexture;
        if(themeOverrides.lineTiling is not null)
            theme.lineTiling = themes[themeOverrides.lineTiling].lineTiling;
        if(themeOverrides.lineOffset is not null)
            theme.lineOffset = themes[themeOverrides.lineOffset].lineOffset;
        if(themeOverrides.lineColor is not null)
            theme.lineColor = themes[themeOverrides.lineColor].lineColor;
        if(themeOverrides.lineWidth is not null)
            theme.lineWidth = themes[themeOverrides.lineWidth].lineWidth;
        if(themeOverrides.lineOrderInLayer is not null)
            theme.lineOrderInLayer = themes[themeOverrides.lineOrderInLayer].lineOrderInLayer;

        if(themeOverrides.useSecondaryLine is not null)
            theme.useSecondaryLine = themes[themeOverrides.useSecondaryLine].useSecondaryLine;
        if(themeOverrides.secondaryLineTexture is not null)
            theme.secondaryLineTexture = themes[themeOverrides.secondaryLineTexture].secondaryLineTexture;
        if(themeOverrides.secondaryLineTiling is not null)
            theme.secondaryLineTiling = themes[themeOverrides.secondaryLineTiling].secondaryLineTiling;
        if(themeOverrides.secondaryLineOffset is not null)
            theme.secondaryLineOffset = themes[themeOverrides.secondaryLineOffset].secondaryLineOffset;
        if(themeOverrides.secondaryLineColor is not null)
            theme.secondaryLineColor = themes[themeOverrides.secondaryLineColor].secondaryLineColor;
        if(themeOverrides.secondaryLineWidth is not null)
            theme.secondaryLineWidth = themes[themeOverrides.secondaryLineWidth].secondaryLineWidth;
        if(themeOverrides.secondaryLineOrderInLayer is not null)
            theme.secondaryLineOrderInLayer =
                themes[themeOverrides.secondaryLineOrderInLayer].secondaryLineOrderInLayer;

        if(themeOverrides.mainShapeRepeatableObjectId is not null)
            theme.mainShapeRepeatableObjectId = themes[themeOverrides.mainShapeRepeatableObjectId].mainShapeRepeatableObjectId;
        if(themeOverrides.mainShapeRepeatableObjectRepeatDistance is not null)
            theme.mainShapeRepeatableObjectRepeateDistance = themes[themeOverrides.mainShapeRepeatableObjectRepeatDistance].mainShapeRepeatableObjectRepeateDistance;
        if(themeOverrides.mainShapeRepeatableObjectSpawnChance is not null)
            theme.mainShapeRepeatableObjectSpawnChance = themes[themeOverrides.mainShapeRepeatableObjectSpawnChance].mainShapeRepeatableObjectSpawnChance;

        if(themeOverrides.backgroundColor is not null)
            theme.backgroundColor = themes[themeOverrides.backgroundColor].backgroundColor;
        if(themeOverrides.fogColor1 is not null)
            theme.fogColor1 = themes[themeOverrides.fogColor1].fogColor1;
        if(themeOverrides.fogColor2 is not null)
            theme.fogColor2 = themes[themeOverrides.fogColor2].fogColor2;
        if(themeOverrides.gradientColor is not null)
            theme.gradientColor = themes[themeOverrides.gradientColor].gradientColor;
        if(themeOverrides.disableGradientSpin is not null)
            theme.disableGradientSpin = themes[themeOverrides.disableGradientSpin].disableGradientSpin;
        if(themeOverrides.backgroundParticles is not null)
            theme.backgroundParticles = themes[themeOverrides.backgroundParticles].backgroundParticles;

        if(themeOverrides.bloom is not null)
            theme.profile.bloom = themes[themeOverrides.bloom].profile.bloom;
        if(themeOverrides.colorGrading is not null)
            theme.profile.colorGrading = themes[themeOverrides.colorGrading].profile.colorGrading;
        if(themeOverrides.chromaticAberration is not null)
            theme.profile.chromaticAberration =
                themes[themeOverrides.chromaticAberration].profile.chromaticAberration;
        if(themeOverrides.grain is not null)
            theme.profile.grain = themes[themeOverrides.grain].profile.grain;
        if(themeOverrides.vignette is not null)
            theme.profile.vignette = themes[themeOverrides.vignette].profile.vignette;
    }

    // ReSharper disable once CognitiveComplexity
    public void ApplyValueOverrides(ref ThemeSystem.Theme theme) {
        if(!this.valueOverrides.HasValue) return;
        CustomThemeValueOverrides valueOverrides = this.valueOverrides.Value;

        if(valueOverrides.primaryColor.HasValue) theme.primaryColor = valueOverrides.primaryColor.Value;
        if(valueOverrides.secondaryColor.HasValue) theme.secondaryColor = valueOverrides.secondaryColor.Value;
        if(valueOverrides.tertiaryColor.HasValue) theme.tertiaryColor = valueOverrides.tertiaryColor.Value;

        if(valueOverrides.primaryTextureTiling.HasValue)
            theme.primaryTextureTiling = valueOverrides.primaryTextureTiling.Value;

        if(valueOverrides.useLine.HasValue) theme.useLine = valueOverrides.useLine.Value;
        if(valueOverrides.lineTiling.HasValue) theme.lineTiling = valueOverrides.lineTiling.Value;
        if(valueOverrides.lineOffset.HasValue) theme.lineOffset = valueOverrides.lineOffset.Value;
        if(valueOverrides.lineColor.HasValue) theme.lineColor = valueOverrides.lineColor.Value;
        if(valueOverrides.lineWidth.HasValue) theme.lineWidth = valueOverrides.lineWidth.Value;
        if(valueOverrides.lineOrderInLayer.HasValue) theme.lineOrderInLayer = valueOverrides.lineOrderInLayer.Value;

        if(valueOverrides.useSecondaryLine.HasValue) theme.useSecondaryLine = valueOverrides.useSecondaryLine.Value;
        if(valueOverrides.secondaryLineTiling.HasValue)
            theme.secondaryLineTiling = valueOverrides.secondaryLineTiling.Value;
        if(valueOverrides.secondaryLineOffset.HasValue)
            theme.secondaryLineOffset = valueOverrides.secondaryLineOffset.Value;
        if(valueOverrides.secondaryLineColor.HasValue)
            theme.secondaryLineColor = valueOverrides.secondaryLineColor.Value;
        if(valueOverrides.secondaryLineWidth.HasValue)
            theme.secondaryLineWidth = valueOverrides.secondaryLineWidth.Value;
        if(valueOverrides.secondaryLineOrderInLayer.HasValue)
            theme.secondaryLineOrderInLayer = valueOverrides.secondaryLineOrderInLayer.Value;

        if(valueOverrides.mainShapeRepeatableObjectId.HasValue)
            theme.mainShapeRepeatableObjectId = valueOverrides.mainShapeRepeatableObjectId.Value;
        if(valueOverrides.mainShapeRepeatableObjectRepeatDistance.HasValue)
            theme.mainShapeRepeatableObjectRepeateDistance = valueOverrides.mainShapeRepeatableObjectRepeatDistance.Value;
        if(valueOverrides.mainShapeRepeatableObjectSpawnChance.HasValue)
            theme.mainShapeRepeatableObjectSpawnChance = valueOverrides.mainShapeRepeatableObjectSpawnChance.Value;

        if(valueOverrides.backgroundColor.HasValue) theme.backgroundColor = valueOverrides.backgroundColor.Value;
        if(valueOverrides.fogColor1.HasValue) theme.fogColor1 = valueOverrides.fogColor1.Value;
        if(valueOverrides.fogColor2.HasValue) theme.fogColor2 = valueOverrides.fogColor2.Value;
        if(valueOverrides.gradientColor.HasValue) theme.gradientColor = valueOverrides.gradientColor.Value;
        if(valueOverrides.disableGradientSpin.HasValue)
            theme.disableGradientSpin = valueOverrides.disableGradientSpin.Value;
        if(valueOverrides.backgroundParticles.HasValue && !valueOverrides.backgroundParticles.Value)
            theme.backgroundParticles = null;

        valueOverrides.bloom?.Apply(theme.profile.bloom);
        valueOverrides.colorGrading?.Apply(theme.profile.colorGrading);
        valueOverrides.chromaticAberration?.Apply(theme.profile.chromaticAberration);
        valueOverrides.grain?.Apply(theme.profile.grain);
        valueOverrides.vignette?.Apply(theme.profile.vignette);
    }
}

[Serializable]
public struct CustomThemeThemeOverrides {
    public string? primaryColor;
    public string? secondaryColor;
    public string? tertiaryColor;

    public string? primaryTexture;
    public string? primaryTextureTiling;

    public string? useLine;
    public string? lineTexture;
    public string? lineTiling;
    public string? lineOffset;
    public string? lineColor;
    public string? lineWidth;
    public string? lineOrderInLayer;

    public string? useSecondaryLine;
    public string? secondaryLineTexture;
    public string? secondaryLineTiling;
    public string? secondaryLineOffset;
    public string? secondaryLineColor;
    public string? secondaryLineWidth;
    public string? secondaryLineOrderInLayer;

    public string? mainShapeRepeatableObjectId;
    public string? mainShapeRepeatableObjectRepeatDistance;
    public string? mainShapeRepeatableObjectSpawnChance;

    public string? backgroundColor;
    public string? fogColor1;
    public string? fogColor2;
    public string? gradientColor;
    public string? disableGradientSpin;
    public string? backgroundParticles;

    public string? bloom;
    public string? colorGrading;
    public string? chromaticAberration;
    public string? grain;
    public string? vignette;
}

[Serializable]
public struct CustomThemeValueOverrides {
    public Color? primaryColor;
    public Color? secondaryColor;
    public Color? tertiaryColor;

    public Vector2? primaryTextureTiling;

    public bool? useLine;
    public Vector2? lineTiling;
    public Vector2? lineOffset;
    public Color? lineColor;
    public float? lineWidth;
    public int? lineOrderInLayer;

    public bool? useSecondaryLine;
    public Vector2? secondaryLineTiling;
    public Vector2? secondaryLineOffset;
    public Color? secondaryLineColor;
    public float? secondaryLineWidth;
    public int? secondaryLineOrderInLayer;

    public int? mainShapeRepeatableObjectId;
    public float? mainShapeRepeatableObjectRepeatDistance;
    public float? mainShapeRepeatableObjectSpawnChance;

    public Color? backgroundColor;
    public Color? fogColor1;
    public Color? fogColor2;
    public Color? gradientColor;
    public bool? disableGradientSpin;
    public bool? backgroundParticles;

    public CustomBloomSettings? bloom;
    public CustomColorGradingSettings? colorGrading;
    public CustomChromaticAberrationSettings? chromaticAberration;
    public CustomGrainSettings? grain;
    public CustomVignetteSettings? vignette;
}

[Serializable]
public struct CustomBloomSettings {
    public bool? enabled;
    public float? intensity;
    public float? threshold;
    public float? softKnee;
    public float? radius;
    public bool? antiFlicker;

    public void Apply(BloomModel model) {
        if(enabled.HasValue) model.enabled = enabled.Value;
        BloomModel.Settings settings = model.settings;
        if(intensity.HasValue) settings.bloom.intensity = intensity.Value;
        if(threshold.HasValue) settings.bloom.threshold = threshold.Value;
        if(softKnee.HasValue) settings.bloom.softKnee = softKnee.Value;
        if(radius.HasValue) settings.bloom.radius = radius.Value;
        if(antiFlicker.HasValue) settings.bloom.antiFlicker = antiFlicker.Value;
        model.settings = settings;
    }
}

[Serializable]
public struct CustomColorGradingSettings {
    public bool? enabled;
    public float? postExposure;
    public float? temperature;
    public float? tint;
    public float? hueShift;
    public float? saturation;
    public float? contrast;
    public Vector3? red;
    public Vector3? green;
    public Vector3? blue;

    public void Apply(ColorGradingModel model) {
        if(enabled.HasValue) model.enabled = enabled.Value;
        ColorGradingModel.Settings settings = model.settings;
        if(postExposure.HasValue) settings.basic.postExposure = postExposure.Value;
        if(temperature.HasValue) settings.basic.temperature = temperature.Value;
        if(tint.HasValue) settings.basic.tint = tint.Value;
        if(hueShift.HasValue) settings.basic.hueShift = hueShift.Value;
        if(saturation.HasValue) settings.basic.saturation = saturation.Value;
        if(contrast.HasValue) settings.basic.contrast = contrast.Value;
        if(red.HasValue) settings.channelMixer.red = red.Value;
        if(green.HasValue) settings.channelMixer.green = green.Value;
        if(blue.HasValue) settings.channelMixer.blue = blue.Value;
        model.settings = settings;
    }
}

[Serializable]
public struct CustomChromaticAberrationSettings {
    public bool? enabled;
    public float? intensity;

    public void Apply(ChromaticAberrationModel model) {
        if(enabled.HasValue) model.enabled = enabled.Value;
        ChromaticAberrationModel.Settings settings = model.settings;
        if(intensity.HasValue) settings.intensity = intensity.Value;
        model.settings = settings;
    }
}

[Serializable]
public struct CustomGrainSettings {
    public bool? enabled;
    public bool? colored;
    public float? intensity;
    public float? size;
    public float? luminanceContribution;

    public void Apply(GrainModel model) {
        if(enabled.HasValue) model.enabled = enabled.Value;
        GrainModel.Settings settings = model.settings;
        if(colored.HasValue) settings.colored = colored.Value;
        if(intensity.HasValue) settings.intensity = intensity.Value;
        if(size.HasValue) settings.size = size.Value;
        if(luminanceContribution.HasValue) settings.luminanceContribution = luminanceContribution.Value;
        model.settings = settings;
    }
}

[Serializable]
public struct CustomVignetteSettings {
    public bool? enabled;
    public Color? color;
    public Vector2? center;
    public float? intensity;
    public float? smoothness;
    public float? roundness;
    public bool? rounded;

    public void Apply(VignetteModel model) {
        if(enabled.HasValue) model.enabled = enabled.Value;
        VignetteModel.Settings settings = model.settings;
        if(color.HasValue) settings.color = color.Value;
        if(center.HasValue) settings.center = center.Value;
        if(intensity.HasValue) settings.intensity = intensity.Value;
        if(smoothness.HasValue) settings.smoothness = smoothness.Value;
        if(roundness.HasValue) settings.roundness = roundness.Value;
        if(rounded.HasValue) settings.rounded = rounded.Value;
        model.settings = settings;
    }
}
