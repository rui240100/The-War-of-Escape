using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class ProtectingDemon : MonoBehaviour
{
    private NavMeshAgent agent; // NavMeshAgent �R���|�[�l���g

    public GameObject player1;
    public GameObject player2;

    private PrisonGate prisonGate;
    private TriggerCamera triggerCamera;
    private Player player;

    private Transform owner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // �G�[�W�F���g�擾
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Launch() 
    {
        if (prisonGate.playerID)
        {
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
        agent.SetDestination(targetPosition);
    }

    public void Stun()
    {

    }

    //���R�������牺�ǉ����܂���
    //public void SetOwner(Player newOwner)
    //{
    //    owner = newOwner;
    //    Debug.Log($"�S���v���C���[{owner.playerID}�̂��̂ɂȂ����I");
    //    // ������AI��Ǐ]��Ȃǂ�؂�ւ��鏈���������I
    //}
}

