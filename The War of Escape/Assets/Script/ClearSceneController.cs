using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ClearSceneController : MonoBehaviour
{
    public TextMeshProUGUI resultText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int id = ResultData.escapedPlayerID;

        if (id != -1)
        {
            resultText.text = $"Player {id} 脱出成功！";
        }
        else
        {
            resultText.text = "誰も脱出していません";
        }
    }

    // Update is called once per frame
    void Update()
    {

        // XboxパッドのBボタンは "JoystickButton1" に対応
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            
            FadeManager.Instance.LoadScene("TitleScene", 2.0f);
        }
    }
}
