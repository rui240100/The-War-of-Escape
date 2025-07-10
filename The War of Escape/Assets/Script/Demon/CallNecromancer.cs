using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CallNecromancer : Item
{
    public GameObject Necromancer;
    private Necromancer necromancerScript;
    private Player playerScript;
    public Vector3[] NecSpawn;
    public float checkRadius = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Activate(Player user)
    {
        Vector3 safePosition = Vector3.zero;
        bool foundSafe = false;

        NecSpawn[1] = new Vector3(user.transform.position.x + 2.0f, user.transform.position.y, user.transform.position.z);
        NecSpawn[2] = new Vector3(user.transform.position.x - 2.0f, user.transform.position.y, user.transform.position.z);
        NecSpawn[3] = new Vector3(user.transform.position.x, user.transform.position.y, user.transform.position.z + 2.0f);
        NecSpawn[4] = new Vector3(user.transform.position.x, user.transform.position.y, user.transform.position.z - 2.0f);
        
        foreach (Vector3 position in NecSpawn)
        {
            Collider[] hits = Physics.OverlapSphere(position,checkRadius);

            if (hits.Length == 0)
            {
                safePosition = position;
                foundSafe = true;
                break; // ç≈èâÇ…å©Ç¬ÇØÇΩà¿ëSÇ»èÍèäÇ≈é~ÇﬂÇÈ
            }
        }

        GameObject NecromancerObj;

        if (foundSafe)
        {
            NecromancerObj = Instantiate(Necromancer);
            NecromancerObj.transform.position = safePosition;
            necromancerScript = NecromancerObj.GetComponent<Necromancer>();

            playerScript = user.GetComponent<Player>();
            if (playerScript.playerID == 1)
            {
                necromancerScript.player2 = true;
            }
            else if (playerScript.playerID == 2)
            {
                necromancerScript.player1 = true;
            }
        }
        else
        {
            Debug.Log("No safe position");
        }

        
    }
}
