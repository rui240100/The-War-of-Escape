using System.Collections;
using System.Collections.Generic;
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

    // 傾け済みかどうか記録する辞書（複数プレイヤー対応）
    private static readonly Dictionary<int, bool> rotatedOnce = new Dictionary<int, bool>();

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
        Player target = user.otherPlayer;
        if (target == null) return;

        Image overlay = FindOverlay(target.playerID);
        if (overlay == null) return;

        StartCoroutine(BlindRoutine(overlay, target.playerID));

        playerScriptBl = user.GetComponent<Player>();
        user.SetHeldItem(null);

        if (itemUseScBl != null)
        {
            if (playerScriptBl.playerID == 1)
            {
                itemUseScBl.ShowMessage(useMessageBl1, useMessageBl2);
            }
            else if (playerScriptBl.playerID == 2)
            {
                itemUseScBl.ShowMessage(useMessageBl2, useMessageBl1);
            }
        }
    }

    // ───────── 補助メソッド ─────────
    private Image FindOverlay(int playerID)
    {
        string objName = string.Format(OverlayNameFormat, playerID);
        GameObject obj = GameObject.Find(objName);
        return obj != null ? obj.GetComponent<Image>() : null;
    }

    private IEnumerator BlindRoutine(Image overlay, int playerID)
    {
        if (blindSprite != null)
        {
            overlay.sprite = blindSprite;
            overlay.color = new Color(1f, 1f, 1f, 1f);
            overlay.preserveAspect = true;

            // 一度だけ回転を適用する
            if (!rotatedOnce.ContainsKey(playerID) || !rotatedOnce[playerID])
            {
                overlay.rectTransform.rotation = Quaternion.Euler(0f, 0f, 15f);
                rotatedOnce[playerID] = true;
            }
        }
        else
        {
            overlay.color = new Color(0f, 0f, 0f, 1f);
        }

        yield return new WaitForSeconds(blindDuration);

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

        overlay.sprite = null;

        Destroy(gameObject);
    }
}
