using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class SoundFloorGimmic : MonoBehaviour
{
    [Header("“¥‚ñ‚¾‚ç‰¹‚ğ–Â‚ç‚·‘ÎÛi•¡”‰Âj")]
    public List<GameObject> playerObjects = new List<GameObject>();

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerObjects.Contains(other.gameObject))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
