using System;
using RG.Core;
using RG.Handlers;

namespace RoosterHub
{
    public static class Central
    {
        
        public static Action OnGameStartedHandler;
        
        public static int GetLevelNo()
        {
            var levelNo = GamePrefs.LevelNo + 1;
            return levelNo;
        }

        public static int GetCoin()
        {
            var coin = GamePrefs.GameCoin;
            return coin;
        }

        public static void SetCoin(int collectedCoin,bool updateRealtime)
        {
            RoosterEventHandler.OnCollectCoin?.Invoke(collectedCoin,updateRealtime);
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