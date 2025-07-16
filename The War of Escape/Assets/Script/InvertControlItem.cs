using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertControlItem : Item
{
    public float duration = 10f; // 反転が続く時間

    private Player playerScript;
    public string useMessageRe1 = "アイテムを使用しました";
    public string useMessageRe2 = "アイテムが使用されました";
    private GameObject itemUse;
    private ItemUse itemUseSc;

    private void Start()
    {
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
        if (user.otherPlayer != null)
        {
            user.StartCoroutine(InvertControl(user.otherPlayer));
        }

        playerScript = user.GetComponent<Player>();

        if (itemUseSc != null)
        {
            if (playerScript.playerID == 1)
            {
                string message1 = useMessageRe1;
                string message2 = useMessageRe2;
                itemUseSc.ShowMessage(message1, message2);
            }
            else if (playerScript.playerID == 2)
            {
                string message1 = useMessageRe2;
                string message2 = useMessageRe1;
                itemUseSc.ShowMessage(message1, message2);
            }
        }
    }

    private IEnumerator InvertControl(Player targetPlayer)
    {
        InvertedInput inverted = targetPlayer.gameObject.GetComponent<InvertedInput>();
        if (inverted == null)
        {
            inverted = targetPlayer.gameObject.AddComponent<InvertedInput>();
        }

        inverted.EnableInversion();

        yield return new WaitForSeconds(duration);

        inverted.DisableInversion();
    }
}
