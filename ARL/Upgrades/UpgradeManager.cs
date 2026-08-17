using System;
using ARL.Utils;
using UnityEngine;

namespace ARL.Upgrades;

public class UpgradeManager : MonoSingleton<UpgradeManager> {
	private void Start() {
		ARL.Logger.LogInfo($"Spawned {nameof(UpgradeManager)}.");
	}
}