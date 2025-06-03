using UnityEngine;
using UnityEngine.AI;

public class ProtectingDemon : MonoBehaviour
{
    private NavMeshAgent agent; // NavMeshAgent コンポーネント
    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // エージェント取得
    }

    // Update is called once per frame
    void Update()
    {
        Launch();
    }

    public void Launch() 
    {
        Vector3 targetPosition = player.transform.position - player.transform.forward * 5f;

        agent.SetDestination(targetPosition);
    }

    public void Stun()
    {

    }
}

