using System;
using System.Data.Common;
using RG.Handlers;
using RG.Loader;
using RoosterHub;
using UnityEngine;

namespace RG.Core
{
    public class TheLoop : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
            LoadScene();
        }

        private void Start()
        {
           
        }

        private void OnEnable()
        {
            RoosterEventHandler.OnClickedNextLevelButton += LoadNewLevel;
            RoosterEventHandler.OnClickedRestartLevelButton += RestartLevel;
            RoosterEventHandler.OnCollectCoin += CollectCoin;
            RoosterEventHandler.OnWinGame += WinLevel;
            RoosterEventHandler.OnFailGame += FailLevel;
        }

       
        private void OnDisable()
        {
            RoosterEventHandler.OnClickedNextLevelButton -= LoadNewLevel;
            RoosterEventHandler.OnClickedRestartLevelButton -= RestartLevel;
            RoosterEventHandler.OnCollectCoin -= CollectCoin;
            RoosterEventHandler.OnWinGame -= WinLevel;
            RoosterEventHandler.OnFailGame -= FailLevel;
        }
        
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Central.Win();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                Central.Fail();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Central.SetIncome(100, true);
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                Debug.LogError(Central.SpendIncome(30));
            }
        }

        void WinLevel()
        {
            RoosterAnalyticSender.SendWinEvent();

            IncreaseLevel();

            UI_Loop.OnChangeUI?.Invoke(true);
            RoosterEventHandler.OnEndGameHandler?.Invoke(true);
        }

        void FailLevel()
        {
            RoosterAnalyticSender.SendFailEvent();

            UI_Loop.OnChangeUI?.Invoke(true);
            RoosterEventHandler.OnEndGameHandler?.Invoke(false);
        }

        void CollectCoin(int coin, bool realtimeUpdate)
        {
            if (realtimeUpdate)
            {
                GamePrefs.CollectedCoin += coin;
                RoosterEventHandler.OnUpdateCoinText?.Invoke();
            }
            else
            {
                GamePrefs.CollectedCoin += coin;
            }
        }

        private void LoadScene()
        {
            var _loadType = GetComponent<ILoader>();
            if (_loadType == null)
            {
                _loadType = gameObject.AddComponent<LoadGameScene>();
                _loadType.Load();
            }
            else
            {
                _loadType.Load();
            }
        }

        private void IncreaseLevel()
        {
            GamePrefs.LevelNo++;
        }

        private void LoadNewLevel()
        {
            LoadScene();
        }

        private void RestartLevel()
        {
            RoosterAnalyticSender.SendRestartEvent();
            LoadScene();
        }
    }
}