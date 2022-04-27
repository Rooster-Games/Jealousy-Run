using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoosterHub
{
    public static class Player
    {
        public static RoosterPlayer GetRandomPlayer()
        {
            return PlayerGenerator.Instance.GetPlayer();
        }

        public static string GetRandomPlayerName()
        {
            return PlayerGenerator.Instance.GetPlayerName();
        }

        public static Sprite GetRandomFlag()
        {
            return PlayerGenerator.Instance.GetPlayerFlag();
        }
    }
}