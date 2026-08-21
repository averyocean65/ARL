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

	internal static int PerformUpgrade(string steamId, CustomUpgrade upgradeInfo, Action<bool> onPerformUpgrade, int value = 1) {
		return 0; // TODO
	}
}