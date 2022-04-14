using System;
using UnityEngine;

namespace RG.Handlers
{
    public static class RoosterEventHandler
    {   
        public static Action<bool> OnEndGameHandler;
        
        public static Action OnClickedNextLevelButton;
        public static Action OnClickedRestartLevelButton;

        public static Action OnWinGame;
        public static Action OnFailGame;
        
        public static Action<bool> OnShowTransition;
        public static Action<int,bool> OnCollectCoin;
        public static Action OnUpdateCoinText;
    }
}