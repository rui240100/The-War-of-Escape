using UnityEngine;
using UnityEngine.UI;

public class ChaseUI : MonoBehaviour
{
    public Image player1UI;     // フェード対象のImage
    public Image player2UI;
    public bool player1 = false;
    public bool player2 = false;
    public float blinkSpeed = 1.0f;

    private float alpha = 1.0f;
    private bool fadingOut = true;

    void Update()
    {
        if (player1 && player1UI != null)
        {
            // アルファ値の変化
            float delta = blinkSpeed * Time.deltaTime;

            if (fadingOut)
            {
                //Color color1 = player1UI.color;
                //color1.a = 0.0f;
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

            Color color = player1UI.color;
            color.a = alpha;
            player1UI.color = color;
        }
        else if (!player1 && player1UI != null)
        {
            // 点滅が無効になったらαを1に戻す
            Color color = player1UI.color;
            color.a = 0.0f;
            player1UI.color = color;
        }

        if (player2 && player2UI != null)
        {
            // アルファ値の変化
            float delta = blinkSpeed * Time.deltaTime;

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

            Color color = player2UI.color;
            color.a = alpha;
            player2UI.color = color;
        }
        else if (!player2 && player2UI != null)
        {
            // 点滅が無効になったらαを1に戻す
            Color color = player2UI.color;
            color.a = 0.0f;
            player2UI.color = color;
        }
    }
}

