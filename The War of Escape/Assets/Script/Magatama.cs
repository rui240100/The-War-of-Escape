using UnityEngine;

public class Magatama : MonoBehaviour
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
        //Debug.Log("衝突相手: " + other.name); // ← これ追加！

        Player player = other.GetComponentInParent<Player>();
        if (player != null)
        {

            //Debug.Log($"PlayerID: {player.playerID} が勾玉を取得");
            player.AddMagatama();
            Destroy(gameObject); // 取得後消す
        }
    }


}
