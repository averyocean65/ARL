using ARL.Upgrades;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace UpgradeTest;

[BepInPlugin("averyocean65.UpgradeTest", "UpgradeTest", "1.0")]
[BepInDependency("averyocean65.ARL", BepInDependency.DependencyFlags.HardDependency)]
internal class UpgradeTest : BaseUnityPlugin {
	internal static UpgradeTest Instance { get; private set; } = null!;
	internal new static ManualLogSource Logger => Instance._logger;
	private ManualLogSource _logger => base.Logger;
	internal Harmony? Harmony { get; set; }

	private void Awake() {
		Instance = this;

		// Prevent the plugin from being deleted
		this.gameObject.transform.parent = null;
		this.gameObject.hideFlags = HideFlags.HideAndDontSave;

		Patch();

		Logger.LogInfo($"{Info.Metadata.GUID} v{Info.Metadata.Version} has loaded!");

		UpgradeManager.Instance.RegisterUpgrade("averyocean65.upgradetest.testupgrade", "Test Upgrade");
	}

	internal void Patch() {
		Harmony ??= new Harmony(Info.Metadata.GUID);
		Harmony.PatchAll();
	}

	internal void Unpatch() {
		Harmony?.UnpatchSelf();
	}
}