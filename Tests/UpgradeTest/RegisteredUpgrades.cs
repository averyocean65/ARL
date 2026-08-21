using ARL.Upgrades;

namespace UpgradeTest;

public static class RegisteredUpgrades {
	private static CustomUpgrade? _testOneInternal = null;

	public static CustomUpgrade TestOne {
		get {
			if (_testOneInternal == null) {
				_testOneInternal =
					UpgradeManager.Instance.RegisterUpgrade("averyocean65.upgradetest.testone", "Test One");
			}

			return (CustomUpgrade)_testOneInternal;
		}
	}
}