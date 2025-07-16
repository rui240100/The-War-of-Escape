using UnityEngine;

public class CallCaltrop : Item
{
    public GameObject caltrop;
    private Player playerScript;

    public string useMessageCa1 = "アイテムを使用しました";
    public string useMessageCa2 = "アイテムが使用されました";
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

    public override void Activate(Player user)
    {
        Debug.Log("まきびし設置0");
        GameObject caltropObj = Instantiate(caltrop);
        caltropObj.transform.position = user.transform.position;
        Debug.Log("まきびし設置1");
        playerScript = user.GetComponent<Player>();

        if (itemUseSc != null)
        {
            if (playerScript.playerID == 1)
            {
                string message1 = useMessageCa1;
                string message2 = useMessageCa2;
                itemUseSc.ShowMessage(message1, message2);
            }
            else if (playerScript.playerID == 2)
            {
                string message1 = useMessageCa2;
                string message2 = useMessageCa1;
                itemUseSc.ShowMessage(message1, message2);
            }
        }
    }
}
