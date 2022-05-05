using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using RG.Handlers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartingUpgradeArea : MonoBehaviour
{
    public UpgradeArea upgradeArea;

    [HorizontalLine(2)] public Image upgradeImage;
    public TextMeshProUGUI upgradeNameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI currentUpgradeNoText;
    public GameObject notEnoughMoneyImage;
    
    private const string _UGVALUE = "_UPGRADE_COST_";
    private const string _UGNO = "_UPGRADE_NO_";
    private Button myUpgradeButton => GetComponent<Button>();
    

    private void Awake()
    {
        if (GetUpgradeValues(_UGNO) == 0)
        {
            SetUpgradeValues(_UGNO, 1);
        }
    }

    private void OnEnable()
    {
        CheckPurchase();
        RoosterEventHandler.OnGameCoinChanged += CheckPurchase;
        myUpgradeButton.onClick.AddListener(OnClickMe);
        InitUpgradeArea();
    }

    private void OnDisable()
    {
        RoosterEventHandler.OnGameCoinChanged -= CheckPurchase;
        myUpgradeButton.onClick.RemoveListener(OnClickMe);
    }

    private void CheckPurchase()
    {
        if (upgradeArea.startingCost + GetUpgradeValues(_UGVALUE) <= RoosterHub.Central.GetIncome())
        {
            notEnoughMoneyImage.SetActive(false);
            myUpgradeButton.interactable = true;
        }
        else
        {
            notEnoughMoneyImage.SetActive(true);
            myUpgradeButton.interactable = false;
        }
    }

    private void OnClickMe()
    {
        var compResult = string.Equals(upgradeArea.upgradeName, upgradeNameText.text);
        if (!compResult) return;

        if (!RoosterHub.Central.SpendIncome(upgradeArea.startingCost + GetUpgradeValues(_UGVALUE))) return;
        
        RoosterHub.Haptic.Selection();
        SetUpgradeValues(_UGNO, GetUpgradeValues(_UGNO) + 1);
        SetUpgradeValues(_UGVALUE, GetUpgradeValues(_UGVALUE) + upgradeArea.costPerUpgrade);
        InitUpgradeArea();

        SendDTO(upgradeArea.upgradeName, GetUpgradeValues(_UGNO));
    }


    private void InitUpgradeArea()
    {
        upgradeImage.sprite = upgradeArea.upgradeImage;
        upgradeNameText.text = upgradeArea.upgradeName;
        costText.text = (upgradeArea.startingCost + GetUpgradeValues(_UGVALUE)).ToString();
        currentUpgradeNoText.text = GetUpgradeValues(_UGNO).ToString();
    }


    private void SetUpgradeValues(string saveName, int value)
    {
        PlayerPrefs.SetInt(saveName + upgradeArea.upgradeName, value);
    }

    private int GetUpgradeValues(string saveName)
    {
        return PlayerPrefs.GetInt(saveName + upgradeArea.upgradeName);
    }

    private void SendDTO(string upgradeName, int upgradeNo)
    {
        DTO data = new DTO
        {
            upgradeName = upgradeName,
            upgradeNo = upgradeNo
        };
        StartUpgrades.OnUpgradePurchased?.Invoke(data);
    }

    public class DTO
    {
        public string upgradeName;
        public int upgradeNo;
    }
}

[System.Serializable]
public class UpgradeArea
{
    public string upgradeName;
    public Sprite upgradeImage;
    public int startingCost;
    public int costPerUpgrade;
}