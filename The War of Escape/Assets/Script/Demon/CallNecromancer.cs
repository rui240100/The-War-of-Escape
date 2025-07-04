using UnityEngine;

public class CallNecromancer : Item
{
    public GameObject Necromancer;
    private Transform Player;

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
        Instantiate(Necromancer);
        Necromancer.transform.position = Player.position;
    }
}
