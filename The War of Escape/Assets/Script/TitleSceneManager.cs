using UnityEngine;
using UnityEngine.SceneManagement;

public class TitlesceneManager : MonoBehaviour
{
   

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {

            FadeManager.Instance.LoadScene("Stage", 2.0f);
        }





    }
}

    
