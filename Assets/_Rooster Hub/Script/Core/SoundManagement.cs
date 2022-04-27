using System.Collections.Generic;
using UnityEngine;

public class SoundManagement : MonoBehaviour
{
    public static SoundManagement Instance;

    public AudioClip buttonClickSound;
    public AudioClip coinCollectSound;
    public List<AudioClip> customAudioClips = new List<AudioClip>();

    private AudioSource _audioSource;

    private void Awake()
    {
        Instance = this;
        _audioSource = FindObjectOfType<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
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