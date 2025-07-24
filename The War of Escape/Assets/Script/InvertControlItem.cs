using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertControlItem : Item
{
    public float duration = 10f; // 反転が続く時間

    private Player playerScriptRe;
    private string useMessageRe1 = "操作反転アイテムを使用しました";
    private string useMessageRe2 = "移動操作が反転されました";
    private GameObject itemUseRe;
    private ItemUse itemUseScRe;

    private void Start()
    {
        // シーン内のMessageDisplayManagerを探す
        itemUseRe = GameObject.Find("ItemUse");
        itemUseScRe = itemUseRe.GetComponent<ItemUse>();
        if (itemUseScRe == null)
        {
            Debug.LogError("MessageDisplayManagerがシーンにありません！");
        }
    }

    public override void Activate(Player user)
    {
        if (user.otherPlayer != null)
        {
            user.StartCoroutine(InvertControl(user.otherPlayer,user));
        }

        playerScriptRe = user.GetComponent<Player>();

        if (itemUseScRe != null)
        {
            if (playerScriptRe.playerID == 1)
            {
                string message1 = useMessageRe1;
                string message2 = useMessageRe2;
                itemUseScRe.ShowMessage(message1, message2);
            }
            else if (playerScriptRe.playerID == 2)
            {
                string message1 = useMessageRe2;
                string message2 = useMessageRe1;
                itemUseScRe.ShowMessage(message1, message2);
            }
        }
        user.SetHeldItem(null);
    }

    private IEnumerator InvertControl(Player targetPlayer,Player user)
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
