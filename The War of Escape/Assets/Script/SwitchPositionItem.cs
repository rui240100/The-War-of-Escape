using System.Collections;
using UnityEngine;
using TMPro;

public class SwitchPositionItem : Item
{
    private static GameObject countdownUI;
    private static TextMeshProUGUI countdownText;
    private bool isUsed = false;

    private Player playerScriptSw;
    private string useMessageSw1 = "";
    private string useMessageSw2 = "";
    private GameObject itemUseSw;
    private ItemUse itemUseScSw;

    void Start()
    {
        // 非アクティブなオブジェクトも対象に含めて探す
        if (countdownUI == null)
        {
            // 1. UIのCanvas（親）を探す
            GameObject uiRoot = GameObject.Find("SwitchPositionCountdownUI");

            if (uiRoot != null)
            {
                countdownUI = uiRoot.transform.Find("SwitchPositionCountdown")?.gameObject;

                // 2. CountText をその子から探す
                if (countdownUI != null)
                {
                    countdownText = countdownUI.transform.Find("CountText")
                                    ?.GetComponent<TextMeshProUGUI>();

                    // 最初は非表示にしておく
                    countdownUI.SetActive(false);
                }
            }
        }

        // シーン内のMessageDisplayManagerを探す
        itemUseSw = GameObject.Find("ItemUse");
        itemUseScSw = itemUseSw.GetComponent<ItemUse>();
        if (itemUseScSw == null)
        {
            Debug.LogError("MessageDisplayManagerがシーンにありません！");
        }
    }

    public override void Activate(Player user)
    {
        if (isUsed || user.otherPlayer == null) return;

        isUsed = true;
        user.StartCoroutine(SwitchPositionsAfterDelay(user));

        playerScriptSw = user.GetComponent<Player>();

        if (itemUseScSw != null)
        {
            if (playerScriptSw.playerID == 1)
            {
                string message1 = useMessageSw1;
                string message2 = useMessageSw2;
                itemUseScSw.ShowMessage(message1, message2);
            }
            else if (playerScriptSw.playerID == 2)
            {
                string message1 = useMessageSw2;
                string message2 = useMessageSw1;
                itemUseScSw.ShowMessage(message1, message2);
            }
        }
    }

    private IEnumerator SwitchPositionsAfterDelay(Player user)
    {
        if (countdownUI != null && countdownText != null)
        {
            countdownUI.SetActive(true);

            countdownText.text = $"{user.playerID + 1}Pが位置入れ替えアイテムを使用！";
            yield return new WaitForSeconds(2f);

            countdownText.text = "3...";
            yield return new WaitForSeconds(1f);

            countdownText.text = "2...";
            yield return new WaitForSeconds(1f);

            countdownText.text = "1...";
            yield return new WaitForSeconds(1f);

            countdownText.text = "0..."; // お好みで表示
        }

        // プレイヤーの位置を入れ替える
        Transform other = user.otherPlayer.transform;
        Vector3 temp = user.transform.position;
        user.transform.position = other.position;
        other.position = temp;

        // 少しだけ表示を続けてもOK（お好み）
        yield return new WaitForSeconds(0.5f);

        if (countdownUI != null)
        {
            countdownUI.SetActive(false);
        }

        user.SetHeldItem(null);
    }

}
