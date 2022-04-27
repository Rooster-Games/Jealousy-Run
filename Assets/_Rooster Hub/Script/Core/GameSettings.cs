using System;
using System.Collections;
using System.Collections.Generic;
using Lofelt.NiceVibrations;
using UnityEngine;
using NaughtyAttributes;
using TMPro;


namespace RG.Core
{
    public class GameSettings : MonoBehaviour
    {
        
        public static GameSettings Instance;
        [HorizontalLine(2.5f, EColor.Yellow)]
        [Range(0.0f,5.5f)]
        [BoxGroup("Game Settings")]
        public float winPanelDelay;
        [BoxGroup("Game Settings")]
        [Range(0.0f,5.5f)]
        public float failPanelDelay;
        
        [HorizontalLine(3.0f, EColor.Yellow)]
        [BoxGroup("UI Settings")]
        public TMP_FontAsset gameFont;
        [BoxGroup("UI Settings")]
        public Sprite transitionLogo;

        [BoxGroup("UI Settings")] public bool showLogs;
        
        [Button]
        void ChangeAllFonts()
        {
            foreach (var item in GetComponentsInChildren<TextMeshProUGUI>(true))
            {
                item.font = gameFont;
            }
            
        }
        private void Awake()
        {
            Instance = this;
            
            HapticOption(GamePrefs.HapticStatus != 0);
            SoundOption(GamePrefs.SoundStatus != 0);
        }

        public static void HapticOption(bool status)
        {
            FindObjectOfType<HapticReceiver>().hapticsEnabled = !status;
        }

        public static void SoundOption(bool status)
        {
            AudioListener.pause = status;
        }
    }
}