using UnityEngine;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour
{
    public static string PlayerGame;
    public AudioMixer mainMixer;
    public static PlayerController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerGame = other.gameObject.CompareTag("GameCollider") ? other.gameObject.name : null;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("GameCollider"))
        {
            PlayerGame = null;
        }
    }
}
