using UnityEngine;
using UnityEngine.SceneManagement; // 脱出後にシーン遷移などしたい場合

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
                Debug.Log($"Player {player.playerID} が脱出に成功しました！");
                SceneManager.LoadScene("Clear"); // 脱出処理など
            }
            else
            {
                Debug.Log($"Player {player.playerID} は鍵が足りません！（{player.keyCount}/3）");
            }
        }
    }




}
