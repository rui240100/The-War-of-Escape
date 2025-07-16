using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    [Header("点灯している時間（秒）")]
    [SerializeField] private float lightOnDuration = 1.0f;

    [Header("消灯している時間（秒）")]
    [SerializeField] private float lightOffDuration = 1.0f;

    private Light targetLight;
    private float timer;
    private bool isLightOn = true;

    void Start()
    {
        targetLight = GetComponent<Light>();
        if (targetLight == null)
        {
            Debug.LogWarning("Lightコンポーネントが見つかりません！");
            return;
        }

        targetLight.enabled = isLightOn;
        timer = lightOnDuration;
    }

    void Update()
    {
        if (targetLight == null) return;

        if (lightOffDuration <= 0f && !isLightOn)
        {
            // 消灯時間が0なのに今OFF → 点けて終了
            isLightOn = true;
            targetLight.enabled = true;
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isLightOn = !isLightOn;
            targetLight.enabled = isLightOn;

            timer = isLightOn ? lightOnDuration : lightOffDuration;
        }
    }

}
