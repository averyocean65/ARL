using System;
using ARL.Upgrades;

namespace UpgradeTest;

public class ItemTestUpgrade : UpgradeBehaviour {
	private CustomUpgrade _customUpgrade;
	
	private void Start() {
		_customUpgrade = UpgradeManager.Instance.RegisterUpgrade("averyocean65.arlupgradetest.testone", "Test 1");
	}

	protected override CustomUpgrade GetUpgradeInfo() {
		return _customUpgrade;
	}
	
	protected override void PerformUpgrade(bool isInLevel) {
		UpgradeTest.Logger.LogInfo($"Applied upgrade! Is in level: {isInLevel}");
	}
}