using RG.Core;

namespace RoosterHub
{
    public class Scoreboard
    {
        public static void SetScore(int score)
        {
            GamePrefs.GameScore += score;
        }
    }
}