using System;
using System.Collections;
using System.Collections.Generic;
using RG.Core;
using RG.Handlers;
using RoosterHub;
using TMPro;
using UnityEngine;

public class CoinArea : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    private void OnEnable()
    {
        coinText.text = GamePrefs.GameCoin.ToString();
        RoosterEventHandler.OnUpdateCoinText += UpdateCoinText;
    }

    private void OnDisable()
    {
        RoosterEventHandler.OnUpdateCoinText -= UpdateCoinText;
    }

    private void UpdateCoinText()
    {
        GamePrefs.GameCoin += GamePrefs.CollectedCoin;
        GamePrefs.CollectedCoin = 0;
        coinText.text = GamePrefs.GameCoin.ToString();
        
        RoosterEventHandler.OnGameCoinChanged?.Invoke();
        RoosterLogger.Log(Central.GetIncome().ToString(),Color.cyan);
    }
    
}
