using UnityEngine;

namespace RoosterHub
{
    public static class Sound
    {
        public static void PlayButtonSound()
        {
            if (SoundManagement.Instance != null)
                SoundManagement.Instance.PlayButtonClick();
            else
                RoosterLogger.Log("Please Add Sound Manager", Color.yellow);
        }

        public static void PlayCoinCollectSound()
        {
            if (SoundManagement.Instance != null)
                SoundManagement.Instance.PlayCoinCollectSound();
            else
                RoosterLogger.Log("Please Add Sound Manager", Color.yellow);
        }

        public static void PlayCustomSound(int index)
        {
            if (SoundManagement.Instance != null)
                SoundManagement.Instance.PlayCustomSound(index);
            else
                RoosterLogger.Log("Please Add Sound Manager", Color.yellow);
        }
    }
}