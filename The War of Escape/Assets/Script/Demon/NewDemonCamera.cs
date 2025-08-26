using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using System.Collections;
//using System.Diagnostics;

public class NewDemonCamera : MonoBehaviour
{
    private DemonAI demon;

    public GameObject player1Object;
    public GameObject player2Object;
    private Player playerScript;
    public bool player1;
    public bool player2;
    public bool player1Chase;
    public bool player2Chase;
    public float stopChaseTime; // プレイヤーがカメラの視界から外れたときに追跡を停止するまでの時間

    public GameObject eyeposition;

    Vector3 origin; //レイの発射位置

    private GameObject ChaseUI;
    private ChaseUI chaseUI;

    void Start()
    {
        demon = GetComponentInParent<DemonAI>();

        ChaseUI = GameObject.Find("ChaseUI");
        chaseUI = ChaseUI.GetComponent<ChaseUI>();
    }

    private void Update()
    {
        origin = eyeposition.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerScript = other.GetComponent<Player>();

            if (playerScript.playerID == 1)
            {
                player1 = true;
            }
            else
            {
                player2 = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerScript = other.GetComponent<Player>();

            if (playerScript.playerID == 1)
            {
                player1 = false;

                if (player1Chase)
                {
                    StartCoroutine(StopChaseTimeCoroutine1());
                }
            }
            else if (playerScript.playerID == 2)
            {
                player2 = false;

                if (player2Chase)
                {
                    StartCoroutine(StopChaseTimeCoroutine2());
                }
            }
        }

    }

    void OnTriggerStay(Collider other)
    {
        //追いかけてない
        if (!demon.isChasing)
        {
            if (other.CompareTag("Player"))
            {
                playerScript = other.GetComponent<Player>();

                if (playerScript.playerID == 1) 
                {
                    if ((CanSeePlayer(other.transform)))
                    {
                        Debug.Log("追いかけてないときにP1みつけた");
                        demon.StartChase(other.transform);
                        player1Chase = true;
                        chaseUI.player1 = true;
                    }
                }
                else
                {
                    if ((CanSeePlayer(other.transform)))
                    {
                        demon.StartChase(other.transform);
                        player2Chase = true;
                        chaseUI.player2 = true;
                    }
                }
            }
        }

        //追いかけてる
        else
        {
            if (player1 && player2 == true)
            {
                if ((CanSeePlayer(player1Object.transform)) && (CanSeePlayer(player2Object.transform)) == true)
                {
                    Vector3 player1Pos = player1Object.transform.position;
                    Vector3 player2Pos = player2Object.transform.position;

                    Vector3 dir1 = player1Pos - origin;
                    Vector3 dir2 = player2Pos - origin;

                    Ray ray1 = new Ray(origin, dir1.normalized);
                    Ray ray2 = new Ray(origin, dir2.normalized);

                    RaycastHit hit1;
                    RaycastHit hit2;

                    Physics.Raycast(ray1, out hit1);
                    Physics.Raycast(ray2, out hit2);

                    if (hit1.distance < hit2.distance)
                    {
                        Debug.Log("Player1が近い");
                        demon.StartChase(player1Object.transform);
                        player1Chase = true;
                        chaseUI.player1 = true;
                        player2Chase = false;
                        chaseUI.player2 = false;
                    }
                    else if (hit1.distance > hit2.distance)
                    {
                        Debug.Log("Player2が近い");
                        demon.StartChase(player2Object.transform);
                        player2Chase = true;
                        chaseUI.player2 = true;
                        player1Chase = false;
                        chaseUI.player1 = false;
                    }
                    else
                    {
                        Debug.Log("Player1とPlayer2は同じ距離");
                    }
                }

            }
        }

    }

    private bool CanSeePlayer(Transform target)
    {
        Vector3 targetPos = target.position + Vector3.up * 0.5f;
        Vector3 dir = targetPos - origin;

        Debug.DrawRay(origin, dir, Color.red, 0.1f);
        Ray ray = new Ray(origin, dir.normalized);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, dir.magnitude))
        {
            Debug.Log("Hit object: " + hit.collider.name);

            return hit.collider.CompareTag("Player");
        }
        return false;
    }

    private IEnumerator StopChaseTimeCoroutine1()
    {
        yield return new WaitForSeconds(5.0f);

        if (player1)
        {
            Debug.Log("AAAA");
            demon.StopChase();
            player1Chase = false;
            chaseUI.player1 = false;
        }
    }

    private IEnumerator StopChaseTimeCoroutine2()
    {
        yield return new WaitForSeconds(5.0f);

        if (player2)
        {
            Debug.Log("BBBB");
            demon.StopChase();
            player2Chase = false;
            chaseUI.player2 = false;
        }
    }
}
