using System.Collections.Generic;
using UnityEngine;

public class RoosterSoundManager : MonoBehaviour
{
    public static RoosterSoundManager Instance;
    
    public AudioClip buttonClickSound;
    public AudioClip coinCollectSound;
    public List<AudioClip> customAudioClips = new List<AudioClip>();

    private AudioSource _audioSource;

    private void Awake()
    {
        Instance = this;
        _audioSource = FindObjectOfType<AudioSource>();
    }

    public void PlayButtonClick()
    {
        _audioSource.PlayOneShot(buttonClickSound);
    }

    public void PlayCoinCollectSound()
    {
        _audioSource.PlayOneShot(coinCollectSound);
    }

    public void PlayCustomSound(int index)
    {
        _audioSource.PlayOneShot(customAudioClips[index]);
    }
}