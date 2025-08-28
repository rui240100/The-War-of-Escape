using UnityEngine;

public class WalkRange : MonoBehaviour
{
    public AudioClip walkSound;
    public AudioSource audioSource;
    private bool walkSoudPlaying = false;

    public DemonAI demonAI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!walkSoudPlaying && demonAI.isChasing == false)
            {
                walkSoudPlaying = true;
                audioSource.PlayOneShot(walkSound);
                walkSoudPlaying = false;
            }
        }
    }
}
