using UnityEngine;
using UnityEngine.SceneManagement;

public class TitlesceneManager : MonoBehaviour
{
    [Header("Œø‰Ê‰¹İ’è")]
    public AudioSource audioSource;   // ‰¹‚ğÄ¶‚·‚éAudioSource
    public AudioClip clip;            // Ä¶‚·‚é‰¹

    [System.Obsolete]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            // Œø‰Ê‰¹‚ğÄ¶
            if (audioSource != null && clip != null)
            {
                audioSource.PlayOneShot(clip);
            }

            // ƒV[ƒ“‘JˆÚ
            FadeManager.Instance.LoadScene("Stage-SIN", 2.0f);
        }
    }
}
