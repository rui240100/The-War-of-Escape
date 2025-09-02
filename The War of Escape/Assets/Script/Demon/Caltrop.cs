using System.Collections;
using UnityEngine;
using static Unity.Burst.Intrinsics.Arm;

public class Caltrap : MonoBehaviour
{
    private Player playerScript;
    private DemonAI demonAIScript;
    private Necromancer necromancerScript;

    private int count = 0;

    public AudioSource caltropAudioSource;

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
        count++;

        if (count <= 1)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            caltropAudioSource.Play();
            playerScript = other.GetComponent<Player>();
            StartCoroutine(SlowDownPlayer(playerScript));
            this.transform.position = new Vector3(0, -100, 0);
            Destroy(this.gameObject,5.1f);
        }
        else if (other.CompareTag("Demon"))
        {
            demonAIScript = other.GetComponent<DemonAI>();
            StartCoroutine(SlowDownDemon(demonAIScript));
            this.transform.position = new Vector3(0, -100, 0);
            Destroy(this.gameObject, 5.1f);
        }
        else if (other.CompareTag("Necromancer"))
        {
            necromancerScript = other.GetComponent<Necromancer>();
            StartCoroutine(SlowDownNecromancer(necromancerScript));
            this.transform.position = new Vector3(0, -100, 0);
            Destroy(this.gameObject, 5.1f);
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
            demonAIScript.demonStun = true;

            yield return new WaitForSeconds(5.0f);

            demonAIScript.chaseSpeed = 6.0f;
            demonAIScript.agent.speed = demonAIScript.chaseSpeed;
            demonAIScript.demonStun = false;
        }
        else if (!demonAIScript.isChasing)
        {
            float demonPatrolSpeed = demonAIScript.patrolSpeed;
            demonAIScript.patrolSpeed = 0.2f;
            demonAIScript.agent.speed = demonAIScript.patrolSpeed;
            demonAIScript.demonStun = true;

            yield return new WaitForSeconds(5.0f);

            demonAIScript.patrolSpeed = 4.0f;
            demonAIScript.agent.speed = demonAIScript.patrolSpeed;
            demonAIScript.demonStun = false;
        }
    }

    private IEnumerator SlowDownNecromancer(Necromancer necromancerScript)
    {
        necromancerScript.agent.speed = 0.2f;

        yield return new WaitForSeconds(5.0f);

        necromancerScript.agent.speed = 6.0f;
    }








}