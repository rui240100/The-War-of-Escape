using UnityEngine;

public class SpeedUpItem : Item
{
    public float speedUpDuration = 30f;    // 効果時間
    public float speedUpMultiplier = 1.2f; // 倍率

    [Header("サウンド設定")]
    public AudioClip useSound;        // 使用時に鳴らす音
    [Range(0f, 1f)] public float soundVolume = 1f;

    public AudioClip loopSound;       // 効果時間中にループ再生する音
    [Range(0f, 1f)] public float loopVolume = 0.7f;

    private string useMessageSp1 = "スピードアップアイテムを使用しました";
    private string useMessageSp2 = "";

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

        // 使用時の効果音
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
        player.Speed = 7.5f;

        // パーティクル制御
        if (player.speedUpEffect != null)
        {
            player.speedUpEffect.gameObject.SetActive(true);
            player.speedUpEffect.Play();
        }

        // ===== ループサウンド再生 =====
        AudioSource loopSource = null;
        if (loopSound != null)
        {
            loopSource = player.gameObject.AddComponent<AudioSource>();
            loopSource.clip = loopSound;
            loopSource.volume = loopVolume;
            loopSource.loop = true;
            loopSource.spatialBlend = 0f; // 2D再生（常に聞こえる）
            loopSource.Play();
        }

        // 効果時間
        yield return new WaitForSeconds(speedUpDuration);

        // スピードを元に戻す
        player.Speed = originalSpeed;

        // パーティクル終了
        if (player.speedUpEffect != null)
        {
            player.speedUpEffect.Stop();
            player.speedUpEffect.gameObject.SetActive(false);
        }

        // ループサウンド停止 & 削除
        if (loopSource != null)
        {
            loopSource.Stop();
            Destroy(loopSource);
        }
    }
}
