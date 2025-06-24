using UnityEngine;
using TMPro;
public class GameDrector : MonoBehaviour
{
    public GameObject timeUI;

    float TimeCount = 300;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeUI.GetComponent<TextMeshProUGUI>().text = "0:00";
    }

    // Update is called once per frame
    void Update()
    {
        TimeCount -= Time.deltaTime;
        timeUI.GetComponent<TextMeshProUGUI>().text = "" + (int)TimeCount / 60 + ":" + (int)TimeCount % 60;
    }
}
