using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RG.Handlers;
using UnityEngine.EventSystems;

namespace RG.Loader
{
    public class MainMenuUI : UIObject
    {
        public Button startGameButton;

        public void OnEnable()
        {
            
            if (TryGetComponent(out IExtension extension))
            {
                extension.RunExtension();
            }
            startGameButton.onClick.AddListener(OnClickStartGameButton);
        }
        private void OnDisable()
        {
            startGameButton.onClick.RemoveListener(OnClickStartGameButton);
        }
        internal void OnClickStartGameButton()
        {
            RoosterSound.PlayButtonSound();
            RoosterHaptic.Selection();
            
            RoosterAnalyticSender.SendStartEvent();
            RoosterHub.Central.OnGameStartedHandler?.Invoke();
        }
    }
}