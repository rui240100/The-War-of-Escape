using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;  // ← 追加！
public class GameDrector : MonoBehaviour
{
    public GameObject timeUI;

    float TimeCount = 180;
    private bool hasEnded = false; // 終了処理が一度だけ実行されるように

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeUI.GetComponent<TextMeshProUGUI>().text = "0:00";
    }

    // Update is called once per frame
    void Update()
    {

        if( TimeCount > 0)
        {
            TimeCount -= Time.deltaTime;

        }

        else if (!hasEnded)
        {
            hasEnded = true; // 2回以上呼ばれないように
            TimeCount = 0;   // マイナスにならないように固定
            SceneManager.LoadScene("Clear"); // ← シーン名をここに！
        }

        int second = (int)TimeCount % 60;

        timeUI.GetComponent<TextMeshProUGUI>().text = "" + (int)TimeCount / 60 + ":" + second.ToString("D2");
    }
}
