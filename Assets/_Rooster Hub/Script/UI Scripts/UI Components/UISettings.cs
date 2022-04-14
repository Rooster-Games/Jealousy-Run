using System;
using System.Collections;
using System.Collections.Generic;
using RG.Core;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    public Button hapticButton;
    public Button soundButton;

    private void OnEnable()
    {
        hapticButton.onClick.AddListener(ToogleHaptic);
        soundButton.onClick.AddListener(SoundButton);
    }

    private void OnDisable()
    {
        hapticButton.onClick.RemoveListener(ToogleHaptic);
        soundButton.onClick.RemoveListener(SoundButton);
    }

    private void SoundButton()
    {
        GamePrefs.SoundStatus = GamePrefs.SoundStatus == 0 ? 1 : 0;
        GameSettings.SoundOption(GamePrefs.SoundStatus != 0);
    }

    private void ToogleHaptic()
    {
        GamePrefs.HapticStatus = GamePrefs.HapticStatus == 0 ? 1 : 0;
        GameSettings.HapticOption(GamePrefs.HapticStatus != 0);
    }
}