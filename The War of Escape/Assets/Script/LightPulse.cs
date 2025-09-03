using UnityEngine;

public class MultiLightPulseRandom : MonoBehaviour
{
    public Light[] pointLights;     // 複数のライトをアサイン
    public float speed = 2f;        // 点滅の速さ
    public float maxIntensity = 3f; // 明るさの最大値
    public float minIntensity = 0f; // 明るさの最小値

    private float[] offsets; // 各ライトごとのオフセット

    void Start()
    {
        offsets = new float[pointLights.Length];
        for (int i = 0; i < pointLights.Length; i++)
        {
            offsets[i] = Random.Range(0f, 10f); // ランダムな位相ずれ
        }
    }

    void Update()
    {
        for (int i = 0; i < pointLights.Length; i++)
        {
            if (pointLights[i] != null)
            {
                float t = Mathf.PingPong((Time.time + offsets[i]) * speed, 1f);
                pointLights[i].intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
            }
        }
    }
}
