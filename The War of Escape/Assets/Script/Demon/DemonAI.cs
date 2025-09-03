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

    private bool collisionWaitTime = false;

    public AudioClip runSound;
    public AudioClip findSound;
    public AudioClip dieSound;
    public AudioSource audioSource;

    public RectTransform uiImage1;  // 対象のUI (Image)
    public RectTransform uiImage12;
    public RectTransform uiImage2;
    public RectTransform uiImage22;
    private Vector2 startPosRight = new Vector2(1000, 0);  // 右外からの開始位置
    private Vector2 centerPos = new Vector2(0, 0);         // 中央位置
    private Vector2 endPosLeft = new Vector2(-1000, 0);    // 左外へ消える位置
    private float slideTime = 0.5f;   // スライドにかかる時間
    private float stayTime = 2.0f;      // 中央で停止する時間

    private bool isAnimating1 = false;
    private bool isAnimating2 = false;
    private bool keyCount1 = false;
    private bool keyCount2 = false;

    private bool runSoundPlaying = false;

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
                    //if (!runSoundPlaying)
                    //{
                    //    runSoundPlaying = true;
                    //    audioSource.PlayOneShot(runSound);
                    //    runSoundPlaying = false;
                    //}


                }
            }
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        //else if (!isChasing)
        {
            if (!demonStun)
            {
                agent.speed = patrolSpeed; // 初期状態ではパトロール速度
                //if (!walkSoundPlaying)
                //{
                //    walkSoundPlaying = true;
                //    audioSource.PlayOneShot(walkSound);
                //    walkSoundPlaying = false;
                //}
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
        audioSource.PlayOneShot(findSound);
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
        if (collision.gameObject.CompareTag("Player") && collisionWaitTime == false)
        {
            //audioSource.PlayOneShot(dieSound);
            collisionWaitTime = true;
            child.SetActive(false);
            //newDemonCamera.enabled = false;
            //Debug.Log("Collided Object Position: " + collision.transform.position);
            //collision.gameObject.GetComponent<CharacterController>().enabled = false;
            //collision.gameObject.transform.position = respawn.position;
            //collision.gameObject.GetComponent<CharacterController>().enabled = true;
            //Debug.Log("Collided Object Position: " + collision.transform.position);
            Debug.Log("Collided Object Name: " + collision.gameObject.name);
            playerScript = collision.gameObject.GetComponent<Player>();

            if (playerScript.keyCount >= 1)
            {
                if (playerScript.playerID == 1)
                    keyCount1 = true;

                if (playerScript.playerID == 2)
                    keyCount2 = true;

                playerScript.keyCount -= 1;
            }
            else if(playerScript.keyCount == 0)
            {
                if (playerScript.playerID == 1)
                    keyCount1 = false;

                if (playerScript.playerID == 2)
                    keyCount2 = false;
            }
                playerScript.UpdateKeyUI();

            int playerID;

            if (playerScript.playerID == 1)
            {
                if (!isAnimating1)
                {
                    playerID = 1;
                    StartCoroutine(PlayUIAnimation(playerID));
                }
                //itemUseSc.ShowMessage(message1, message2);
            }
            else
            {
                if (!isAnimating1)
                {
                    playerID = 2;
                    StartCoroutine(PlayUIAnimation(playerID));
                }
                //itemUseSc.ShowMessage(message2, message1);
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

    IEnumerator PlayUIAnimation(int playerID)
    {
        if(playerID == 1)
        {
            isAnimating1 = true;

            if(keyCount1 == true)
            {
                // 右 → 中央へ移動
                yield return StartCoroutine(Slide(uiImage1, startPosRight, centerPos, slideTime));

                // 中央で待機
                yield return new WaitForSeconds(stayTime);

                // 中央 → 左へ移動
                yield return StartCoroutine(Slide(uiImage1, centerPos, endPosLeft, slideTime));

                // 終了したら初期位置に戻す
                uiImage1.anchoredPosition = startPosRight;
            }
            else if(keyCount1 == false)
            {
                // 右 → 中央へ移動
                yield return StartCoroutine(Slide(uiImage12, startPosRight, centerPos, slideTime));

                // 中央で待機
                yield return new WaitForSeconds(stayTime);

                // 中央 → 左へ移動
                yield return StartCoroutine(Slide(uiImage12, centerPos, endPosLeft, slideTime));

                // 終了したら初期位置に戻す
                uiImage12.anchoredPosition = startPosRight;
            }

                isAnimating1 = false;
        }
        else
        {
            isAnimating2 = true;

            if(keyCount2 == true)
            {
                // 右 → 中央へ移動
                yield return StartCoroutine(Slide(uiImage2, startPosRight, centerPos, slideTime));

                // 中央で待機
                yield return new WaitForSeconds(stayTime);

                // 中央 → 左へ移動
                yield return StartCoroutine(Slide(uiImage2, centerPos, endPosLeft, slideTime));

                // 終了したら初期位置に戻す
                uiImage1.anchoredPosition = startPosRight;
            }
            else if (keyCount2 == false)
            {
                // 右 → 中央へ移動
                yield return StartCoroutine(Slide(uiImage22, startPosRight, centerPos, slideTime));

                // 中央で待機
                yield return new WaitForSeconds(stayTime);

                // 中央 → 左へ移動
                yield return StartCoroutine(Slide(uiImage22, centerPos, endPosLeft, slideTime));

                // 終了したら初期位置に戻す
                uiImage22.anchoredPosition = startPosRight;
            }

                isAnimating2 = false;
        }
    }

    // 補間で移動
    IEnumerator Slide(RectTransform target, Vector2 from, Vector2 to, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            target.anchoredPosition = Vector2.Lerp(from, to, t);
            yield return null;
        }
        target.anchoredPosition = to;
    }

    private IEnumerator DisableForSeconds()
    {
        yield return new WaitForSeconds(10.0f);
        child.SetActive(true);
        newDemonCamera.player1 = false;
        newDemonCamera.player2 = false;
        newDemonCamera.player1Chase = false;
        newDemonCamera.player2Chase = false;
        collisionWaitTime = false;
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