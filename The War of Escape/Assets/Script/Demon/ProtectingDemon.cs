using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class ProtectingDemon : MonoBehaviour
{
    private NavMeshAgent agent; // NavMeshAgent �R���|�[�l���g

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
        agent = GetComponent<NavMeshAgent>(); // �G�[�W�F���g�擾
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
                       

            Debug.Log("�v���C���[1�̏����ɓ�����");



            var triggerCandidates = player1.GetComponentsInChildren<TriggerCamera>(true);
            Debug.Log($"TriggerCamera �̐�: {triggerCandidates.Length}");

            triggerCamera = triggerCandidates.Length > 0 ? triggerCandidates[0] : null;
            Debug.Log($"triggerCamera �� null �ł����H: {triggerCamera == null}");

            if (triggerCamera == null)
            {
                Debug.LogError("TriggerCamera �� GetComponentsInChildren �ł�������܂���ł�����I");
                return;
            }
            else
            {
                Debug.Log("TriggerCamera �𐳏�Ɏ擾�ł��܂�����I");

                Debug.Log($"triggerCamera.enabled: {triggerCamera.enabled}, triggerCamera.gameObject.activeSelf: {triggerCamera.gameObject.activeSelf}");

                triggerCamera.demonHave = true;
            }



            /*var triggerCandidates = player1.GetComponentsInChildren<TriggerCamera>(true);*/
            /*triggerCamera = player1.GetComponentInChildren<TriggerCamera>();*/


            //triggerCamera = player1.GetComponent<TriggerCamera>();
            //triggerCamera.demonHave = true;


            if (triggerCamera == null)
            {
                Debug.LogError("TriggerCamera �� GetComponentsInChildren �ł�������܂���ł�����I");
                return;
            }
            else
            {
                Debug.Log("TriggerCamera �𐳏�Ɏ擾�ł��܂�����I");
                triggerCamera.demonHave = true;
            }





            player = player1.GetComponent<Player>();
            player.pd = prisonGate.protectingDemon;

            


            player1.transform.position = owner.transform.position;

        }
        else if (!prisonGate.playerID) 
        {
            triggerCamera = player2.GetComponent<TriggerCamera>();
            triggerCamera.demonHave= true;

            player = player2.GetComponent<Player>();
            player.pd = prisonGate.protectingDemon;

            player2.transform.position = owner.transform.position;
        }

        Debug.Log("owner �₻�̑��̃`�F�b�N�����������̂ŁA�ړI�n���v�Z���܂���I");
        Vector3 targetPosition = owner.transform.position - owner.forward * 5f;
        Debug.Log($"�S�̖ړI�n: {targetPosition} �Ɍ������܂��I");
        



        //Debug.Log($"targetPosition: {targetPosition}");
        agent.SetDestination(targetPosition);
        Debug.Log("SetDestination ���Ăяo���܂�����I");

    }


    public void SetOwner(Player newOwner)
    {
        owner = newOwner.transform;
        //Debug.Log($"�S���v���C���[{newOwner.playerID}�̂��̂ɂȂ����I");
    }


    public void Stun()
    {

    }

    //���R�������牺�ǉ����܂���
   /* public void SetOwner(Player newOwner)
    {
        owner = newOwner;
        Debug.Log($"�S���v���C���[{owner.playerID}�̂��̂ɂȂ����I");
        // ������AI��Ǐ]��Ȃǂ�؂�ւ��鏈���������I
    }*/
}

