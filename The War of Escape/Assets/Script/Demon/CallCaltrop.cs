using UnityEngine;

public class CallCaltrop : Item
{
    public GameObject caltrop;
    private Player playerScriptCo;

    private string useMessageCa1 = "まきびしを設置しました";
    private string useMessageCa2 = "";
    private GameObject itemUseCo;
    private ItemUse itemUseScCo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // シーン内のMessageDisplayManagerを探す
        itemUseCo = GameObject.Find("ItemUse");
        itemUseScCo = itemUseCo.GetComponent<ItemUse>();
        if (itemUseScCo == null)
        {
            Debug.Log("nothingで");
        }
        else
        {
            Debug.Log("見つけた");
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
        caltropObj.transform.position = new Vector3(user.transform.position.x, user.transform.position.y - 1.5f, user.transform.position.z);
        Debug.Log("まきびし設置1");
        playerScriptCo = user.GetComponent<Player>();

        if (itemUseScCo != null)
        {
            if (playerScriptCo.playerID == 1)
            {
                string message1 = useMessageCa1;
                string message2 = useMessageCa2;
                itemUseScCo.ShowMessage(message1, message2);
            }
            else if (playerScriptCo.playerID == 2)
            {
                string message1 = useMessageCa2;
                string message2 = useMessageCa1;
                itemUseScCo.ShowMessage(message1, message2);
            }
            Debug.Log("メッセージ渡した");
        }
        else
        {
            Debug.Log("メッセージ渡せなかった");
        }
            user.SetHeldItem(null);
    }
}
