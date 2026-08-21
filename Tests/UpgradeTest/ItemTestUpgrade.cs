using ARL.Upgrades;

namespace UpgradeTest;

public class ItemTestUpgrade : UpgradeBehaviour {
	protected override CustomUpgrade GetUpgradeInfo() {
		return RegisteredUpgrades.TestOne;
	}
	
	protected override void PerformUpgrade(bool isInLevel) {
		UpgradeTest.Logger.LogInfo($"Applied upgrade! Is in level: {isInLevel}");
	}
}