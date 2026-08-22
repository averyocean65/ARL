using System;
using System.Reflection;
using HarmonyLib;
using Steamworks;
using UnityEngine;

namespace ARL.Upgrades;

public abstract class UpgradeBehaviour : MonoBehaviour {
	public string guid = "author.mod.upgrade";
	public string displayName = "My Upgrade";
	
	protected ItemToggle _toggle;
	protected ItemUpgrade _itemUpgrade;
	
	private FieldInfo _photonIdField;

	protected virtual void Awake() {
		_toggle = GetComponent<ItemToggle>();
		_itemUpgrade = GetComponent<ItemUpgrade>();
		_itemUpgrade.upgradeEvent.AddListener(Upgrade);
		
		GetDataFields();
	}

	private void GetDataFields() {
		_photonIdField = AccessTools.Field(typeof(ItemToggle), "playerTogglePhotonID");
	}

	protected virtual void Upgrade() {
		string steamId = "";

		try {
			int photonId = (int)_photonIdField.GetValue(_toggle);
			PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromPhotonID(photonId);
			steamId = SemiFunc.PlayerGetSteamID(playerAvatar);
		}
		catch {
			steamId = SteamClient.SteamId.ToString();
		}

		UpgradeManager.PerformUpgrade(steamId, GetUpgradeInfo(), PerformUpgrade);
	}

	protected virtual CustomUpgrade GetUpgradeInfo() {
		return UpgradeManager.Instance.RegisterOrFetchUpgrade(guid, displayName);
	}
	
	protected abstract void PerformUpgrade(bool isInLevel);
}