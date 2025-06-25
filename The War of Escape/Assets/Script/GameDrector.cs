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
        int second = (int)TimeCount % 60;



        timeUI.GetComponent<TextMeshProUGUI>().text = "0" + (int)TimeCount / 60 + ":" + second.ToString("00");
    }
}
