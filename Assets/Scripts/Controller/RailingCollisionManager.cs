using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RailingCollisionManager : MonoBehaviour
{
    private AudioSource metalAudioSource;

    void Start()
    {
        metalAudioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Player")
        {
            metalAudioSource.Play();
        }
    }
}

