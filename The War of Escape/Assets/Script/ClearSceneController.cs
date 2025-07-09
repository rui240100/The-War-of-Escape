using UnityEngine;
using UnityEngine.SceneManagement;


public class ClearSceneController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // XboxパッドのBボタンは "JoystickButton1" に対応
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            // タイトルシーンへ移動（"TitleScene" は実際のシーン名に変えてね）
            SceneManager.LoadScene("TitleScene");
        }
    }
}
