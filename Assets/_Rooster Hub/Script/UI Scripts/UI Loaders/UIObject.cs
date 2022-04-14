using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using RG.Core;
using UnityEngine;

namespace RG.Loader
{
    
    public abstract class UIObject : MonoBehaviour
    {
        public bool useUnlockQueue;
        [ShowIf("useUnlockQueue")]
        public int[] unlockQueue;

        public bool useTransition;
        
        public void OpenMenu()
        {
            gameObject.SetActive(true);
        }

        public void CloseMenu()
        {
            gameObject.SetActive(false);
        }

        public bool IsUnlockableCanvas()
        {
            var result = unlockQueue.Contains(GamePrefs.LevelNo);
            return result;
        }
    }
}