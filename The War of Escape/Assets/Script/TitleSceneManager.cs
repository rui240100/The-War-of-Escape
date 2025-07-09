using UnityEngine;
using UnityEngine.SceneManagement;

public class TitlesceneManager : MonoBehaviour
{
    [Header("遷移先シーン名")]
    [SerializeField] private string nextSceneName = "MainScene"; // 遷移させたいシーン名を設定

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            // タイトルシーンへ移動（"TitleScene" は実際のシーン名に変えてね）
            SceneManager.LoadScene("Stage");
        }




        /*if (AnyButtonPressed())
        {
            SceneManager.LoadScene(nextSceneName);
        }*/
    }

    // 何かしらのボタンが押されたか判定
    /*bool AnyButtonPressed()
    {
        // Xboxコントローラー含め、登録されている全ボタンをチェック
        for (int i = 0; i < 20; i++) // ボタン数は必要に応じて調整
        {
            if (Input.GetKeyDown("joystick button " + i))
            {
                return true;
            }
        }

        return false;
    }*/
}
