using System;
using RG.Handlers;
using RG.Loader;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RG.Core
{
    public class TheLoop : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
            LoadScene();
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
                RoosterHub.Central.Win();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                RoosterHub.Central.Fail();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                RoosterHub.Central.SetCoin(100,false);
                
                GameObject x = RoosterParticle.SpawnCustomParticle(0,4f);
                x.transform.position=Vector3.zero;
                
                RoosterSound.PlayButtonSound();
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

        void CollectCoin(int coin,bool realtimeUpdate)
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
