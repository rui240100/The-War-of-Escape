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
        // 完全遮蔽
        var c = overlay.color; c.a = 1f; overlay.color = c;
        yield return new WaitForSeconds(blindDuration);

        // フェードアウト
        float t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeOutDuration);
            overlay.color = c;
            yield return null;
        }
        c.a = 0f; overlay.color = c;

        // 処理完了、アイテム自壊
        Destroy(gameObject);
    }
}
