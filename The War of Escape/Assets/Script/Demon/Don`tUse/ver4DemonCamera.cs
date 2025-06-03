using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ver4DemonCamera : MonoBehaviour
{
    private ver2DemonAI demon;

    [SerializeField] private float searchRadius = 15f;  // åüímÇ∑ÇÈç≈ëÂãóó£

    void Start()
    {
        demon = GetComponentInParent<ver2DemonAI>();
        StartCoroutine(CheckClosestPlayerRoutine());
    }

    private IEnumerator CheckClosestPlayerRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);

        while (true)
        {
            yield return wait;

            Transform closestPlayer = null;
            float closestDistance = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player1") || collider.CompareTag("Player2"))
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPlayer = collider.transform;
                    }
                }
            }

            if (closestPlayer != null)
            {
                if (demon.CurrentTarget != closestPlayer)
                {
                    demon.StartChase(closestPlayer);
                }
            }
            else
            {
                if (demon.CurrentTarget != null)
                {
                    demon.StopChase();
                }
            }
        }
    }
}
