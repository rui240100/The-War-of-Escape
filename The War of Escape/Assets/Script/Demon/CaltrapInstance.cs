using System.Collections;
using UnityEngine;

public class CaltrapInstance : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                StartCoroutine(SlowDownPlayer(player));
            }
        }
        else if (other.CompareTag("Demon"))
        {
            DemonAI demon = other.GetComponent<DemonAI>();
            if (demon != null)
            {
                StartCoroutine(SlowDownDemon(demon));
            }
        }
        else if (other.CompareTag("Necromancer"))
        {
            Necromancer necro = other.GetComponent<Necromancer>();
            if (necro != null)
            {
                StartCoroutine(SlowDownNecromancer(necro));
            }
        }

        // Ç«ÇÍÇ…ìñÇΩÇ¡ÇƒÇ‡çÌèú
        Destroy(gameObject);
    }

    private IEnumerator SlowDownPlayer(Player player)
    {
        float originalSpeed = player.Speed;
        player.Speed = 0.2f;
        yield return new WaitForSeconds(5f);
        player.Speed = originalSpeed;
    }

    private IEnumerator SlowDownDemon(DemonAI demon)
    {
        float originalSpeed = demon.isChasing ? demon.chaseSpeed : demon.patrolSpeed;

        if (demon.isChasing)
            demon.chaseSpeed = 0.2f;
        else
            demon.patrolSpeed = 0.2f;

        demon.agent.speed = 0.2f;
        yield return new WaitForSeconds(5f);

        if (demon.isChasing)
            demon.chaseSpeed = originalSpeed;
        else
            demon.patrolSpeed = originalSpeed;

        demon.agent.speed = originalSpeed;
    }

    private IEnumerator SlowDownNecromancer(Necromancer necro)
    {
        float originalSpeed = necro.agent.speed;
        necro.agent.speed = 0.2f;
        yield return new WaitForSeconds(5f);
        necro.agent.speed = originalSpeed;
    }
}
