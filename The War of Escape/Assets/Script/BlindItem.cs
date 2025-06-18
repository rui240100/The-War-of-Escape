using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 相手プレイヤーの画面中央にお札（黒い画像）を貼り付け、
/// 5 秒後から 3 秒でフェードアウトさせるアイテム。
/// </summary>
public class BlindItem : Item
{
    [Header("Blind Settings")]
    public float blindDuration = 5f;
    public float fadeOutDuration = 3f;

    [Header("UI Manager")]
    private UIManager uiManager;

    private void Awake()
    {
        // UIManagerをシーンから探す（シングルトンでもOK）
        uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager がシーンに存在しません！");
        }
    }

    public override void Activate(Player user)
    {
        if (user.otherPlayer != null)
        {
            // 相手のプレイヤーの画面にブラインドを貼る
            StartCoroutine(BlindRoutine(user.otherPlayer.playerID));
        }

        Destroy(gameObject); // アイテムは使い捨て
    }

    private IEnumerator BlindRoutine(int targetPlayerID)
    {
        Canvas canvas = (targetPlayerID == 1) ?
                        uiManager.player1Canvas :
                        uiManager.player2Canvas;

        if (canvas == null)
        {
            Debug.LogError(" Canvas が null です！");
            yield break;
        }

        Debug.Log(" 使用中の Canvas: " + canvas.name);

        // ── まずは直下に探す
        Transform overlayTransform = canvas.transform.Find("BlindOverlay");
        if (overlayTransform == null)
        {
            Debug.Log("BlindOverlay は直下に見つかりませんでした。階層が深いかも？");
        }

        Image overlay = overlayTransform?.GetComponent<Image>();

        // ── 子孫全体から再検索（階層が深い場合対応）
        if (overlay == null)
        {
            Debug.Log(" 子階層の Image を探索中...");

            foreach (var img in canvas.GetComponentsInChildren<Image>(true))
            {
                Debug.Log(" 検出: " + img.gameObject.name);
                if (img.gameObject.name == "BlindOverlay")
                {
                    overlay = img;
                    Debug.Log(" BlindOverlay を子階層で発見しました！");
                    break;
                }
            }
        }

        if (overlay == null)
        {
            Debug.LogError(" BlindOverlay が本当に Canvas 内に見つかりません！");
            yield break;
        }

        // ── フェード処理
        overlay.enabled = true;
        var c = overlay.color;
        c.a = 1f;
        overlay.color = c;

        yield return new WaitForSeconds(blindDuration);

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
