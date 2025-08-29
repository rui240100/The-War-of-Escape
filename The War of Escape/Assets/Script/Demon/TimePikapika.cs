using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class TimePikapika : MonoBehaviour
{
    public Image time;
    private float blinkSpeed = 2.0f;

    private float alpha = 1.0f;
    private bool fadingOut = true;

    public GameDrector gameDrector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameDrector.TimeCount <= 60)
        {
            Debug.Log("********************UI点滅");
            // アルファ値の変化
            float delta = blinkSpeed * UnityEngine.Time.deltaTime;

            if (fadingOut)
            {
                alpha -= delta;
                if (alpha <= 0f)
                {
                    alpha = 0f;
                    fadingOut = false;
                }
            }
            else
            {
                alpha += delta;
                if (alpha >= 1f)
                {
                    alpha = 1f;
                    fadingOut = true;
                }
            }

            Color color = time.color;
            color.a = alpha;
            time.color = color;
        }
    }
}

