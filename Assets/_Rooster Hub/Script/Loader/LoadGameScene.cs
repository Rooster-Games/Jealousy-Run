using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RG.Loader
{
    public class LoadGameScene : MonoBehaviour,ILoader
    {
        public void Load()
        {
            ILooper loopType = GetComponent<ILooper>();
            if (loopType == null)
            {
                Debug.LogError("LOOP SİSTEMİ EKLEYİNİZ...");
                //return;
            }
            else
            {
                loopType.LoadLevel();
            }
        }
    }
}