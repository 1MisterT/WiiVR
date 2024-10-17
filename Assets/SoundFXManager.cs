using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    [SerializeField] private AudioSource _soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFX(AudioClip audioClip, Transform spawnTransform, float volume = 1f)
    {
        //spawn gameObject
        AudioSource audioSource = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);
        
        //assign audioClip and play
        audioSource.clip = audioClip;
        audioSource.Play();
        
        float clipLenght = audioClip.length;
        Destroy(audioSource, clipLenght);
    }

    public void PlayRandomSoundFX(AudioClip[] audioClips, Transform spawnTransform, float volume = 1f)
    {
        int randomIndex = Random.Range(0, audioClips.Length);
        
        //spawn gameObject
        AudioSource audioSource = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);
        
        //assign audioClip and play
        audioSource.clip = audioClips[randomIndex];
        audioSource.Play();
        
        //delete Object after end
        float clipLenght = audioClips[randomIndex].length;
        Destroy(audioSource, clipLenght);
    }
}
