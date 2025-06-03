using UnityEngine;
using UnityEngine.AI;

public class DemonAI : MonoBehaviour
{
    public Transform[] patrolPoints; // �p�g���[������n�_�̔z��
    private int currentPointIndex = 0; // ���݌������Ă���p�g���[���n�_�̃C���f�b�N�X

    private NavMeshAgent agent; // NavMeshAgent �R���|�[�l���g
    private Transform player; // ���ݒǐՂ��Ă���v���C���[
    public bool isChasing = false; // �ǐՏ�ԃt���O
    private bool callStart = true;

    public float patrolSpeed = 2.0f; // �p�g���[�����̈ړ����x
    public float chaseSpeed = 4.0f;  // �ǐՎ��̈ړ����x

    public Transform CurrentTarget => player; // ���݂̃^�[�Q�b�g���O������Q�Ƃł���v���p�e�B

    private Player playerScript;
    public GameObject keyPrefab;
    public Vector3[] keySpawnPositions;
    public float checkRadius = 0.1f;

    public Vector3 respawn;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // �G�[�W�F���g�擾
        agent.speed = patrolSpeed; // ������Ԃł̓p�g���[�����x
        GoToNextPatrolPoint(); // �ŏ��̃p�g���[���|�C���g�ֈړ�
    }

    void Update()
    {
        if (isChasing && player != null)
        {
            Debug.Log("�ǂ�������" + player.position);
            Debug.Log("Chase Object Name " + player);

            Debug.Log("agent.hasPath" + agent.hasPath);
            Debug.Log("agent.isStopped" + agent.isStopped);

            if (agent.hasPath)
            {
                agent.SetDestination(player.position); // �v���C���[��ǐ�
            }
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Debug.Log("�T����");
            GoToNextPatrolPoint(); // ���̃p�g���[���n�_��
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return; // �p�g���[���|�C���g���ݒ肳��Ă��Ȃ��ꍇ�͉������Ȃ�

        agent.destination = patrolPoints[currentPointIndex].position; // ���̖ړI�n��ݒ�
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length; // ���̃C���f�b�N�X�ցi���[�v�j
    }

    public void StartChase(Transform target)
    {
        if (callStart)
        {
            Debug.Log("Start1");

            player = target;
            isChasing = true;
            agent.speed = chaseSpeed; // �ǐՎ��̃X�s�[�h�ɕύX

            Debug.Log("Start2");

            callStart = false;
        }
    }

    public void StopChase()
    {
        Debug.Log("Stop");

        isChasing = false;
        player = null;
        agent.speed = patrolSpeed; // �p�g���[�����x�ɖ߂�

        callStart = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided Object Position: " + collision.transform.position);
            collision.gameObject.GetComponent<CharacterController>().enabled = false;
            collision.gameObject.transform.position = respawn;
            collision.gameObject.GetComponent<CharacterController>().enabled = true;
            Debug.Log("Collided Object Position: " + collision.transform.position);
            Debug.Log("Collided Object Name: " + collision.gameObject.name);
            playerScript = collision.gameObject.GetComponent<Player>();

            int keyCountToReturn = playerScript.keyCount;
            Debug.Log("�v���C���[�������Ă������̐�: " + keyCountToReturn);

            playerScript.keyCount = 0;

            int placedCount = 0;

            foreach (Vector3 pos in keySpawnPositions)
            {
                if (placedCount >= keyCountToReturn) break;

                // �w��ʒu�̎��͂ɂ���I�u�W�F�N�g�𒲂ׂ�
                Collider[] hits = Physics.OverlapSphere(pos, checkRadius);
                bool keyExists = false;

                foreach (Collider col in hits)
                {
                    if (col.CompareTag("Key"))
                    {
                        keyExists = true;
                        break;
                    }
                }

                // �����Ȃ���ΐ���
                if (!keyExists)
                {
                    Instantiate(keyPrefab, pos, Quaternion.identity);
                    placedCount++;
                    Debug.Log("����z�u���܂��� @ " + pos);
                }
            }

            if (placedCount < keyCountToReturn)
            {
                Debug.LogWarning("����߂��ꏊ������܂���ł����I");
            }

            StopChase();
        }
    }
}
