using UnityEngine;
using UnityEngine.AI;

public class ProtectingDemon : MonoBehaviour
{
    private NavMeshAgent agent; // NavMeshAgent �R���|�[�l���g
    public GameObject player;
    //���R�ǉ����܂����@�v���C���[���I�[�i�[�ɂ��Ă܂�
    private Player owner;

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
        Vector3 targetPosition = owner.transform.position - owner.transform.forward * 5f;

        agent.SetDestination(targetPosition);
    }

    public void Stun()
    {

    }

    //���R�������牺�ǉ����܂���
    public void SetOwner(Player newOwner)
    {
        owner = newOwner;
        Debug.Log($"�S���v���C���[{owner.playerID}�̂��̂ɂȂ����I");
        // ������AI��Ǐ]��Ȃǂ�؂�ւ��鏈���������I
    }





}

