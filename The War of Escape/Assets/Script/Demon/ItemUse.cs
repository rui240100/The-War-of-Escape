using System.Collections;
using UnityEngine;
using TMPro;

public class ItemUse : MonoBehaviour
{
    public TextMeshProUGUI messageText1;
    public TextMeshProUGUI messageText2;
    public float displayDuration = 2.0f;

    private Coroutine currentCoroutine;

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

        messageText1.enabled = false;
        messageText2.enabled = false;
    }
}
