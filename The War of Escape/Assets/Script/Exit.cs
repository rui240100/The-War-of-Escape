using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private bool isPlayerInTrigger = false;
    private Player currentPlayer;
    private float holdTime = 0f;
    private float requiredHoldTime = 7f; // 滞在に必要な秒数

    [System.Obsolete]
    void Update()
    {
        if (isPlayerInTrigger && currentPlayer != null)
        {
            holdTime += Time.deltaTime;

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

                // 処理後にリセット
                isPlayerInTrigger = false;
                holdTime = 0f;
            }
        }
        else
        {
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
            holdTime = 0f; // 新しく入ったときにリセット
            Debug.Log("出口範囲に入りました: " + player.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && player == currentPlayer)
        {
            isPlayerInTrigger = false;
            currentPlayer = null;
            holdTime = 0f;
            Debug.Log("出口範囲から出ました: " + player.name);
        }
    }
}
