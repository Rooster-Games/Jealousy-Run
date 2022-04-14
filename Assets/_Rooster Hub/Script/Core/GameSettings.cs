using System;
using System.Collections;
using System.Collections.Generic;
using Lofelt.NiceVibrations;
using UnityEngine;
using NaughtyAttributes;


namespace RG.Core
{
    public class GameSettings : MonoBehaviour
    {
        private void Awake()
        {
            HapticOption(GamePrefs.HapticStatus != 0);
            SoundOption(GamePrefs.SoundStatus != 0);

          
        }

        public static void HapticOption(bool status)
        {
            FindObjectOfType<HapticReceiver>().hapticsEnabled = status;
        }

        public static void SoundOption(bool status)
        {
            AudioListener.pause = status;
        }
    }
}