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
        Debug.Log("Launch(): �J�n");

        if (prisonGate == null)
        {
            Debug.LogError("prisonGate �� null �ł��I");
        }
        else
        {
            Debug.Log($"prisonGate.playerID: {prisonGate.playerID}");
        }

        if (owner == null)
        {
            Debug.LogError("owner �� null �ł��I");
        }
        else
        {
            Debug.Log($"owner: {owner.name}");
        }

        if (player1 == null || player2 == null)
        {
            Debug.LogError("player1 �܂��� player2 �� null �ł��I");
        }

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent ���擾�ł��Ă��܂���I");
        }
        else
        {
            Debug.Log("NavMeshAgent �擾 OK");
        }

        Debug.Log("Launch(): ��������O");


        if (prisonGate.playerID)
        {

            Debug.Log("�v���C���[1�̏����ɓ�����");

            triggerCamera = player1.GetComponentInChildren<TriggerCamera>();
            //triggerCamera = player1.GetComponent<TriggerCamera>();
            if (triggerCamera == null)
            {
                Debug.LogError("player1 �� TriggerCamera ���A�^�b�`����Ă��܂���I");
            }
            else
            {
                Debug.Log("player1 �� TriggerCamera ���擾�ł��܂����I");
                triggerCamera.demonHave = true;
            }

            triggerCamera = player1.GetComponent<TriggerCamera>();
            triggerCamera.demonHave = true;

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

        Vector3 targetPosition = owner.transform.position - owner.forward * 5f;
             
        Debug.Log($"�S�̖ړI�n: {targetPosition} �Ɍ������܂��I");



        //Debug.Log($"targetPosition: {targetPosition}");
        agent.SetDestination(targetPosition);
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

