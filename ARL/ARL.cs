using ARL.Upgrades;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace ARL;

[BepInPlugin("averyocean65.ARL", "ARL", "1.0")]
[BepInDependency("REPOLib", BepInDependency.DependencyFlags.HardDependency)]
internal class ARL : BaseUnityPlugin
{
    internal static ARL Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger => Instance._logger;
    private ManualLogSource _logger => base.Logger;
    internal Harmony? Harmony { get; set; }

    private GameObject _upgradeManager = null!;

    private void Awake()
    {
        Instance = this;
        
        // Prevent the plugin from being deleted
        gameObject.transform.parent = null;
        gameObject.hideFlags = HideFlags.HideAndDontSave;

        Patch();

        Logger.LogInfo($"{Info.Metadata.GUID} v{Info.Metadata.Version} has loaded!");

        _upgradeManager = new GameObject("Custom Upgrade Manager");
        _upgradeManager.hideFlags = HideFlags.HideAndDontSave;
        _upgradeManager.AddComponent<UpgradeManager>();
    }

    internal void Patch()
    {
        Harmony ??= new Harmony(Info.Metadata.GUID);
        Harmony.PatchAll();
    }

    internal void Unpatch()
    {
        Harmony?.UnpatchSelf();
    }
}