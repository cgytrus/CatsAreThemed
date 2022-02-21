using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using CalApi.API;

using HarmonyLib;

using PolyMap;

using ProphecySystem;

using UnityEngine;

namespace CatsAreThemed;

[DataEditorComponent("Custom Theme Prophecy")]
[DataEditorComponentControlButtons(
    new[] { nameof(MoveProphecyUp), nameof(MoveProphecyDown), nameof(DeleteProphecy) },
    new[] { "ARROW_UP", "ARROW_DOWN", "REMOVE" })]
public class CustomThemeProphecy : BaseProphecy {
    private static readonly Func<bool, List<Item?>?, Item?> getProminentItemInFrontOfMouse =
        (Func<bool, List<Item?>?, Item?>)Delegate.CreateDelegate(typeof(Func<bool, List<Item?>?, Item?>),
            AccessTools.Method(typeof(ItemManager), "GetProminentItemInFrontOfMouse"));

    [DataEditorStringDropdown("GENERIC_THEME")]
    [SerializeField] private string themeName = "";
    // ReSharper disable once InconsistentNaming
    [IgnoreWhenSaving] public static string[] themeName_Options = Array.Empty<string>();
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once NotAccessedField.Global
    [IgnoreWhenSaving] public static string[] themeName_DisplayValues = Array.Empty<string>();

#pragma warning disable CS0649
#pragma warning disable CS0169
    [DataEditorDropdown("Fallback Theme",
        new string[] {
            "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME1", "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME2",
            "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME3", "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME4",
            "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME5", "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME6",
            "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME7", "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME8",
            "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME9", "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME10",
            "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME11", "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME12",
            "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME13", "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME14",
            "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME15", "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME16",
            "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME17", "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME18",
            "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME19", "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME20",
            "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME21", "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME22",
            "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME23", "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME24",
            "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME25", "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME26",
            "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME27", "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME28",
            "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME29", "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME30",
            "EDITOR_ROOMSETTINGS_ROOMTHEME_DROPDOWN_THEME31", "Don't change"
        })]
    // ReSharper disable once NotAccessedField.Local
    [SerializeField] private int themeID;
#pragma warning restore CS0649
#pragma warning restore CS0169

    [SerializeField] public string connectedItemGuid = string.Empty;

    [DataEditorInverseConditional(nameof(connectedItemGuid), "")]
    [DataEditorStringDisplay("Apply To")]
    // ReSharper disable once NotAccessedField.Local
    [NonSerialized] private string? _connectedItemName;

    private void Start() {
        Item? item = ItemManager.GetItemWithGUID(connectedItemGuid);
        _connectedItemName = item ? item!.name : null;
    }

    public override IEnumerator Performer(Prophet prophet, int index) {
        CustomThemes.TryApplyTheme(themeName, ItemManager.GetItemWithGUID(connectedItemGuid));
        yield break;
    }

    [DataEditorButton("EDITOR_DATAEDITOR_BUTTON_CONNECTTOITEM_BUTTON_LABEL")]
    // ReSharper disable once UnusedMember.Local
    private void ConnectToClickedItem() => StartCoroutine(ConnectToClickedItemCoroutine());

    private IEnumerator ConnectToClickedItemCoroutine() {
        ItemManager.AllowSelecting = false;
        while(!Input.GetMouseButtonUp(0)) yield return null;
        RoomEditorToast.ShowToast("EDITOR_DATAEDITOR_BUTTON_TOAST_CONNECTTOITEMSTART");
        while(!Input.GetMouseButton(0)) yield return null;
        Item? itemInFrontOfMouse = getProminentItemInFrontOfMouse(false, null);
        ItemManager.AllowSelecting = true;
        if(itemInFrontOfMouse) {
            connectedItemGuid = itemInFrontOfMouse ? itemInFrontOfMouse!.data.guid : string.Empty;
            _connectedItemName = itemInFrontOfMouse ? itemInFrontOfMouse!.name : null;
            DataEditor.EditItem(GetComponent<Item>());
            RoomEditorToast.ShowToast("EDITOR_DATAEDITOR_BUTTON_TOAST_CONNECTTOITEMEND");
        }
        else {
            connectedItemGuid = string.Empty;
            DataEditor.EditItem(GetComponent<Item>());
            RoomEditorToast.ShowToast("EDITOR_DATAEDITOR_BUTTON_TOAST_CONNECTTOITEMFAIL");
        }
    }

    [DataEditorInverseConditional(nameof(connectedItemGuid), "")]
    [DataEditorButton("EDITOR_DATAEDITOR_BUTTON_DISCONNECTITEM_BUTTON_LABEL")]
    // ReSharper disable once UnusedMember.Local
    private void DisconnectConnectedItem() {
        connectedItemGuid = string.Empty;
        DataEditor.EditItem(GetComponent<Item>());
        RoomEditorToast.ShowToast("EDITOR_DATAEDITOR_BUTTON_TOAST_DISCONNECTITEM");
    }
}
