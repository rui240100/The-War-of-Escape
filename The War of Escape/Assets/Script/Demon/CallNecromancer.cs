using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CallNecromancer : Item
{
    public GameObject Necromancer;
    private Necromancer necromancerScript;
    private Player playerScript;
    public Vector3[] NecSpawn;
    public float checkRadius = 1.0f;

    public string useMessageNe1 = "アイテムを使用しました";
    public string useMessageNe2 = "アイテムが使用されました";
    private GameObject itemUse;
    private ItemUse itemUseSc;

    private void Start()
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
        Vector3[] NecSpawn = new Vector3[5];

        Vector3 safePosition = Vector3.zero;
        bool foundSafe = false;

        NecSpawn[1] = new Vector3(user.transform.position.x + 2.0f, user.transform.position.y, user.transform.position.z);
        NecSpawn[2] = new Vector3(user.transform.position.x - 2.0f, user.transform.position.y, user.transform.position.z);
        NecSpawn[3] = new Vector3(user.transform.position.x, user.transform.position.y, user.transform.position.z + 2.0f);
        NecSpawn[4] = new Vector3(user.transform.position.x, user.transform.position.y, user.transform.position.z - 2.0f);
        
        foreach (Vector3 position in NecSpawn)
        {
            Collider[] hits = Physics.OverlapSphere(position,checkRadius);

            if (hits.Length == 0)
            {
                safePosition = position;
                foundSafe = true;
                break; // 最初に見つけた安全な場所で止める
            }
        }

        GameObject NecromancerObj;

        if (foundSafe)
        {
            NecromancerObj = Instantiate(Necromancer);
            NecromancerObj.transform.position = safePosition;
            necromancerScript = NecromancerObj.GetComponent<Necromancer>();

            playerScript = user.GetComponent<Player>();
            if (playerScript.playerID == 1)
            {
                necromancerScript.player2 = true;
            }
            else if (playerScript.playerID == 2)
            {
                necromancerScript.player1 = true;
            }
        }
        else
        {
            Debug.Log("No safe position");
        }

        if (itemUseSc != null)
        {
            if (playerScript.playerID == 1)
            {
                string message1 = useMessageNe1;
                string message2 = useMessageNe2;
                itemUseSc.ShowMessage(message1, message2);
            }
            else if(playerScript.playerID == 2)
            {
                string message1 = useMessageNe2;
                string message2 = useMessageNe1;
                itemUseSc.ShowMessage(message1, message2);
            }
        }
    }
}
