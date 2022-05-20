using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Core
{
    public class Remote : MonoBehaviour
    {
        public static Remote Instance;
        [SerializeField]
        private string levelArray;
        

        void Awake()
        {
            Instance = this;
            levelArray = ElephantSDK.RemoteConfig.GetInstance().Get("Levels", levelArray);
        }

        public string GetRemoteLevelArray()
        {
            return levelArray;
        } 
    }
}