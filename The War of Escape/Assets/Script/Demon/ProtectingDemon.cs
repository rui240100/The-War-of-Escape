using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class ProtectingDemon : MonoBehaviour
{
    private NavMeshAgent agent; // NavMeshAgent コンポーネント

    public GameObject player1;
    public GameObject player2;
    public GameObject prisonGateObject;

    private PrisonGate prisonGate;
    private TriggerCamera triggerCamera;
    private Player player;

    private Transform owner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // エージェント取得
        prisonGate = prisonGateObject.GetComponent<PrisonGate>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Launch() 
    {

        if (prisonGate.playerID)
        {
                 
            //Debug.Log("プレイヤー1の処理に入った");

            var triggerCandidates = player1.GetComponentsInChildren<TriggerCamera>(true);
            //Debug.Log($"TriggerCamera の数: {triggerCandidates.Length}");

            triggerCamera = triggerCandidates.Length > 0 ? triggerCandidates[0] : null;
            //Debug.Log($"triggerCamera は null ですか？: {triggerCamera == null}");

            if (triggerCamera == null)
            {
                Debug.LogError("TriggerCamera が GetComponentsInChildren でも見つかりませんでしたわ！");
                return;
            }
            else
            {
                Debug.Log("TriggerCamera を正常に取得できましたわ！");

                Debug.Log($"triggerCamera.enabled: {triggerCamera.enabled}, triggerCamera.gameObject.activeSelf: {triggerCamera.gameObject.activeSelf}");

                triggerCamera.demonHave = true;
            }



            /*var triggerCandidates = player1.GetComponentsInChildren<TriggerCamera>(true);
            triggerCamera = player1.GetComponentInChildren<TriggerCamera>();*/


            //triggerCamera = player1.GetComponent<TriggerCamera>();
            //triggerCamera.demonHave = true;


            if (triggerCamera == null)
            {
                Debug.LogError("TriggerCamera が GetComponentsInChildren でも見つかりませんでしたわ！");
                return;
            }
            else
            {
                Debug.Log("TriggerCamera を正常に取得できましたわ！");
                triggerCamera.demonHave = true;
            }

            player = player1.GetComponent<Player>();
            player.pd = prisonGate.protectingDemon;


            player1.transform.position = owner.transform.position;

        }
        else if (!prisonGate.playerID)
        {
            var triggerCandidates = player2.GetComponentsInChildren<TriggerCamera>(true);
            triggerCamera = triggerCandidates.Length > 0 ? triggerCandidates[0] : null;

            if (triggerCamera == null)
            {
                Debug.LogError("2P側 TriggerCamera が GetComponentsInChildren でも見つかりませんでしたわ！");
                return;
            }
            else
            {
                Debug.Log("2P側 TriggerCamera を正常に取得できましたわ！");
                Debug.Log($"triggerCamera.enabled: {triggerCamera.enabled}, triggerCamera.gameObject.activeSelf: {triggerCamera.gameObject.activeSelf}");

                triggerCamera.demonHave = true;
            }

            player = player2.GetComponent<Player>();
            player.pd = prisonGate.protectingDemon;

            player2.transform.position = owner.transform.position;
        }

        Debug.Log("owner やその他のチェックが完了したので、目的地を計算しますわ！");
        Vector3 targetPosition = owner.transform.position - owner.forward * 5f;
        Debug.Log($"鬼の目的地: {targetPosition} に向かいます！");
        



        //Debug.Log($"targetPosition: {targetPosition}");
        agent.SetDestination(targetPosition);
        Debug.Log("SetDestination を呼び出しましたわ！");

    }


    public void SetOwner(Player newOwner)
    {
        owner = newOwner.transform;
        //Debug.Log($"鬼がプレイヤー{newOwner.playerID}のものになった！");
    }


    public void Stun()
    {

    }

    
}

