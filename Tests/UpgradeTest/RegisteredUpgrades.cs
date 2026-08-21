using ARL.Upgrades;

namespace UpgradeTest;

public static class RegisteredUpgrades {
	public static CustomUpgrade TestOne { get; private set; }
	
	public static void InitUpgrades() {
		TestOne = UpgradeManager.Instance.RegisterUpgrade("averyocean65.testupgrade.testone", "Test One");
	}
}