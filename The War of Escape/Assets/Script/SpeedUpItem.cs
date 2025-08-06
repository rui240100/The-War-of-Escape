using UnityEngine;

public class SpeedUpItem : Item
{
    public float speedUpDuration = 30f;
    public float speedUpMultiplier = 1.2f;

    private string useMessageSp1 = "スピードアップアイテムを使用しました";
    private string useMessageSp2 = "相手がスピードアップアイテムを使用しました";

    private GameObject itemUseSp;
    private ItemUse itemUseScSp;

    void Start()
    {
        itemUseSp = GameObject.Find("ItemUse");
        itemUseScSp = itemUseSp?.GetComponent<ItemUse>();
        if (itemUseScSp == null)
        {
            Debug.LogError("MessageDisplayManager（ItemUse）が見つかりません！");
        }
    }

    public override void Activate(Player user)
    {
        // 自分のスピードアップ
        user.StartCoroutine(SpeedUp(user));

        // メッセージ表示（任意）
        if (itemUseScSp != null)
        {
            if (user.playerID == 1)
            {
                itemUseScSp.ShowMessage(useMessageSp1, useMessageSp2);
            }
            else
            {
                itemUseScSp.ShowMessage(useMessageSp2, useMessageSp1);
            }
        }

        // アイテム消費
        user.SetHeldItem(null);
    }

    private System.Collections.IEnumerator SpeedUp(Player player)
    {
        float originalSpeed = player.Speed;
        player.Speed *= speedUpMultiplier;

        yield return new WaitForSeconds(speedUpDuration);

        player.Speed = originalSpeed;
    }
}
