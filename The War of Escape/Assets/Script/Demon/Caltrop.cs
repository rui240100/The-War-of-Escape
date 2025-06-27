using System.Collections;
using UnityEngine;
using static Unity.Burst.Intrinsics.Arm;

public class Caltrap : Item
{
    private Player playerScript;
    private DemonAI demonAIScript;
    private Necromancer necromancerScript;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerScript = other.GetComponent<Player>();
            StartCoroutine(SlowDownPlayer(playerScript));
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Demon"))
        {
            demonAIScript = other.GetComponent<DemonAI>();
            StartCoroutine(SlowDownDemon(demonAIScript));
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Necromancer"))
        {
            necromancerScript = other.GetComponent<Necromancer>();
            StartCoroutine(SlowDownNecromancer(necromancerScript));
            Destroy(this.gameObject);
        }
    }

    private IEnumerator SlowDownPlayer(Player playerScript)
    {
        playerScript.Speed = 0.2f;
        yield return new WaitForSeconds(5.0f);
        playerScript.Speed = 5.0f;
    }

    private IEnumerator SlowDownDemon(DemonAI demonAIScript)
    {


        if (demonAIScript.isChasing)
        {
            float demonChaseSpeed = demonAIScript.chaseSpeed;
            demonAIScript.chaseSpeed = 0.2f;
            demonAIScript.agent.speed = demonAIScript.chaseSpeed;

            yield return new WaitForSeconds(5.0f);

            demonAIScript.chaseSpeed = 6.0f;
            demonAIScript.agent.speed = demonAIScript.chaseSpeed;
        }
        else if (!demonAIScript.isChasing)
        {
            float demonPatrolSpeed = demonAIScript.patrolSpeed;
            demonAIScript.patrolSpeed = 0.2f;
            demonAIScript.agent.speed = demonAIScript.patrolSpeed;

            yield return new WaitForSeconds(5.0f);

            demonAIScript.patrolSpeed = 4.0f;
            demonAIScript.agent.speed = demonAIScript.patrolSpeed;
        }
    }

    private IEnumerator SlowDownNecromancer(Necromancer necromancerScript)
    {
        float necromancerSpeed = necromancerScript.agent.speed;
        necromancerScript.agent.speed = 0.2f;

        yield return new WaitForSeconds(5.0f);

        necromancerScript.agent.speed = necromancerSpeed;
    }








}