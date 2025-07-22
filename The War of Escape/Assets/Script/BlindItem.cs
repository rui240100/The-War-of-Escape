using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// げっそー：相手の視界を 5 秒完全遮蔽し、3 秒かけてフェードアウト
/// </summary>
public class BlindItem : Item
{
    [Header("Blind Settings")]
    [SerializeField] private float blindDuration = 5f;
    [SerializeField] private float fadeOutDuration = 3f;

    private const string OverlayNameFormat = "BlindOverlay_P{0}";

    [Header("Blind Image")]
    [SerializeField] private Sprite blindSprite;

    private Player playerScriptBl;
    private string useMessageBl1 = "目隠しアイテムを使用しました";
    private string useMessageBl2 = "";
    private GameObject itemUseBl;
    private ItemUse itemUseScBl;

    private void Start()
    {
        // シーン内のMessageDisplayManagerを探す
        itemUseBl = GameObject.Find("ItemUse");
        itemUseScBl = itemUseBl.GetComponent<ItemUse>();
        if (itemUseScBl == null)
        {
            Debug.LogError("MessageDisplayManagerがシーンにありません！");
        }
    }

    // ───────── アイテム使用 ─────────
    public override void Activate(Player user)
    {
        if (user == null) return;

        // 相手プレイヤーを取得（Player スクリプトの otherPlayer を利用）
        Player target = user.otherPlayer;
        if (target == null) return;

        // 対象プレイヤー専用のオーバーレイ Image を探す
        Image overlay = FindOverlay(target.playerID);
        if (overlay == null) return;

        // エフェクト開始（コルーチンはアイテム自身で動かす）
        StartCoroutine(BlindRoutine(overlay));

        playerScriptBl = user.GetComponent<Player>();

        if (itemUseScBl != null)
        {
            if (playerScriptBl.playerID == 1)
            {
                string message1 = useMessageBl1;
                string message2 = useMessageBl2;
                itemUseScBl.ShowMessage(message1, message2);
            }
            else if (playerScriptBl.playerID == 2)
            {
                string message1 = useMessageBl2;
                string message2 = useMessageBl1;
                itemUseScBl.ShowMessage(message1, message2);
            }
        }
        user.SetHeldItem(null);
    }

    // ───────── 補助メソッド ─────────
    private Image FindOverlay(int playerID)
    {
        string objName = string.Format(OverlayNameFormat, playerID); // 例: BlindOverlay_P2
        GameObject obj = GameObject.Find(objName);
        return obj != null ? obj.GetComponent<Image>() : null;
    }

    private IEnumerator BlindRoutine(Image overlay)
    {
        //  スプライトを設定（nullチェックも入れると安心）
        if (blindSprite != null)
        {
            overlay.sprite = blindSprite;
            overlay.color = new Color(1f, 1f, 1f, 1f); // 不透明で白色
            overlay.preserveAspect = true;             // 画像が伸びないように

            overlay.rectTransform.rotation = Quaternion.Euler(0f, 0f, 15f); // 15度右に傾ける

        }
        else
        {
            // スプライト未設定の場合は黒で塗りつぶす
            overlay.color = new Color(0f, 0f, 0f, 1f);
        }

        yield return new WaitForSeconds(blindDuration);

        // 🔻 フェードアウト処理
        float t = 0f;
        Color c = overlay.color;

        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeOutDuration);
            overlay.color = c;
            yield return null;
        }

        c.a = 0f;
        overlay.color = c;

        //  スプライトを消す（リセット）
        overlay.sprite = null;

        Destroy(gameObject);
    }
}
