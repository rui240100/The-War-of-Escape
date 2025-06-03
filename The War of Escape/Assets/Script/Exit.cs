using UnityEngine;
using UnityEngine.SceneManagement; // �E�o��ɃV�[���J�ڂȂǂ������ꍇ

public class Exit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            if (player.keyCount >= 3)
            {
                Debug.Log($"Player {player.playerID} ���E�o�ɐ������܂����I");
                SceneManager.LoadScene("Clear"); // �E�o�����Ȃ�
            }
            else
            {
                Debug.Log($"Player {player.playerID} �͌�������܂���I�i{player.keyCount}/3�j");
            }
        }
    }




}
