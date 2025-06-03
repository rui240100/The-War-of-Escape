using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
//using System.Diagnostics;

public class DemonCamera : MonoBehaviour
{
    private DemonAI demon;
    private List<Transform> playersInRange = new List<Transform>();
    public GameObject eyeposition;
    private float nextActionTime = 0f;
    public float interval = 1f; // 1秒ごと

    Vector3 origin; //レイの発射位置

    void Start()
    {
        demon = GetComponentInParent<DemonAI>();
    }

    private void Update()
    {
        origin = eyeposition.transform.position;
        //Debug.Log("EyePosition " + eyeposition.transform.position);

        //if (demon.CurrentTarget != null && !CanSeePlayer(demon.CurrentTarget))
        //{
        //    demon.StopChase(); // 見えなくなったら停止
        //}
    }

    void OnTriggerStay(Collider other)
    {
        if (demon.isChasing)
            return;


        //if (Time.time >= nextActionTime)
        //{
        if (other.CompareTag("Player"))
        {
            //if (!playersInRange.Contains(other.transform))
            //{
            //    playersInRange.Add(other.transform);
            //}
            Debug.Log("視界内" + other.name);
            if (CanSeePlayer(other.transform))
            {
                //Debug.Log("CanSeePlayer returned TRUE — Starting chase.");

                demon.StartChase(other.transform);
            }
            else
            {
                Debug.Log("CanSeePlayer returned FALSE — Not chasing.");
            }
        }

        nextActionTime = Time.time + interval;

        //}
    }

    void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Player"))
        //{
        //    playersInRange.Remove(other.transform);

        //    if (demon.CurrentTarget == other.transform)
        //    {
        //        demon.StopChase();
        //    }
        //}
    }

    private bool CanSeePlayer(Transform target)
    {

        //Debug.Log("Ray Origin_A: " + origin); //座標を出力


        Vector3 targetPos = target.position + Vector3.up * 0.5f;
        Vector3 dir = targetPos - origin;

        //Debug.Log("Ray Origin_B: " + origin); //座標を出力

        Debug.DrawRay(origin, dir, Color.red, 0.1f);
        Ray ray = new Ray(origin, dir.normalized);

        //Debug.Log("Ray Origin_C: " + origin); //座標を出力


        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, dir.magnitude))
        {
            Debug.Log("Hit object: " + hit.collider.name);

            return hit.collider.CompareTag("Player");
        }

        //Debug.Log("Ray Origin_D: " + origin); //座標を出力

        return false;
    }
}
