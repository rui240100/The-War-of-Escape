using UnityEngine;

public class Item : MonoBehaviour
{
    public float slowDuration = 5f;
    public float slowMultiplier = 0.2f;

    public Sprite icon;

    private Player playerScript;
    public string useMessageSl1 = "アイテムを使用しました";
    public string useMessageSl2 = "アイテムが使用されました";
    private GameObject itemUse;
    private ItemUse itemUseSc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // シーン内のMessageDisplayManagerを探す
        itemUse = GameObject.Find("ItemUse");
        itemUseSc = itemUse.GetComponent<ItemUse>();
        if (itemUseSc == null)
        {
            Debug.LogError("MessageDisplayManagerがシーンにありません！");
        }
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

    public virtual void Activate(Player user)
    {
        if (user.otherPlayer != null)
        {
            user.StartCoroutine(user.otherPlayer.SlowDown(slowMultiplier, slowDuration));
        }

        if (itemUseSc != null)
        {
            if (playerScript.playerID == 1)
            {
                string message1 = useMessageSl1;
                string message2 = useMessageSl2;
                itemUseSc.ShowMessage(message1, message2);
            }
            else if (playerScript.playerID == 2)
            {
                string message1 = useMessageSl2;
                string message2 = useMessageSl1;
                itemUseSc.ShowMessage(message1, message2);
            }
        }

        Destroy(gameObject); // アイテムは使い捨て
    }
}




