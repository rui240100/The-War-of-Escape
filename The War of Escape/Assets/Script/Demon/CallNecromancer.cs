using UnityEngine;

public class CallNecromancer : Item
{
    public GameObject Necromancer;
    private Necromancer necromancerScript;
    private Player playerScript;

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
        GameObject NecromancerObj = Instantiate(Necromancer);
        NecromancerObj.transform.position = user.transform.position;
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
}
