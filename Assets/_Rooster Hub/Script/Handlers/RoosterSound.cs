
public static class RoosterSound 
{
    public static void PlayButtonSound()
    {
        RoosterSoundManager.Instance.PlayButtonClick();
    }

    public static void PlayCoinCollectSound()
    {
        RoosterSoundManager.Instance.PlayCoinCollectSound();
    }

    public static void PlayCustomSound(int index)
    {
        RoosterSoundManager.Instance.PlayCustomSound(index);
    }
}
