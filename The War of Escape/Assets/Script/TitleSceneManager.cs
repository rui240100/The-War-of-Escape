using UnityEngine;
using UnityEngine.SceneManagement;

public class TitlesceneManager : MonoBehaviour
{
    [System.Obsolete]
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {

            FadeManager.Instance.LoadScene("Stage-SIN", 2.0f);
        }





    }
}

    
