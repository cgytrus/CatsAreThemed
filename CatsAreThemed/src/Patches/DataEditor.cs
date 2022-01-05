using System.Reflection;

using CalApi.Patches;

using UnityEngine;

using CalDataEditor = DataEditor;

namespace CatsAreThemed.Patches;

// ReSharper disable once UnusedType.Global
internal class DataEditor : IPatch {
    public void Apply() => On.DataEditor.AddDropdown += (On.DataEditor.orig_AddDropdown orig, CalDataEditor self,
        RectTransform content, MonoBehaviour component, FieldInfo field, string nameTranslationString,
        string[] itemTranslationStrings, ref float yPosition) => nameTranslationString == "GENERIC_THEME" ?
        orig(self, content, component, field, nameTranslationString, CustomThemes.prophecyItems, ref yPosition) :
        orig(self, content, component, field, nameTranslationString, itemTranslationStrings, ref yPosition);
}