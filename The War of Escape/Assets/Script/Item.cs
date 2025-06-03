using UnityEngine;

public class Item : MonoBehaviour
{
    public float slowDuration = 5f;
    public float slowMultiplier = 0.2f;

    public Sprite icon;
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
        if (player != null && !player.HasItem)
        {
            player.SetHeldItem(this); 
            transform.SetParent(player.transform);
            transform.localPosition = Vector3.zero;
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }



    /*void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && !player.HasItem)
        {
            player.heldItem = this;
            transform.SetParent(player.transform); // 持ち主にくっつける（任意）
            transform.localPosition = Vector3.zero; // 表示位置も調整可
            GetComponent<Collider>().enabled = false; // 拾ったら当たり判定を無効に
            GetComponent<MeshRenderer>().enabled = false; // 見えなくする（任意）
        }
    }*/

    public void Activate(Player user)
    {
        if (user.otherPlayer != null)
        {
            user.StartCoroutine(user.otherPlayer.SlowDown(slowMultiplier, slowDuration));
        }

        Destroy(gameObject); // アイテムは使い捨て
    }
}




