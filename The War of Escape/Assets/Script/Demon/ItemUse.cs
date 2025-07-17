using System.Collections;
using UnityEngine;
using TMPro;

public class ItemUse : MonoBehaviour
{
    public TextMeshProUGUI messageText1;
    public TextMeshProUGUI messageText2;
    public float displayDuration = 2.0f;

    private Coroutine currentCoroutine;

    public float fadeDuration = 2f;  // フェード時間
    private float currentTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMessage(string message1,string message2)
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(ShowMessageCoroutine(message1,message2));
    }

    private IEnumerator ShowMessageCoroutine(string message1,string message2)
    {
        messageText1.text = message1;
        messageText2.text = message2;
        messageText1.enabled = true;
        messageText2.enabled = true;

        yield return new WaitForSeconds(displayDuration);  

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Debug.Log("FadeOut started");
        currentTime = 0f;
        Color originalColor1 = messageText1.color;
        Color originalColor2 = messageText2.color;

        while (currentTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration);
            messageText1.color = new Color(originalColor1.r, originalColor1.g, originalColor1.b, alpha);
            messageText2.color = new Color(originalColor2.r, originalColor2.g, originalColor2.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        // 完全に透明にする
        messageText1.color = new Color(originalColor1.r, originalColor1.g, originalColor1.b, 0f);
        messageText2.color = new Color(originalColor2.r, originalColor2.g, originalColor2.b, 0f);

        messageText1.enabled = false;
        messageText2.enabled = false;
        Debug.Log("FadeOut completed");
    }
}
