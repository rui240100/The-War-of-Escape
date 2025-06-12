using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
//using System.Diagnostics;

public class NewDemonCamera : MonoBehaviour
{
    private DemonAI demon;

    public GameObject player;
    public GameObject eyeposition;

    public float radius = 5f;
    public Transform playerTransform;

    Vector3 origin; //ƒŒƒC‚Ì”­ŽËˆÊ’u

    void Start()
    {
        demon = GetComponentInParent<DemonAI>();
    }

    private void Update()
    {
        origin = eyeposition.transform.position;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= radius)
        {

        }
    }

    void OnTriggerStay(Collider other)
    {
        if (demon.isChasing)
            return;

        if (other.gameObject == player)
        {
            Debug.Log("Ž‹ŠE“à" + other.name);
            if (CanSeePlayer(other.transform))
            {
                demon.StartChase(other.transform);
            }
            else
            {
                Debug.Log("CanSeePlayer returned FALSE ? Not chasing.");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //if (demon.CurrentTarget == other.transform)
        //{
        //    demon.StopChase();
        //}
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
