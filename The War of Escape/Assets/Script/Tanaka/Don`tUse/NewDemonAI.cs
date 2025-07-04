using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class NewDemonAI : MonoBehaviour
{
    [Header("Players")]
    public Transform player1;
    public Transform player2;

    [Header("Patrol Settings")]
    public List<Transform> patrolPoints;
    private int currentPatrolIndex = 0;

    [Header("View Settings")]
    public float viewDistance = 15f;
    public float viewAngle = 180f;

    [Header("Speed Settings")]
    public float patrolSpeed = 5f;
    public float chaseSpeed = 10f;

    NavMeshAgent agent;
    Transform targetPlayer = null;

    bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        GoToNextPatrolPoint();
        Debug.Log("Demon: パトロール開始");
    }

    void Update()
    {
        Transform seenPlayer = CheckForPlayers();

        if (seenPlayer != null)
        {
            if (targetPlayer != seenPlayer)
            {
                Debug.Log($"Demon: プレイヤー[{seenPlayer.name}]を新たに視認、追跡開始");
            }

            targetPlayer = seenPlayer;
            agent.speed = chaseSpeed;
            agent.SetDestination(targetPlayer.position);
            isChasing = true;
        }
        else if (targetPlayer != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetPlayer.position);
            if (distanceToTarget > viewDistance || !IsInView(targetPlayer))
            {
                Debug.Log($"Demon: プレイヤー[{targetPlayer.name}]を見失った。パトロールに戻る");
                targetPlayer = null;
                agent.speed = patrolSpeed;
                isChasing = false;
                GoToNextPatrolPoint();
            }
            else
            {
                agent.SetDestination(targetPlayer.position);
            }
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GoToNextPatrolPoint();
            }
        }

        CheckCollisionWithPlayers();
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Count == 0) return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        Debug.Log($"Demon: パトロールポイント[{patrolPoints[currentPatrolIndex].name}]に移動中");

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
    }

    Transform CheckForPlayers()
    {
        List<Transform> players = new List<Transform> { player1, player2 };
        Transform closestPlayer = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform player in players)
        {
            if (player == null) continue;

            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= viewDistance && IsInView(player))
            {
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPlayer = player;
                }
            }
        }

        return closestPlayer;
    }

    bool IsInView(Transform target)
    {
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        float angleToTarget = Vector3.Angle(transform.forward, dirToTarget);

        if (angleToTarget <= viewAngle / 2f)
        {
            Ray ray = new Ray(transform.position + Vector3.up * 1.5f, dirToTarget);
            Debug.DrawRay(ray.origin, ray.direction * viewDistance, Color.red); // 👈 可視化追加

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, viewDistance))
            {
                if (hit.transform == target)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void CheckCollisionWithPlayers()
    {
        if (player1 && Vector3.Distance(transform.position, player1.position) < 1.5f)
        {
            Debug.Log("Demon: Player1 に接触。Destroy 実行");
            Destroy(player1.gameObject);
        }

        if (player2 && Vector3.Distance(transform.position, player2.position) < 1.5f)
        {
            Debug.Log("Demon: Player2 に接触。Destroy 実行");
            Destroy(player2.gameObject);
        }
    }

    // 👇 Sceneビューでの視界可視化
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Vector3 forward = transform.forward;

        Quaternion leftRayRotation = Quaternion.AngleAxis(-viewAngle / 2, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(viewAngle / 2, Vector3.up);

        Vector3 leftRayDirection = leftRayRotation * forward;
        Vector3 rightRayDirection = rightRayRotation * forward;

        Gizmos.DrawRay(transform.position + Vector3.up * 1.5f, leftRayDirection * viewDistance);
        Gizmos.DrawRay(transform.position + Vector3.up * 1.5f, rightRayDirection * viewDistance);

        Gizmos.DrawWireSphere(transform.position + Vector3.up * 1.5f, viewDistance);
    }
}
