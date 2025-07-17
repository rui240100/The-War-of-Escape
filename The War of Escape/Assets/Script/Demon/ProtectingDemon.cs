using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class ProtectingDemon : MonoBehaviour
{
    private NavMeshAgent agent; // NavMeshAgent コンポーネント

    public GameObject player1;
    public GameObject player2;
    public GameObject prisonGateObject;

    private PrisonGate prisonGateScript;
    private TriggerCamera triggerCameraScript;
    private Player playerScript;

    private Transform owner;
    private bool ownerFlag;

    private string useMessagePr1 = "アイテムを使用しました";
    private string useMessagePr2 = "アイテムが使用されました";
    private GameObject itemUse;
    private ItemUse itemUseSc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ownerFlag = false;
        agent = GetComponent<NavMeshAgent>(); // エージェント取得
        prisonGateScript = prisonGateObject.GetComponent<PrisonGate>();

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
        if (ownerFlag)
        {
            UpdateLaunch();
        }
    }

    public void Launch() 
    {

        if (prisonGateScript.playerID)
        {        
            var triggerCandidates = player1.GetComponentsInChildren<TriggerCamera>(true);

            triggerCameraScript = triggerCandidates.Length > 0 ? triggerCandidates[0] : null;

            if (triggerCameraScript == null)
            {
                return;
            }
            else
            {
                triggerCameraScript.demonHave = true;
            }

            //triggerCamera = player1.GetComponent<TriggerCamera>();
            //triggerCamera.demonHave = true;

            playerScript = player1.GetComponent<Player>();
            playerScript.pd = prisonGateScript.protectingDemon;


            player1.transform.position = owner.transform.position;

        }
        else if (!prisonGateScript.playerID)
        {
            var triggerCandidates = player2.GetComponentsInChildren<TriggerCamera>(true);
            triggerCameraScript = triggerCandidates.Length > 0 ? triggerCandidates[0] : null;

            if (triggerCameraScript == null)
            {
                return;
            }
            else
            { 
                triggerCameraScript.demonHave = true;
            }

            playerScript = player2.GetComponent<Player>();
            playerScript.pd = prisonGateScript.protectingDemon;

            player2.transform.position = owner.transform.position;
        }
        ownerFlag = true;

        if (itemUseSc != null)
        {
            if (playerScript.playerID == 1)
            {
                string message1 = useMessagePr1;
                string message2 = useMessagePr2;
                itemUseSc.ShowMessage(message1, message2);
            }
            else if (playerScript.playerID == 2)
            {
                string message1 = useMessagePr2;
                string message2 = useMessagePr1;
                itemUseSc.ShowMessage(message1, message2);
            }
        }
    }

    private void UpdateLaunch()
    {
        Vector3 targetPosition = owner.transform.position - owner.forward * 5f;
        agent.SetDestination(targetPosition);
    }

    public void SetOwner(Player newOwner)
    {
        owner = newOwner.transform;
    }
}

