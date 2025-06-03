using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float timeLimit = 180f;  // 制限時間（秒）
    private float currentTime;     // 現在の残り時間
    public TextMeshProUGUI timeText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = timeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime <= 0)
        {
            GameOver();
        }
        else
        {
            currentTime -= Time.deltaTime;
        }

        // 残り時間をUIに表示
        timeText.text = "Time Left: " + Mathf.Ceil(currentTime).ToString() + "s";
    }

    void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    // 残り時間を取得する関数を追加
    public float GetRemainingTime()
    {
        return Mathf.Max(currentTime, 0f); // 0以下にならないようにする
    }








}
