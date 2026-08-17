using System.Collections.Generic;

namespace ARL.Upgrades;

public struct CustomUpgrade {
	public string Guid;
	public string DisplayName;
	
	public Dictionary<string, int> UpgradeDictionary;
	public StatsManager.UpgradeInfo UpgradeInfo;
}