using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CallNecromancer : Item
{
    public GameObject Necromancer;
    private Necromancer necromancerScript;
    private Player playerScriptNe;
    public Vector3[] NecSpawn;
    public float checkRadius = 1.0f;

    public string useMessageNe1 = "鬼を召喚しました";
    public string useMessageNe2 = "";
    private GameObject itemUseNe;
    private ItemUse itemUseScNe;

    private void Start()
    {
        // シーン内のMessageDisplayManagerを探す
        itemUseNe = GameObject.Find("ItemUse");
        itemUseScNe = itemUseNe.GetComponent<ItemUse>();
        if (itemUseScNe == null)
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

            playerScriptNe = user.GetComponent<Player>();
            if (playerScriptNe.playerID == 1)
            {
                necromancerScript.player2 = true;
            }
            else if (playerScriptNe.playerID == 2)
            {
                necromancerScript.player1 = true;
            }
        }
        else
        {
            Debug.Log("No safe position");
        }

        if (itemUseScNe != null)
        {
            if (playerScriptNe.playerID == 1)
            {
                string message1 = useMessageNe1;
                string message2 = useMessageNe2;
                itemUseScNe.ShowMessage(message1, message2);
            }
            else if(playerScriptNe.playerID == 2)
            {
                string message1 = useMessageNe2;
                string message2 = useMessageNe1;
                itemUseScNe.ShowMessage(message1, message2);
            }
        }
    }
}
