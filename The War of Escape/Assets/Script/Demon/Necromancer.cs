using UnityEngine;
using UnityEngine.AI;

public class Necromancer : MonoBehaviour
{
    public NavMeshAgent agent;

    private Player playerScript;
    private Transform player;
    public Vector3 respawn;

    public GameObject keyPrefab;
    public Vector3[] keySpawnPositions;
    public float checkRadius = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        Destroy(this.gameObject, 30.0f);
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<CharacterController>().enabled = false;
            collision.gameObject.transform.position = respawn;
            collision.gameObject.GetComponent<CharacterController>().enabled = true;

            playerScript = collision.gameObject.GetComponent<Player>();

            int keyCountToReturn = playerScript.keyCount;

            playerScript.keyCount = 0;

            int placedCount = 0;

            foreach (Vector3 pos in keySpawnPositions)
            {
                if (placedCount >= keyCountToReturn) break;

                // 指定位置の周囲にあるオブジェクトを調べる
                Collider[] hits = Physics.OverlapSphere(pos, checkRadius);
                bool keyExists = false;

                foreach (Collider col in hits)
                {
                    if (col.CompareTag("Key"))
                    {
                        keyExists = true;
                        break;
                    }
                }

                // 鍵がなければ生成
                if (!keyExists)
                {
                    Instantiate(keyPrefab, pos, Quaternion.identity);
                    placedCount++;
                    //  Debug.Log("鍵を配置しました @ " + pos);
                }
            }

            if (placedCount < keyCountToReturn)
            {
                //Debug.LogWarning("鍵を戻す場所が足りませんでした！");
            }
        }
    }
}
