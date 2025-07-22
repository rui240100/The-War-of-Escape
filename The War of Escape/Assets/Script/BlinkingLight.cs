using System.Collections;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    [SerializeField] private Light lightComponent;
    [SerializeField] private float minOnDuration = 0.5f;
    [SerializeField] private float maxOnDuration = 1.5f;
    [SerializeField] private float minOffDuration = 0.5f;
    [SerializeField] private float maxOffDuration = 1.5f;
    [SerializeField] private bool alwaysOn = false; // ← 追加

    private void Start()
    {
        if (lightComponent == null)
        {
            lightComponent = GetComponent<Light>();
        }

        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            if (alwaysOn)
            {
                lightComponent.enabled = true;
                yield break; // 常時点灯なら点滅処理終了
            }

            // ライトを ON にして一定時間待つ
            lightComponent.enabled = true;
            float onDuration = Random.Range(minOnDuration, maxOnDuration);
            yield return new WaitForSeconds(onDuration);

            // ライトを OFF にして一定時間待つ
            lightComponent.enabled = false;
            float offDuration = Random.Range(minOffDuration, maxOffDuration);
            yield return new WaitForSeconds(offDuration);
        }
    }
}
