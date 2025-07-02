using UnityEngine;

public class CallCaltrop : MonoBehaviour
{
    public GameObject caltrop;
    private Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Activate()
    {
        Instantiate(caltrop);
        caltrop.transform.position = player.position;
    }
}
