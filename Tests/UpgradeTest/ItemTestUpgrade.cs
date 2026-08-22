using System.Linq;
using ARL.Upgrades;

namespace UpgradeTest;

public class ItemTestUpgrade : UpgradeBehaviour {
	protected override CustomUpgrade GetUpgradeInfo() {
		return UpgradeManager.Instance.RegisterOrFetchUpgrade("averyocean65.upgradetest.testone", "Test One");
	}
	
	protected override void PerformUpgrade(bool isInLevel) {
		UpgradeTest.Logger.LogInfo($"Applied upgrade! Is in level: {isInLevel}; Amount: {GetUpgradeInfo()
			.UpgradeDictionary
			.First(x => x.Value > 0)
		}");
	}
}