using System;
using System.Collections.Generic;
using System.Reflection;
using ARL.Utils;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

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

	private void Start() {
		SceneManager.sceneLoaded += (arg0, mode) => {
			foreach (var kvp in DictOfDicts) {
				if (!kvp.Key.StartsWith(Constants.UpgradeGuidPrefix)) {
					continue;
				}
				
				ARL.Logger.LogInfo("Upgrade Key: " + kvp.Key);
			}
		};
	}

	/// <summary>
	/// Registers or fetches an upgrade in the StatsManager.
	/// </summary>
	/// <param name="upgradeGuid">The GUID of the upgrade, recommended format is "author.mod.title"</param>
	/// <param name="displayName">The name that should show up next to the player map in-game when consuming the upgrade.</param>
	/// <returns>A structure with all the necessary information regarding the custom upgrade.</returns>
	public CustomUpgrade RegisterOrFetchUpgrade(string upgradeGuid, string displayName) {
		string patchedUpgradeGuid = $"{Constants.UpgradeGuidPrefix}{upgradeGuid}"; // required so R.E.P.O shows the upgrade in the upgrade list... thanks semiwork.
		if (upgradeGuid.StartsWith(Constants.UpgradeGuidPrefix)) {
			patchedUpgradeGuid = upgradeGuid;
		}
		
		if (_registeredUpgrades.TryGetValue(patchedUpgradeGuid, out var upgrade)) {
			return upgrade;
		}

		Dictionary<string, int> upgradeDict = new Dictionary<string, int>();
		if(DictOfDicts.TryGetValue(patchedUpgradeGuid, out var dict)) {
			upgradeDict = dict;
		}
		else {
			DictOfDicts.Add(patchedUpgradeGuid, upgradeDict);
		}
		
		CustomUpgrade output = new CustomUpgrade {
			Guid = upgradeGuid,
			DisplayName = displayName,
			UpgradeDictionary = upgradeDict,
			UpgradeInfo = new StatsManager.UpgradeInfo {
				displayName = displayName,
				displayNameLocalized = null
			}
		};

		StatsManager.instance.upgradesInfo.TryAdd(patchedUpgradeGuid, output.UpgradeInfo);
		
		_registeredUpgrades.Add(patchedUpgradeGuid, output);
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