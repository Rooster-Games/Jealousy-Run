using System;
using RG.Handlers;
using RoosterHub;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Loader
{
    public class MainMenuUI : UIObject
    {
        public Button startGameButton;

        public void OnEnable()
        {
            if (GetComponentInParent<UI_Loop>().useTransition)
            {
                RoosterEventHandler.OnShowTransition?.Invoke(false);
            }

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
            Sound.PlayButtonSound();
            Haptic.Selection();

            RoosterAnalyticSender.SendStartEvent();
            Central.OnGameStartedHandler?.Invoke();
        }

    }
}