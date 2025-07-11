using UnityEngine;
using DG.Tweening;
using TMPro;

public class ButtonPush : MonoBehaviour
{
    public TextMeshProUGUI blinkingText;

    void Start()
    {
        if (blinkingText == null)
        {
            Debug.LogError("blinkingText が未設定です！");
            return;
        }

        //  色を取得してAlphaを明示的に 1 に
        Color c = blinkingText.color;
        c.a = 1f;
        blinkingText.color = c;

        //  点滅処理（Alpha → 0 → 1 を繰り返す）
        blinkingText.DOFade(0f, 1.0f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}
