using UnityEngine;

public class KeyItem : MonoBehaviour
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
        //Debug.Log("何かが鍵に触れました！"); // 名前なしでもOK

        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.AddKey(); // プレイヤーに鍵を追加
            Destroy(gameObject); // 鍵を消す
        }
    }


}
