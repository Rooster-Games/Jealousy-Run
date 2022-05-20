using System;
using RG.Core;
using RG.Handlers;

namespace RoosterHub
{
    public static class Central
    {
        public static Action OnGameStartedHandler;
        public static Action OnTransectionStart;
        public static Action OnTransectionComplete;

        public static int GetLevelNo()
        {
            var levelNo = GamePrefs.LevelNo + 1;
            return levelNo;
        }

        public static int GetIncome()
        {
            var coin = GamePrefs.GameCoin;
            return coin;
        }

        public static void SetIncome(int collectedCoin, bool updateRealtime)
        {
            RoosterEventHandler.OnCollectCoin?.Invoke(collectedCoin, updateRealtime);
        }

        public static bool SpendIncome(int spendValue)
        {
            var coin = GamePrefs.GameCoin;
            if (spendValue <= coin)
            {
                RoosterEventHandler.OnCollectCoin?.Invoke(-spendValue, true);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void Win()
        {
            RoosterEventHandler.OnWinGame?.Invoke();
        }

        public static void Fail()
        {
            RoosterEventHandler.OnFailGame?.Invoke();
        }
    }
}