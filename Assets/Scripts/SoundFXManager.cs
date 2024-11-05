using UnityEngine;

// Copyright (C) Tom Troeger

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;
    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] public AudioClip uiSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySoundFX(AudioClip audioClip, Transform spawnTransform, float volume = 1f)
    {
        if (audioClip is null) return;
        //spawn gameObject
        var audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        
        //assign audioClip and play
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        
        var clipLenght = audioClip.length;
        Destroy(audioSource, clipLenght);
    }

    public void PlayRandomSoundFX(AudioClip[] audioClips, Transform spawnTransform, float volume = 1f)
    {
        var randomIndex = Random.Range(0, audioClips.Length);
        
        //spawn gameObject
        var audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        
        //assign audioClip and play
        audioSource.clip = audioClips[randomIndex];
        audioSource.Play();
        
        //delete Object after end
        var clipLenght = audioClips[randomIndex].length;
        Destroy(audioSource, clipLenght);
    }
}
