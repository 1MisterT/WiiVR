using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    [FormerlySerializedAs("_soundFXObject")] 
    [SerializeField] public AudioSource soundFXObject;
    [SerializeField] public AudioClip uiSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFX(AudioClip audioClip, Transform spawnTransform, float volume = 1f)
    {
        if (audioClip is null) return;
        //spawn gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        
        //assign audioClip and play
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        
        float clipLenght = audioClip.length;
        Destroy(audioSource, clipLenght);
    }

    public void PlayRandomSoundFX(AudioClip[] audioClips, Transform spawnTransform, float volume = 1f)
    {
        int randomIndex = Random.Range(0, audioClips.Length);
        
        //spawn gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        
        //assign audioClip and play
        audioSource.clip = audioClips[randomIndex];
        audioSource.Play();
        
        //delete Object after end
        float clipLenght = audioClips[randomIndex].length;
        Destroy(audioSource, clipLenght);
    }
}
