using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
//using System.Diagnostics;

public class NewDemonCamera : MonoBehaviour
{
    private DemonAI demon;

    public GameObject player1Object;
    public GameObject player2Object;
    private Player playerScript;
    private bool player1;
    private bool player2;
    private bool player1Chase;
    private bool player2Chase;

    public GameObject eyeposition;

    public float radius = 5f;
    private Transform playerTransform;

    Vector3 origin; //ƒŒƒC‚Ì”­ŽËˆÊ’u

    void Start()
    {
        demon = GetComponentInParent<DemonAI>();
    }

    private void Update()
    {
        origin = eyeposition.transform.position;

        //float distance = Vector3.Distance(transform.position, playerTransform.position);

        //if (distance <= radius)
        //{

        //}
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
                    demon.StopChase();
                }
            }
            else if (playerScript.playerID == 2)
            {
                player2 = false;

                if (player2Chase)
                {
                    demon.StopChase();
                }
            }
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (!demon.isChasing)
        {
            if (other.CompareTag("Player"))
            {
                playerScript = other.GetComponent<Player>();

                if (playerScript.playerID == 1) 
                {
                    CanSeePlayer(other.transform);
                    demon.StartChase(other.transform);
                    player1Chase = true;
                }
                else
                {
                    CanSeePlayer(other.transform);
                    demon.StartChase(other.transform);
                    player2Chase = true;
                }
            }
        }
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
                        Debug.Log("Player1‚ª‹ß‚¢");
                        demon.StartChase(player1Object.transform);
                        player1Chase = true;
                        player2Chase = false;
                    }
                    else if (hit1.distance > hit2.distance)
                    {
                        Debug.Log("Player2‚ª‹ß‚¢");
                        demon.StartChase(player2Object.transform);
                        player2Chase = true;
                        player1Chase = false;
                    }
                    else
                    {
                        Debug.Log("Player1‚ÆPlayer2‚Í“¯‚¶‹——£");
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
}
