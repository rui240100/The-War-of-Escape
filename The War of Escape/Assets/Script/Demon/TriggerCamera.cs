using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TriggerCamera : MonoBehaviour
{
    public GameObject targetObject;
    private Player target;
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
        if (demonHave)  //©•ª‚ªçŒì‹S‚ğ‚Á‚Ä‚¢‚½‚ç
        {
            if (other.CompareTag("Player"))     //‘Šè‚ªƒvƒŒƒCƒ„[‚©Šm‚©‚ß‚é
            {
                target = other.GetComponent<Player>();
                targetCamera = other.GetComponent<TriggerCamera>();

                if (!targetCamera.demonHave)    //‘Šè‚ªçŒì‹S‚ğ‚Á‚Ä‚¢‚È‚©‚Á‚½‚ç
                {
                    StartCoroutine(StunCoroutine(target));
                }
                else
                {
                    //‘Šè‚ÌçŒì‹SDestroy
                }

                //©•ª‚ÌçŒì‹SDestroy
            }
            else if (other.CompareTag("Demon"))
            {
                demon = other.GetComponent<DemonAI>();

                StartCoroutine(ProtectCoroutine(demon));

                //©•ª‚ÌçŒì‹SDestroy
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

        Debug.Log("‹SÄŠJ");

        demon.StopChase();
    }
}
