using UnityEngine;

public class WalkRange : MonoBehaviour
{
    //public AudioClip walkSound;
    public AudioSource audioSource;

    public DemonAI demonAI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!audioSource.isPlaying && demonAI.isChasing == false)
            {
                audioSource.pitch = 1.0f;
                audioSource.Play();
            }
            else if(!audioSource.isPlaying && demonAI.isChasing == true)
            {
                audioSource.pitch = 1.5f;
                audioSource.Play();
            }
        }
    }
}
