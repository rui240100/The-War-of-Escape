using System.Collections;
using UnityEngine;

public class LightPulse : MonoBehaviour
{
    [SerializeField] private Light[] lightComponents; // 複数ライトをまとめて指定
    [SerializeField] private float minOnDuration = 0.5f;
    [SerializeField] private float maxOnDuration = 1.5f;
    [SerializeField] private float minOffDuration = 0.5f;
    [SerializeField] private float maxOffDuration = 1.5f;
    [SerializeField] private float fadeSpeed = 2f;     // フェードの速さ
    [SerializeField] private float maxIntensity = 2f;  // 明るいときの強さ
    [SerializeField] private float minIntensity = 0f;  // 暗いときの強さ
    [SerializeField] private bool alwaysOn = false;

    private void Start()
    {
        if (lightComponents == null || lightComponents.Length == 0)
        {
            lightComponents = GetComponentsInChildren<Light>(); // 子にあるライトを自動取得
        }

        foreach (var light in lightComponents)
        {
            if (light != null)
            {
                StartCoroutine(Blink(light));
            }
        }
    }

    private IEnumerator Blink(Light targetLight)
    {
        while (true)
        {
            if (alwaysOn)
            {
                targetLight.intensity = maxIntensity;
                yield break;
            }

            // フェードイン（暗 → 明）
            float onDuration = Random.Range(minOnDuration, maxOnDuration);
            yield return StartCoroutine(FadeLight(targetLight, minIntensity, maxIntensity, onDuration));

            // フェードアウト（明 → 暗）
            float offDuration = Random.Range(minOffDuration, maxOffDuration);
            yield return StartCoroutine(FadeLight(targetLight, maxIntensity, minIntensity, offDuration));
        }
    }

    private IEnumerator FadeLight(Light targetLight, float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime * fadeSpeed;
            float t = Mathf.Clamp01(elapsed / duration);
            targetLight.intensity = Mathf.Lerp(from, to, t);
            yield return null;
        }
        targetLight.intensity = to;
    }
}
