using UnityEngine;
using UnityEngine.AI;

public class ver2DemonAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPointIndex = 0;

    private NavMeshAgent agent;
    private Transform player;
    private bool isChasing = false;

    public float patrolSpeed = 2.0f;
    public float chaseSpeed = 4.0f;

    public Transform CurrentTarget => player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (isChasing && player != null)
        {
            Debug.Log("chase");

            float distanceToPlayer = Vector3.Distance(agent.transform.position, player.position);
            //if (distanceToPlayer > 2f)  // �v���C���[��2m�ȏ㓮�����ꍇ�ɂ̂ݍĐݒ�
            //{
            //    agent.SetDestination(player.position);
            //}

            // �X���[�Y�ȉ�]
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);  // ��]���x�𒲐�
            agent.SetDestination(player.position);
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPointIndex].position;
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
    }

    public void StartChase(Transform target)
    {
        Debug.Log("start");

        if (player == target && isChasing) return;

        player = target;
        isChasing = true;
        agent.speed = chaseSpeed;
    }

    public void StopChase()
    {
        Debug.Log("stop");

        if (!isChasing) return;

        isChasing = false;
        player = null;
        agent.speed = patrolSpeed;
        GoToNextPatrolPoint(); // �p�g���[���ĊJ��ǉ�
    }
}
