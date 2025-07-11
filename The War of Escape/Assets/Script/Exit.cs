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

                ResultData.escapedPlayerID = player.playerID;
                FadeManager.Instance.LoadScene("Clear", 2.0f);
                
            }
            else
            {
                Debug.Log($"Player {player.playerID} は鍵が足りません！（{player.keyCount}/3）");
            }
        }
    }




}
