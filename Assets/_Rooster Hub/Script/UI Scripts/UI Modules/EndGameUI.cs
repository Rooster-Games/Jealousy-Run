using System;
using System.Collections;
using System.Collections.Generic;
using RG.Handlers;
using UnityEngine;

namespace RG.Loader
{
    public class EndGameUI : UIObject
    {
        public GameObject WinPanel;
        public GameObject FailPanel;

        private void OnEnable()
        {
            RoosterEventHandler.OnEndGameHandler += OpenPanel;
        }

        private void OnDisable()
        {
            RoosterEventHandler.OnEndGameHandler -= OpenPanel;
            ClosePanels();
        }

        void OpenPanel(bool status)
        {
            if (status)
                WinPanel.SetActive(true);
            else
                FailPanel.SetActive(true);
        }

        void ClosePanels()
        {
            WinPanel.SetActive(false);
            FailPanel.SetActive(false);
        }

    }    
}

