using UnityEngine;
using UnityEngine.AI;

public class Necromancer : MonoBehaviour
{
    public NavMeshAgent agent;

    private Player playerScript;
    public GameObject player1Obj;
    public GameObject player2Obj;
    public bool player1 = false;
    public bool player2 = false;
    private GameObject respawn;

    public bool NecStun = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        respawn = GameObject.Find("Respawn");

        Destroy(this.gameObject, 30.0f);
    }

    // Update is called once per frame
    public void Update()
    {
        if (player1)
        {
            player1Obj = GameObject.Find("Player1 ");
            if (player1Obj != null)
            {
                agent.SetDestination(player1Obj.transform.position);
                Debug.Log("プレイヤー1追跡");
            }
            else
            {
                Debug.Log("プレイヤー1が見つかりません");
            }
        }
        else if (player2)
        {
            player2Obj = GameObject.Find("Player2 ");
            if (player2Obj != null)
            {
                agent.SetDestination(player2Obj.transform.position);
                Debug.Log("プレイヤー2追跡");
            }
            else
            {
                Debug.Log("プレイヤー2が見つかりません");
            }
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.isTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<CharacterController>().enabled = false;
            collision.gameObject.transform.position = respawn.transform.position;
            collision.gameObject.GetComponent<CharacterController>().enabled = true;

            playerScript = collision.gameObject.GetComponent<Player>();

            playerScript.keyCount = 0;
            playerScript.UpdateKeyUI();  

            Destroy(this.gameObject);
        }
    }
}
