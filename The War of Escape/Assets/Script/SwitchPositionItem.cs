using System.Collections;
using UnityEngine;
using TMPro;

public class SwitchPositionItem : Item
{
    private static GameObject countdownUI;
    private static TextMeshProUGUI countdownText;
    private bool isUsed = false;

    private Player playerScript;
    public string useMessageSw1 = "アイテムを使用しました";
    public string useMessageSw2 = "アイテムが使用されました";
    private GameObject itemUse;
    private ItemUse itemUseSc;

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
        itemUse = GameObject.Find("ItemUse");
        itemUseSc = itemUse.GetComponent<ItemUse>();
        if (itemUseSc == null)
        {
            Debug.LogError("MessageDisplayManagerがシーンにありません！");
        }
    }

    public override void Activate(Player user)
    {
        if (isUsed || user.otherPlayer == null) return;

        isUsed = true;
        user.StartCoroutine(SwitchPositionsAfterDelay(user));

        playerScript = user.GetComponent<Player>();

        if (itemUseSc != null)
        {
            if (playerScript.playerID == 1)
            {
                string message1 = useMessageSw1;
                string message2 = useMessageSw2;
                itemUseSc.ShowMessage(message1, message2);
            }
            else if (playerScript.playerID == 2)
            {
                string message1 = useMessageSw2;
                string message2 = useMessageSw1;
                itemUseSc.ShowMessage(message1, message2);
            }
        }
    }

    private IEnumerator SwitchPositionsAfterDelay(Player user)
    {
        if (countdownUI != null && countdownText != null)
        {
            countdownUI.SetActive(true);

            countdownText.text = $"{user.playerID + 1}Pが位置入れ替えアイテムを使用！";
            yield return new WaitForSeconds(1f);

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

        Destroy(gameObject);
    }

}
