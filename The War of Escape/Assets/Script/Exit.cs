using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private bool isPlayerInTrigger = false;
    private Player currentPlayer;
    private float holdTime = 0f;
    private float requiredHoldTime = 2f; // 長押しに必要な秒数

    [System.Obsolete]
    void Update()
    {
        if (isPlayerInTrigger && currentPlayer != null)
        {
            string inputButton = GetInputButtonName(currentPlayer.playerID);

            if (!string.IsNullOrEmpty(inputButton) && Input.GetButton(inputButton))
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
    }

    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            currentPlayer = player;
            isPlayerInTrigger = true;
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

    /// <summary>
    /// プレイヤーIDに応じて使用するボタン名を返す
    /// </summary>
    string GetInputButtonName(int playerID)
    {
        switch (playerID)
        {
            case 1: return "Fire2";     // 1P用
            case 2: return "Fire2_2";   // 2P用
            default: return null;
        }
    }
}
