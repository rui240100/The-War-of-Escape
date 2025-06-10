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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ownerFlag = false;
        agent = GetComponent<NavMeshAgent>(); // エージェント取得
        prisonGateScript = prisonGateObject.GetComponent<PrisonGate>();
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
                 
            //Debug.Log("プレイヤー1の処理に入った");

            var triggerCandidates = player1.GetComponentsInChildren<TriggerCamera>(true);
            //Debug.Log($"TriggerCamera の数: {triggerCandidates.Length}");

            triggerCameraScript = triggerCandidates.Length > 0 ? triggerCandidates[0] : null;
            //Debug.Log($"triggerCamera は null ですか？: {triggerCamera == null}");

            if (triggerCameraScript == null)
            {
                Debug.LogError("TriggerCamera が GetComponentsInChildren でも見つかりませんでしたわ！");
                return;
            }
            else
            {
                Debug.Log("TriggerCamera を正常に取得できましたわ！");

                Debug.Log($"triggerCamera.enabled: {triggerCameraScript.enabled}, triggerCamera.gameObject.activeSelf: {triggerCameraScript.gameObject.activeSelf}");

                triggerCameraScript.demonHave = true;
            }



            /*var triggerCandidates = player1.GetComponentsInChildren<TriggerCamera>(true);
            triggerCamera = player1.GetComponentInChildren<TriggerCamera>();*/


            //triggerCamera = player1.GetComponent<TriggerCamera>();
            //triggerCamera.demonHave = true;


            if (triggerCameraScript == null)
            {
                Debug.LogError("TriggerCamera が GetComponentsInChildren でも見つかりませんでしたわ！");
                return;
            }
            else
            {
                Debug.Log("TriggerCamera を正常に取得できましたわ！");
                triggerCameraScript.demonHave = true;
            }

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
                Debug.LogError("2P側 TriggerCamera が GetComponentsInChildren でも見つかりませんでしたわ！");
                return;
            }
            else
            {
                Debug.Log("2P側 TriggerCamera を正常に取得できましたわ！");
                Debug.Log($"triggerCamera.enabled: {triggerCameraScript.enabled}, triggerCamera.gameObject.activeSelf: {triggerCameraScript.gameObject.activeSelf}");

                triggerCameraScript.demonHave = true;
            }

            playerScript = player2.GetComponent<Player>();
            playerScript.pd = prisonGateScript.protectingDemon;

            player2.transform.position = owner.transform.position;
        }

        ownerFlag = true;

        Debug.Log("owner やその他のチェックが完了したので、目的地を計算しますわ！");
        
        //Debug.Log($"鬼の目的地: {targetPosition} に向かいます！");
        



        //Debug.Log($"targetPosition: {targetPosition}");
        
        Debug.Log("SetDestination を呼び出しましたわ！");

    }

    private void UpdateLaunch()
    {
        Vector3 targetPosition = owner.transform.position - owner.forward * 5f;
        agent.SetDestination(targetPosition);
    }

    public void SetOwner(Player newOwner)
    {
        owner = newOwner.transform;
        //Debug.Log($"鬼がプレイヤー{newOwner.playerID}のものになった！");
    }
}

