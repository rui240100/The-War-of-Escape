using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ClearSceneController : MonoBehaviour
{
    public TextMeshProUGUI resultText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int id = ResultData.escapedPlayerID;

        if (id != -1)
        {
            resultText.text = $"Player {id} 脱出成功！";
        }
        else
        {
            resultText.text = "誰も脱出していません";
        }
    }


    [Header("効果音設定")]
    public AudioSource audioSource;   // 音を再生するAudioSource
    public AudioClip clip;            // 再生する音

    [System.Obsolete]


    // Update is called once per frame
    void Update()
    {


        // XboxパッドのBボタンは "JoystickButton1" に対応
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            // 効果音を再生
            if (audioSource != null && clip != null)
            {
                audioSource.PlayOneShot(clip);
            }


            FadeManager.Instance.LoadScene("TitleScene", 2.0f);
        }
    }
}
