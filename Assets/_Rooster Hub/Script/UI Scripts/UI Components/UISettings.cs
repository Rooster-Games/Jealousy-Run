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

    public Color openColor;
    public Color closeColor;

    private void OnEnable()
    {
        hapticButton.onClick.AddListener(ToogleHaptic);
        soundButton.onClick.AddListener(SoundButton);
        CheckButtonColors();
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

        CheckButtonColors();
    }

    private void ToogleHaptic()
    {
        GamePrefs.HapticStatus = GamePrefs.HapticStatus == 0 ? 1 : 0;
        GameSettings.HapticOption(GamePrefs.HapticStatus != 0);

        CheckButtonColors();
    }

    void CheckButtonColors()
    {
        soundButton.GetComponent<Image>().color = GamePrefs.SoundStatus == 0 ? openColor : closeColor;
        hapticButton.GetComponent<Image>().color = GamePrefs.HapticStatus == 0 ? openColor : closeColor;
    }
}