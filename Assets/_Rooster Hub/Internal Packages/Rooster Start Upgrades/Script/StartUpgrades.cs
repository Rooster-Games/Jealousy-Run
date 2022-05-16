using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoosterHub
{
    public class StartUpgrades : MonoBehaviour
    {
        public static Action<StartingUpgradeArea.DTO> OnUpgradePurchased;
        private static List<StartingUpgradeArea> _upgrades;

        private void Awake()
        {
            _upgrades = GetComponentsInChildren<StartingUpgradeArea>().ToList();
        }

        public static int GetUpgradeLevel(string upgradeName)
        {
            foreach (var item in _upgrades)
            {
                if (item.upgradeArea.upgradeName == upgradeName)
                {
                    return item.GetUpgradeLevel();
                }
            }

            return 0;
        }
    }
}