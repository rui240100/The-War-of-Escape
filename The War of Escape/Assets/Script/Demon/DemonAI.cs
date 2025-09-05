using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UI;

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
    private float disableTime = 10.0f;
    public GameObject child;

    private bool isCollision = false;
    private float collisionTime = 0.0f;

    public ItemUse itemUseSc;
    private string message1 = "鍵持っている。";
    private string message2 = "鍵持っていない。";

    private bool collisionWaitTime = false;

    public AudioClip runSound;
    public AudioClip findSound;
    public AudioClip dieSound;
    public AudioSource audioSource;

    private bool isAnimating1 = false;
    private bool isAnimating2 = false;
    private bool keyCount1 = false;
    private bool keyCount2 = false;

    private bool runSoundPlaying = false;

    public Image targetImage1; // 透明度を変更する対象のImage
    public Image targetImage2;
    public float duration = 0.5f; // フェードにかける時間（秒
    public float waitTime = 4.0f;     // 表示しておく時間
    public float fadeDuration = 0.5f; // フェードアウトにかける時間（秒）


    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // エージェント取得
        GoToNextPatrolPoint(); // 最初のパトロールポイントへ移動

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
    }

    public void StopChase()
    {
        isChasing = false;
        player = null;
        agent.speed = patrolSpeed;
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
            }
            else
            {
                if (!isAnimating1)
                {
                    playerID = 2;
                    StartCoroutine(PlayUIAnimation(playerID));
                }
            }

            StartCoroutine(SlowDownPlayer(playerScript));

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
        if (playerID == 1)
        {
            isAnimating1 = true;

            if (keyCount1 == true)
            {
                Color color = targetImage1.color;
                float startAlpha = color.a;
                float time = 0f;

                while (time < duration)
                {
                    time += Time.deltaTime;
                    float t = Mathf.Clamp01(time / duration);
                    color.a = Mathf.Lerp(startAlpha, 1f, t); // α値を補間
                    targetImage1.color = color;
                    yield return null;
                }

                // 最終的に確実に Max にしておく
                color.a = 1f;
                targetImage1.color = color;

                yield return new WaitForSeconds(waitTime);

                // 3. フェードアウト (1 → 0)
                time = 0f;
                while (time < fadeDuration)
                {
                    time += Time.deltaTime;
                    float t = Mathf.Clamp01(time / fadeDuration);
                    color.a = Mathf.Lerp(1f, 0f, t);
                    targetImage1.color = color;
                    yield return null;
                }
                color.a = 0f;
                targetImage1.color = color;
            }

            else if (keyCount1 == false)
            {
                Color color = targetImage1.color;
                float startAlpha = color.a;
                float time = 0f;

                while (time < duration)
                {
                    time += Time.deltaTime;
                    float t = Mathf.Clamp01(time / duration);
                    color.a = Mathf.Lerp(startAlpha, 1f, t); // α値を補間
                    targetImage1.color = color;
                    yield return null;
                }

                // 最終的に確実に Max にしておく
                color.a = 1f;
                targetImage1.color = color;

                yield return new WaitForSeconds(waitTime);

                // 3. フェードアウト (1 → 0)
                time = 0f;
                while (time < fadeDuration)
                {
                    time += Time.deltaTime;
                    float t = Mathf.Clamp01(time / fadeDuration);
                    color.a = Mathf.Lerp(1f, 0f, t);
                    targetImage1.color = color;
                    yield return null;
                }
                color.a = 0f;
                targetImage1.color = color;
            }

            isAnimating1 = false;
        }
        else
        {
            isAnimating2 = true;

            if (keyCount2 == true)
            {
                Color color = targetImage2.color;
                float startAlpha = color.a;
                float time = 0f;

                while (time < duration)
                {
                    time += Time.deltaTime;
                    float t = Mathf.Clamp01(time / duration);
                    color.a = Mathf.Lerp(startAlpha, 1f, t); // α値を補間
                    targetImage2.color = color;
                    yield return null;
                }

                // 最終的に確実に Max にしておく
                color.a = 1f;
                targetImage2.color = color;

                yield return new WaitForSeconds(waitTime);

                // 3. フェードアウト (1 → 0)
                time = 0f;
                while (time < fadeDuration)
                {
                    time += Time.deltaTime;
                    float t = Mathf.Clamp01(time / fadeDuration);
                    color.a = Mathf.Lerp(1f, 0f, t);
                    targetImage2.color = color;
                    yield return null;
                }
                color.a = 0f;
                targetImage2.color = color;
            }
        
            else if (keyCount2 == false)
            {
                Color color = targetImage2.color;
                float startAlpha = color.a;
                float time = 0f;

                while (time < duration)
                {
                    time += Time.deltaTime;
                    float t = Mathf.Clamp01(time / duration);
                    color.a = Mathf.Lerp(startAlpha, 1f, t); // α値を補間
                    targetImage2.color = color;
                    yield return null;
                }

                // 最終的に確実に Max にしておく
                color.a = 1f;
                targetImage2.color = color;

                yield return new WaitForSeconds(waitTime);

                // 3. フェードアウト (1 → 0)
                time = 0f;
                while (time < fadeDuration)
                {
                    time += Time.deltaTime;
                    float t = Mathf.Clamp01(time / fadeDuration);
                    color.a = Mathf.Lerp(1f, 0f, t);
                    targetImage2.color = color;
                    yield return null;
                }
                color.a = 0f;
                targetImage2.color = color;
            }
    
            isAnimating2 = false;
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
        collisionWaitTime = false;
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