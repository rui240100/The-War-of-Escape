using UnityEngine;

public class SpeedUpItem : Item
{
    public float speedUpDuration = 30f;    // 効果時間
    public float speedUpMultiplier = 1.2f; // 倍率

    [Header("サウンド設定")]
    public AudioClip useSound;   // 使用時に鳴らす音
    [Range(0f, 1f)] public float soundVolume = 1f;

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
        // コルーチン開始
        user.StartCoroutine(SpeedUp(user));

        // 効果音再生
        if (useSound != null)
        {
            AudioSource.PlayClipAtPoint(useSound, user.transform.position, soundVolume);
        }

        // メッセージ表示
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

        // パーティクル制御
        if (player.speedUpEffect != null)
        {
            player.speedUpEffect.gameObject.SetActive(true);
            player.speedUpEffect.Play();
        }

        yield return new WaitForSeconds(speedUpDuration);

        player.Speed = originalSpeed;

        if (player.speedUpEffect != null)
        {
            player.speedUpEffect.Stop();
            player.speedUpEffect.gameObject.SetActive(false);
        }
    }
}
