using UnityEngine;
using TMPro;

public class TYForPlaying : MonoBehaviour
{
    [Header("スクロール設定")]
    public float startX = 500f;    // 開始位置X（右端）
    public float endX = -500f;     // 終端位置X（左端）
    public float speed = 100f;     // 移動速度（正の値でOK）

    private RectTransform rectTransform;
    private float fixedY;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // Y位置を固定して、Xだけ動かす
        fixedY = rectTransform.anchoredPosition.y;

        // 初期位置を右端にセット
        rectTransform.anchoredPosition = new Vector2(startX, fixedY);
    }

    void Update()
    {
        // X座標を更新（マイナス方向に移動）
        float newX = rectTransform.anchoredPosition.x - speed * Time.deltaTime;
        rectTransform.anchoredPosition = new Vector2(newX, fixedY);

        // 終端を過ぎたら開始位置に戻す
        if (newX <= endX)
        {
            rectTransform.anchoredPosition = new Vector2(startX, fixedY);
        }
    }
}
