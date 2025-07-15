using System.Collections;
using UnityEngine;
using TMPro;

public class ItemUse : MonoBehaviour
{
    public TextMeshProUGUI messageText;
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

    public void ShowMessage(string message)
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(ShowMessageCoroutine(message));
    }

    private IEnumerator ShowMessageCoroutine(string message)
    {
        messageText.text = message;
        messageText.enabled = true;

        yield return new WaitForSeconds(displayDuration);

        messageText.enabled = false;
    }
}
