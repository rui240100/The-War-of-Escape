using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro用

public class Exit : MonoBehaviour
{
    private bool isPlayerInTrigger = false;
    private Player currentPlayer;
    private float holdTime = 0f;
    private float requiredHoldTime = 7f; // 滞在に必要な秒数

    [Header("UI表示用")]
    public TextMeshProUGUI countdownText; // インスペクターで設定

    [System.Obsolete]
    void Update()
    {
        if (isPlayerInTrigger && currentPlayer != null)
        {
            holdTime += Time.deltaTime;

            // 残り秒数（切り上げ）
            float remainingTime = Mathf.Ceil(requiredHoldTime - holdTime);
            if (remainingTime < 0) remainingTime = 0;

            // UIに表示
            if (countdownText != null)
            {
                countdownText.text = remainingTime.ToString("0");
            }

            // 指定時間経過
            if (holdTime >= requiredHoldTime)
            {
                if (currentPlayer.keyCount >= 3)
                {
                    ResultData.escapedPlayerID = currentPlayer.playerID;
                    FadeManager.Instance.LoadScene("Clear", 2.0f);
                }
                else
                {
                    Debug.Log($"Player {currentPlayer.playerID} は鍵が足りません！（{currentPlayer.keyCount}/3）");
                }

                ResetTrigger();
            }
        }
        else
        {
            // 範囲外では非表示
            if (countdownText != null)
            {
                countdownText.text = "";
            }
            holdTime = 0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            currentPlayer = player;
            isPlayerInTrigger = true;
            holdTime = 0f;
            Debug.Log("出口範囲に入りました: " + player.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && player == currentPlayer)
        {
            ResetTrigger();
            Debug.Log("出口範囲から出ました: " + player.name);
        }
    }

    private void ResetTrigger()
    {
        isPlayerInTrigger = false;
        currentPlayer = null;
        holdTime = 0f;

        if (countdownText != null)
        {
            countdownText.text = "";
        }
    }
}
