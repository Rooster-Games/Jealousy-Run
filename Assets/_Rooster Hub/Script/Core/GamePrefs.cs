using UnityEngine;

namespace RG.Core
{
    public static class GamePrefs
    {
        public static int LevelNo
        {
            get => PlayerPrefs.GetInt("LevelNo-RG");
            set => PlayerPrefs.SetInt("LevelNo-RG", value);
        }

        public static int GameCoin
        {
            get => PlayerPrefs.GetInt("Coin-RG");
            set => PlayerPrefs.SetInt("Coin-RG", value);
        }

        public static int CollectedCoin
        {
            get => PlayerPrefs.GetInt("CollectedCoin-RG");
            set => PlayerPrefs.SetInt("CollectedCoin-RG", value);
        }

        public static int SoundStatus
        {
            get => PlayerPrefs.GetInt("SoundStatus-RG");
            set => PlayerPrefs.SetInt("SoundStatus-RG", value);
        }

        public static int HapticStatus
        {
            get => PlayerPrefs.GetInt("HapticStatus-RG");
            set => PlayerPrefs.SetInt("HapticStatus-RG", value);
        }

        public static int GameScore
        {
            get => PlayerPrefs.GetInt("GameScore-RG");
            set => PlayerPrefs.SetInt("GameScore-RG", value);
        }

        public static int ScoreboardIndex
        {
            get => PlayerPrefs.GetInt("ScoreboardIndex-RG");
            set => PlayerPrefs.SetInt("ScoreboardIndex-RG", value);
        }
    }
}