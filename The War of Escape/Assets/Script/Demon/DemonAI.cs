using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class DemonAI : MonoBehaviour
{
    public NavMeshAgent agent; // NavMeshAgent コンポーネント
    public float patrolSpeed = 4.0f; // パトロール時の移動速度
    public float chaseSpeed = 6.0f;  // 追跡時の移動速度
    public Transform[] patrolPoints; // パトロールする地点の配列
    private int currentPointIndex = 0; // 現在向かっているパトロール地点のインデックス

    private Transform player; // 現在追跡しているプレイヤー
    private Player playerScript;
    public bool isChasing = false; // 追跡状態フラグ
    public Transform CurrentTarget => player; // 現在のターゲットを外部から参照できるプロパティ
    public Transform respawn;

    private NewDemonCamera newDemonCamera;

    public bool demonStun = false;

    private GameObject ChaseUI;
    private ChaseUI chaseUI;

    private MonoBehaviour targetScript;
    private float disableTime = 20.0f;
    public GameObject child;

    private bool isCollision = false;
    private float collisionTime = 0.0f;

    public ItemUse itemUseSc;
    private string message1 = "捕まっちゃった";
    private string message2 = "";

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // エージェント取得
        GoToNextPatrolPoint(); // 最初のパトロールポイントへ移動

        //child = Instantiate(childPrefab, transform);

        newDemonCamera = GetComponentInChildren<NewDemonCamera>();

        ChaseUI = GameObject.Find("ChaseUI");
        chaseUI = ChaseUI.GetComponent<ChaseUI>();
    }

    void Update()
    {
        if (isChasing)
        {
            //if (agent.hasPath)
            {
                Transform target = player.transform;
                agent.destination = target.position; // プレイヤーを追跡
                //Debug.Log("PlayerPosition" + target.position);

                if (!demonStun)
                {
                    agent.speed = chaseSpeed; // 追跡時のスピードに変更
                }
            }
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        //else if (!isChasing)
        {
            if (!demonStun)
            {
                agent.speed = patrolSpeed; // 初期状態ではパトロール速度
            }
            GoToNextPatrolPoint(); // 次のパトロール地点へ
        }

        if (isCollision)
        {
            collisionTime += Time.deltaTime; // 衝突中は時間を加算
            Debug.Log("Collision Time: " + collisionTime);
            if (collisionTime >= 4.0f)
            {
                gameObject.transform.position = respawn.position;
            }
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return; // パトロールポイントが設定されていない場合は何もしない

        agent.destination = patrolPoints[currentPointIndex].position; // 次の目的地を設定
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length; // 次のインデックスへ（ループ）
        //Debug.Log("Patroling");
    }

    public void StartChase(Transform target)
    {
        player = target;
        isChasing = true;
        //Debug.Log("ChaseStart");
    }

    public void StopChase()
    {
        isChasing = false;
        player = null;
        agent.speed = patrolSpeed;
        Debug.Log("ChaseStop");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            child.SetActive(false);
            //newDemonCamera.enabled = false;
            //Debug.Log("Collided Object Position: " + collision.transform.position);
            //collision.gameObject.GetComponent<CharacterController>().enabled = false;
            //collision.gameObject.transform.position = respawn.position;
            //collision.gameObject.GetComponent<CharacterController>().enabled = true;
            //Debug.Log("Collided Object Position: " + collision.transform.position);
            Debug.Log("Collided Object Name: " + collision.gameObject.name);
            playerScript = collision.gameObject.GetComponent<Player>();

            playerScript.keyCount -= 1;
            playerScript.UpdateKeyUI();

            if (playerScript.playerID == 1)
            {
                itemUseSc.ShowMessage(message1, message2);
            }
            else
            {
                itemUseSc.ShowMessage(message2, message1);
            }

            StartCoroutine(SlowDownPlayer(playerScript));



            Debug.Log("プレイヤー" + playerScript.playerID + "と接触");



            //if (playerScript.playerID == 1)
            //{
            //    newDemonCamera.player1Chase = false;
            //    chaseUI.player1 = false;
            //}
            //else if (playerScript.playerID == 2)
            //{
            //    newDemonCamera.player2Chase = false;
            //    chaseUI.player2 = false;
            //}


            chaseUI.player1 = false;
            chaseUI.player2 = false;
            StartCoroutine(DisableForSeconds());
            isChasing = false;
            player = null;
            StopChase();
            Debug.Log("終了");
        }
    }

    private IEnumerator DisableForSeconds()
    {
        yield return new WaitForSeconds(10.0f);
        child.SetActive(true);
        newDemonCamera.player1 = false;
        newDemonCamera.player2 = false;
        newDemonCamera.player1Chase = false;
        newDemonCamera.player2Chase = false;
        //Instantiate(child, transform);
        //MonoBehaviour script =this.GetComponentInChildren<NewDemonCamera>();

        //if (script != null)
        //{
        //    //script.enabled = false;       // 無効化
        //    Debug.Log("DemonCameraFalse");
        //    Debug.Log("今から5秒止めます");
        //    yield return new WaitForSeconds(disableTime);
        //    Debug.Log("5秒後");
        //    script.enabled = true;        // 再び有効化
        //}
    }

    private IEnumerator SlowDownPlayer(Player playerScript)
    {
        playerScript.Speed = 0.2f;
        yield return new WaitForSeconds(5.0f);
        playerScript.Speed = 5.0f;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isCollision = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isCollision = false;
            collisionTime = 0.0f;
        }
    }
}