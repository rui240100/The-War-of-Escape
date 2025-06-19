using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlindItem : Item
{
    public float blindDuration = 5f;
    public float fadeOutDuration = 3f;

    private Image blindOverlayP1;
    private Image blindOverlayP2;

    private void Awake()
    {
        var canvasP1 = GameObject.Find("BlindCanvas_P1");
        if (canvasP1 != null)
            blindOverlayP1 = canvasP1.GetComponentInChildren<Image>();

        var canvasP2 = GameObject.Find("BlindCanvas_P2");
        if (canvasP2 != null)
            blindOverlayP2 = canvasP2.GetComponentInChildren<Image>();

        if (blindOverlayP1 != null)
            blindOverlayP1.color = new Color(1, 1, 1, 0);
        if (blindOverlayP2 != null)
            blindOverlayP2.color = new Color(1, 1, 1, 0);
    }

    public override void Activate(Player user)
    {
        if (user == null) return;

        Player target = user.otherPlayer;
        if (target == null)
        {
            Debug.LogWarning("BlindItem: ëäéËÉvÉåÉCÉÑÅ[Ç™å©Ç¬Ç©ÇËÇ‹ÇπÇÒ");
            return;
        }

        if (target.playerID == 1)
            user.StartCoroutine(ApplyBlindEffectCoroutine(blindOverlayP1));
        else if (target.playerID == 2)
            user.StartCoroutine(ApplyBlindEffectCoroutine(blindOverlayP2));
        else
            Debug.LogWarning("BlindItem: ñ≥å¯Ç»playerID");

        Destroy(this.gameObject);
    }

    private IEnumerator ApplyBlindEffectCoroutine(Image overlay)
    {
        if (overlay == null) yield break;

        overlay.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(blindDuration);

        float timer = 0f;
        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, timer / fadeOutDuration);
            overlay.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        overlay.color = new Color(1, 1, 1, 0);
    }
}
