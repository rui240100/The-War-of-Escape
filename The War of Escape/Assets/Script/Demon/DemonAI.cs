using UnityEngine;
using UnityEngine.AI;

public class DemonAI : MonoBehaviour
{
    public Transform[] patrolPoints; // パトロールする地点の配列
    private int currentPointIndex = 0; // 現在向かっているパトロール地点のインデックス

    private NavMeshAgent agent; // NavMeshAgent コンポーネント
    private Transform player; // 現在追跡しているプレイヤー
    public bool isChasing = false; // 追跡状態フラグ
    private bool callStart = true;

    public float patrolSpeed = 2.0f; // パトロール時の移動速度
    public float chaseSpeed = 4.0f;  // 追跡時の移動速度

    public Transform CurrentTarget => player; // 現在のターゲットを外部から参照できるプロパティ

    private Player playerScript;
    public GameObject keyPrefab;
    public Vector3[] keySpawnPositions;
    public float checkRadius = 0.1f;

    public Vector3 respawn;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // エージェント取得
        agent.speed = patrolSpeed; // 初期状態ではパトロール速度
        GoToNextPatrolPoint(); // 最初のパトロールポイントへ移動
    }

    void Update()
    {
        if (isChasing && player != null)
        {
            Debug.Log("追いかけ中" + player.position);
            Debug.Log("Chase Object Name " + player);

            Debug.Log("agent.hasPath" + agent.hasPath);
            Debug.Log("agent.isStopped" + agent.isStopped);

            if (agent.hasPath)
            {
                agent.SetDestination(player.position); // プレイヤーを追跡
            }
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Debug.Log("探索中");
            GoToNextPatrolPoint(); // 次のパトロール地点へ
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return; // パトロールポイントが設定されていない場合は何もしない

        agent.destination = patrolPoints[currentPointIndex].position; // 次の目的地を設定
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length; // 次のインデックスへ（ループ）
    }

    public void StartChase(Transform target)
    {
        if (callStart)
        {
            Debug.Log("Start1");

            player = target;
            isChasing = true;
            agent.speed = chaseSpeed; // 追跡時のスピードに変更

            Debug.Log("Start2");

            callStart = false;
        }
    }

    public void StopChase()
    {
        Debug.Log("Stop");

        isChasing = false;
        player = null;
        agent.speed = patrolSpeed; // パトロール速度に戻す

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
            Debug.Log("プレイヤーが持っていた鍵の数: " + keyCountToReturn);

            playerScript.keyCount = 0;

            int placedCount = 0;

            foreach (Vector3 pos in keySpawnPositions)
            {
                if (placedCount >= keyCountToReturn) break;

                // 指定位置の周囲にあるオブジェクトを調べる
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

                // 鍵がなければ生成
                if (!keyExists)
                {
                    Instantiate(keyPrefab, pos, Quaternion.identity);
                    placedCount++;
                    Debug.Log("鍵を配置しました @ " + pos);
                }
            }

            if (placedCount < keyCountToReturn)
            {
                Debug.LogWarning("鍵を戻す場所が足りませんでした！");
            }

            StopChase();
        }
    }
}
