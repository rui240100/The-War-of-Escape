using UnityEngine;
using TMPro;
using System.Collections;

public class SwitchPositionItem : Item
{
    //  static にして全インスタンスで共通にする（シーンに1つのUI）
    private static GameObject countdownUI;
    private static TextMeshProUGUI countdownText;

    private bool isUsed = false;

    void Start()
    {
        //  まだ取得していない場合だけ探す
        if (countdownUI == null)
        {
            countdownUI = GameObject.Find("SwitchPositionCountdown");
            if (countdownUI != null)
            {
                Transform textObj = countdownUI.transform.Find("CountText");
                if (textObj != null)
                {
                    countdownText = textObj.GetComponent<TextMeshProUGUI>();
                }

                countdownUI.SetActive(false); // ← ゲーム開始時に非表示
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && !player.HasItem)
        {
            player.SetHeldItem(this);
            transform.SetParent(player.transform);
            transform.localPosition = Vector3.zero;
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public override void Activate(Player user)
    {
        if (isUsed || user.otherPlayer == null) return;

        isUsed = true;
        user.StartCoroutine(SwitchPositionsAfterDelay(user));
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
        }

        Transform other = user.otherPlayer.transform;
        Vector3 tempPos = user.transform.position;
        user.transform.position = other.position;
        other.position = tempPos;

        if (countdownUI != null)
        {
            countdownUI.SetActive(false);
        }

        Destroy(gameObject); // アイテムは使い捨て
    }
}
