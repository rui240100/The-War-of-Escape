using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameDrector : MonoBehaviour
{
    public GameObject timeUI;
    public AudioSource bgmSource;       // 通常BGM用
    public AudioClip finalMinuteBGM;    // 残り1分用の短いBGM

    public float TimeCount = 180;
    private bool hasEnded = false;
    private bool isFinalMinuteBGMPlayed = false; // 1分BGMを一度だけ再生するフラグ

    public bool startGame = false;

    void Start()
    {
        timeUI.GetComponent<TextMeshProUGUI>().text = "0:00";
        if (bgmSource != null) bgmSource.Play(); // 最初のBGM再生
    }

    [System.Obsolete]
    void Update()
    {
        if (TimeCount > 0 )
        {
            TimeCount -= Time.deltaTime;

            // 残り1分になったら短いBGM再生
            if (TimeCount <= 60f && !isFinalMinuteBGMPlayed)
            {
                isFinalMinuteBGMPlayed = true;

                if (bgmSource != null && finalMinuteBGM != null)
                {
                    bgmSource.Stop(); // 通常BGMを止める
                    bgmSource.clip = finalMinuteBGM;
                    bgmSource.Play();
                }
            }
        }
        else if (TimeCount == 0 && !hasEnded)
        {
            hasEnded = true;
            TimeCount = 0;
            FadeManager.Instance.LoadScene("Clear", 2.0f);
        }

        int second = (int)TimeCount % 60;
        timeUI.GetComponent<TextMeshProUGUI>().text = "" + (int)TimeCount / 60 + ":" + second.ToString("D2");
    }
}
