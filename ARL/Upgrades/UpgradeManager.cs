using System;
using System.Collections.Generic;
using System.Reflection;
using ARL.Utils;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Localization;

namespace ARL.Upgrades;

public class UpgradeManager : MonoSingleton<UpgradeManager> {
	private FieldInfo _dictOfDictsField = null!;
	private Dictionary<string, CustomUpgrade> _registeredUpgrades = new();
	
	private SortedDictionary<string, Dictionary<string, int>> DictOfDicts =>
		(SortedDictionary<string, Dictionary<string, int>>)_dictOfDictsField.GetValue(StatsManager.instance);
	
	private void GetFields() {
		_dictOfDictsField = AccessTools.Field(typeof(StatsManager), "dictionaryOfDictionaries");
	}

	protected override void Awake() {
		base.Awake();
		
		DontDestroyOnLoad(this);
		
		ARL.Logger.LogInfo($"Spawned {nameof(UpgradeManager)}.");
		GetFields();
	}

	private LocalizedAsset GetLocalizedString(string upgradeGuid) {
		LocalizedAsset asset = ScriptableObject.CreateInstance<LocalizedAsset>();
		asset.stringReference = new LocalizedString(Constants.UpgradeTableGuid, upgradeGuid);
		return asset;
	}
	
	/// <summary>
	/// Registers an upgrade in the StatsManager.
	/// Never call this in a frequently called function like implementations of <see cref="UpgradeBehaviour.GetUpgradeInfo()"/>
	/// </summary>
	/// <param name="upgradeGuid">The GUID of the upgrade, recommended format is "author.mod.title"</param>
	/// <param name="displayName">The name that should show up next to the player map in-game when consuming the upgrade.</param>
	/// <returns>A structure with all the necessary information regarding the custom upgrade.</returns>
	public CustomUpgrade RegisterUpgrade(string upgradeGuid, string displayName) {
		if (_registeredUpgrades.TryGetValue(upgradeGuid, out var upgrade)) {
			return upgrade;
		}
		
		CustomUpgrade output = new CustomUpgrade() {
			Guid = upgradeGuid,
			DisplayName = displayName,
			UpgradeDictionary = new Dictionary<string, int>(),
			UpgradeInfo = new StatsManager.UpgradeInfo() {
				displayName = displayName,
				displayNameLocalized = GetLocalizedString(upgradeGuid)
			}
		};
		
		DictOfDicts.Add(upgradeGuid, output.UpgradeDictionary);
		_registeredUpgrades.Add(upgradeGuid, output);
		return output;
	}

	internal static int PerformUpgrade(string steamId, CustomUpgrade upgradeInfo, Action<bool> onPerformUpgrade, int value = 1) {
		int upgradeCount = upgradeInfo.UpgradeDictionary.GetValueOrDefault(steamId, 0);
		if (value == 0) {
			return upgradeCount;
		}

		upgradeInfo.UpgradeDictionary.TryAdd(steamId, 0);
		upgradeInfo.UpgradeDictionary[steamId] += value;
		onPerformUpgrade.Invoke(SemiFunc.RunIsLevel());
		
		return upgradeInfo.UpgradeDictionary[steamId];
	}
}