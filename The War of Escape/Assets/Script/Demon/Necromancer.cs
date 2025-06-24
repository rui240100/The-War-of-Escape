using UnityEngine;
using UnityEngine.AI;

public class Necromancer : MonoBehaviour
{
    public NavMeshAgent agent;
    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("Collided Object Position: " + collision.transform.position);
    //        collision.gameObject.GetComponent<CharacterController>().enabled = false;
    //        collision.gameObject.transform.position = respawn;
    //        collision.gameObject.GetComponent<CharacterController>().enabled = true;
    //        Debug.Log("Collided Object Position: " + collision.transform.position);
    //        Debug.Log("Collided Object Name: " + collision.gameObject.name);
    //        playerScript = collision.gameObject.GetComponent<Player>();

    //        int keyCountToReturn = playerScript.keyCount;
    //        // Debug.Log("プレイヤーが持っていた鍵の数: " + keyCountToReturn);

    //        playerScript.keyCount = 0;

    //        int placedCount = 0;

    //        foreach (Vector3 pos in keySpawnPositions)
    //        {
    //            if (placedCount >= keyCountToReturn) break;

    //            // 指定位置の周囲にあるオブジェクトを調べる
    //            Collider[] hits = Physics.OverlapSphere(pos, checkRadius);
    //            bool keyExists = false;

    //            foreach (Collider col in hits)
    //            {
    //                if (col.CompareTag("Key"))
    //                {
    //                    keyExists = true;
    //                    break;
    //                }
    //            }

    //            // 鍵がなければ生成
    //            if (!keyExists)
    //            {
    //                Instantiate(keyPrefab, pos, Quaternion.identity);
    //                placedCount++;
    //                //  Debug.Log("鍵を配置しました @ " + pos);
    //            }
    //        }

    //        if (placedCount < keyCountToReturn)
    //        {
    //            //Debug.LogWarning("鍵を戻す場所が足りませんでした！");
    //        }


    //        if (playerScript.playerID == 1)
    //        {
    //            newDemonCamera.player1Chase = false;
    //        }
    //        else if (playerScript.playerID == 2)
    //        {
    //            newDemonCamera.player2Chase = false;
    //        }
    //    }
    //}
}
