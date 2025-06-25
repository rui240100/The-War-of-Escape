using UnityEngine;

public class CallNecromancer : MonoBehaviour
{
    public GameObject Necromancer;
    private Transform Player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(Necromancer);
        Necromancer.transform.position = Player.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
