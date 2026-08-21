using System;
using System.Reflection;
using HarmonyLib;
using Steamworks;
using UnityEngine;

namespace ARL.Upgrades;

public class UpgradeBehaviour : MonoBehaviour {
	protected ItemToggle _toggle;
	private FieldInfo _photonIdField;

	protected virtual void Awake() {
		_toggle = GetComponent<ItemToggle>();
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
		
		// TODO: call function to perform the upgrade in REPO's logic
	}
}