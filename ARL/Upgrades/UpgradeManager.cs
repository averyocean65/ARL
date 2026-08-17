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

	private SortedDictionary<string, Dictionary<string, int>> DictOfDicts =>
		(SortedDictionary<string, Dictionary<string, int>>)_dictOfDictsField.GetValue(StatsManager.instance);
	
	private void GetFields() {
		_dictOfDictsField = AccessTools.Field(typeof(SortedDictionary<string, Dictionary<string, int>>),
			"dictionaryOfDictionaries");
	}
	
	private void Start() {
		ARL.Logger.LogInfo($"Spawned {nameof(UpgradeManager)}.");
	}

	private LocalizedAsset GetLocalizedString(string upgradeGuid) {
		LocalizedAsset asset = ScriptableObject.CreateInstance<LocalizedAsset>();
		asset.stringReference = new LocalizedString(Constants.UpgradeTableGuid, upgradeGuid);
		return asset;
	}
	
	public CustomUpgrade RegisterUpgrade(string upgradeGuid, string displayName) {
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
		return output;
	}
}