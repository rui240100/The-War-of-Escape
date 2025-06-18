using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 相手プレイヤーの画面中央にお札を貼り付け、
/// 5 秒後から 3 秒でフェードアウトさせるアイテム。
/// </summary>
public class BlindItem : Item
{
    [Header("Blind settings")]
    public float blindDuration = 5f;
    public float fadeOutDuration = 3f;

    // slowdown 用パラメータは無視
    public override void Activate(Player user)
    {
        if (user.otherPlayer != null)
        {
            user.otherPlayer.StartCoroutine(BlindRoutine(user.otherPlayer));
        }

        Destroy(gameObject);   // 使い捨て
    }

    private IEnumerator BlindRoutine(Player target)
    {
        Canvas canvas = GameObject.Find($"Player{target.playerID}Canvas")?.GetComponent<Canvas>();
        if (canvas == null) yield break;

        Image overlay = canvas.transform
                               .Find("BlindOverlay")
                               ?.GetComponent<Image>();
        if (overlay == null) yield break;

        // 完全に貼り付け
        overlay.enabled = true;          // ← 修正
        var c = overlay.color;
        c.a = 1f;
        overlay.color = c;

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

        overlay.enabled = false;
    }
}
