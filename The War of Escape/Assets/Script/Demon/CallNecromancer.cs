using System.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CallNecromancer : Item
{
    public GameObject Necromancer;
    private Necromancer necromancerScript;
    private Player playerScriptNe;
    public Vector3[] NecSpawn;
    public float checkRadius = 0.5f;

    private string useMessageNe1 = "鬼を召喚しました";
    private string useMessageNe2 = "相手プレイヤーによって鬼が召喚されました";
    private string useMessageNe12 = "30秒間相手プレイヤーを追跡します";
    private string useMessageNe22 = "30秒間あなたを追跡します";
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
        playerScriptNe = user.GetComponent<Player>();
        GameObject NecromancerObj = Instantiate(Necromancer);
        NecromancerObj.transform.position = user.transform.position;
        necromancerScript = NecromancerObj.GetComponent<Necromancer>();

        if (playerScriptNe.playerID == 1)
        {
            necromancerScript.player2 = true;
        }
        else if (playerScriptNe.playerID == 2)
        {
            necromancerScript.player1 = true;
        }

        if (itemUseScNe != null)
        {
            if (playerScriptNe.playerID == 1)
            {
                string message1 = useMessageNe1;
                string message2 = useMessageNe2;
                itemUseScNe.ShowMessage(message1, message2);
                string message3 = useMessageNe12;
                string message4 = useMessageNe22;
                StartCoroutine(Wait(message3, message4));
            }
            else if (playerScriptNe.playerID == 2)
            {
                string message1 = useMessageNe2;
                string message2 = useMessageNe1;
                itemUseScNe.ShowMessage(message1, message2);
                message1 = useMessageNe22;
                message2 = useMessageNe12;
                StartCoroutine(Wait(message1, message2));
            }
        }

        //Vector3[] NecSpawn = new Vector3[5];

        //Vector3 safePosition = Vector3.zero;
        //bool foundSafe = false;

        //NecSpawn[1] = new Vector3(user.transform.position.x + 1.0f, user.transform.position.y, user.transform.position.z);
        //NecSpawn[2] = new Vector3(user.transform.position.x - 1.0f, user.transform.position.y, user.transform.position.z);
        //NecSpawn[3] = new Vector3(user.transform.position.x, user.transform.position.y, user.transform.position.z + 1.0f);
        //NecSpawn[4] = new Vector3(user.transform.position.x, user.transform.position.y, user.transform.position.z - 1.0f);

        //foreach (Vector3 position in NecSpawn)
        //{
        //    Collider[] hits = Physics.OverlapSphere(position,checkRadius);

        //    if (hits.Length == 0)
        //    {
        //        safePosition = position;
        //        foundSafe = true;
        //        break; // 最初に見つけた安全な場所で止める
        //    }
        //}

        //GameObject NecromancerObj;

        //if (foundSafe)
        //{
        //    NecromancerObj = Instantiate(Necromancer);
        //    NecromancerObj.transform.position = safePosition;
        //    necromancerScript = NecromancerObj.GetComponent<Necromancer>();

        //    if (playerScriptNe.playerID == 1)
        //    {
        //        necromancerScript.player2 = true;
        //    }
        //    else if (playerScriptNe.playerID == 2)
        //    {
        //        necromancerScript.player1 = true;
        //    }

        //    if (itemUseScNe != null)
        //    {
        //        if (playerScriptNe.playerID == 1)
        //        {
        //            string message1 = useMessageNe1;
        //            string message2 = useMessageNe2;
        //            itemUseScNe.ShowMessage(message1, message2);
        //            string message3 = useMessageNe12;
        //            string message4 = useMessageNe22;
        //            StartCoroutine(Wait(message3, message4));
        //        }
        //        else if (playerScriptNe.playerID == 2)
        //        {
        //            string message1 = useMessageNe2;
        //            string message2 = useMessageNe1;
        //            itemUseScNe.ShowMessage(message1, message2);
        //            message1 = useMessageNe22;
        //            message2 = useMessageNe12;
        //            StartCoroutine(Wait(message1, message2));
        //        }
        //    }
        //    Destroy(this.gameObject);
        //}
        //else
        //{
        //    Debug.Log("No safe position");
        //}
    }

    private IEnumerator Wait(string message1, string message2)
    {
        yield return new WaitForSeconds(3.0f);

        itemUseScNe.ShowMessage(message1, message2);
        Debug.Log("sended");
        Destroy(this.gameObject);
    }
}
