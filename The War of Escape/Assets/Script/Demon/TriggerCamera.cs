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

    private Necromancer necromancer;

    public float stun = 0.0f;

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
            if (other.gameObject == targetObject)   //相手がプレイヤーか確かめる
            {
                target = other.GetComponent<Player>();
                targetCamera = other.GetComponentInChildren<TriggerCamera>();

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
                Debug.Log("鬼確認");
                demon = other.GetComponent<DemonAI>();

                StartCoroutine(ProtectCoroutine(demon));

                player = transform.parent.GetComponent<Player>();
                Destroy(player.pd);
            }
            else if(other.CompareTag("Necromancer"))
            {
                necromancer = other.GetComponent<Necromancer>();

                StartCoroutine(ProtectNecCoroutine(necromancer));

                player = transform.parent.GetComponent<Player>();
                Destroy(player.pd);
                
            }
        }
    }

    private IEnumerator StunCoroutine(Player target)
    {
        if(target != this.transform.parent.GetComponent<Player>())
        {
            float playerSpeed = target.Speed;
            target.Speed = stun;

            yield return new WaitForSeconds(5.0f);

            target.Speed = playerSpeed;
        }
        else
        {
            Debug.Log("自分のGetComponentしてる");
        }
    }

    private IEnumerator ProtectCoroutine(DemonAI demon)
    {
        float demonSpeed = demon.chaseSpeed;

        demon.demonStun = true;
        demon.chaseSpeed = 0.0f;
        demon.agent.speed = demon.chaseSpeed;

        Debug.Log("SlowDemon");

        yield return new WaitForSeconds(5.0f);

        demon.chaseSpeed = 6.0f;
        demon.agent.speed = demon.chaseSpeed;
        demon.demonStun = false;

        Debug.Log("鬼再開");

        demon.StopChase();
    }

    private IEnumerator ProtectNecCoroutine(Necromancer necromancer)
    {
        necromancer.agent.speed = 0.0f;
        necromancer.NecStun = true;

        Debug.Log("SlowNec");

        yield return new WaitForSeconds(5.0f);

        necromancer.agent.speed = 6.0f;
        necromancer.NecStun = false;

        Debug.Log("Nec再開");

    }
}
