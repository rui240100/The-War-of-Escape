using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TriggerCamera : MonoBehaviour
{
    public GameObject targetObject;
    private Player target;
    private Player player;
    private TriggerCamera targetCamera;
    public bool demonHave;

    private ProtectingDemon protectingDemon; 
    private DemonAI demon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        demonHave = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (demonHave)  //自分が守護鬼を持っていたら
        {
            if (other.CompareTag("Player"))     //相手がプレイヤーか確かめる
            {
                target = other.GetComponent<Player>();
                targetCamera = other.GetComponent<TriggerCamera>();

                if (!targetCamera.demonHave)    //相手が守護鬼を持っていなかったら
                {
                    StartCoroutine(StunCoroutine(target));
                }
                else
                {
                    Destroy(target.pd);
                }

                player = transform.parent.GetComponent<Player>();
                Destroy(player.pd);
            }
            else if (other.CompareTag("Demon"))
            {
                demon = other.GetComponent<DemonAI>();

                StartCoroutine(ProtectCoroutine(demon));

                player = transform.parent.GetComponent<Player>();
                Destroy(player.pd);
            }
        }
    }

    private IEnumerator StunCoroutine(Player target)
    {
        float playerSpeed = target.Speed;
        target.Speed = playerSpeed * 0.1f;

        yield return new WaitForSeconds(5.0f);

        target.Speed = playerSpeed;
    }

    private IEnumerator ProtectCoroutine(DemonAI demon)
    {
        float demonSpeed = demon.chaseSpeed;

        demon.chaseSpeed = 0.0f;
        demon.agent.speed = demon.chaseSpeed;

        Debug.Log("SlowDemon");

        yield return new WaitForSeconds(5.0f);

        demon.chaseSpeed = 6.0f;
        demon.agent.speed = demon.chaseSpeed;

        Debug.Log("鬼再開");

        demon.StopChase();
    }
}
