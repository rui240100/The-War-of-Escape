using UnityEngine;

public class CallCaltrop : Item
{
    public GameObject caltrop;

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
        Debug.Log("Ç‹Ç´Ç—Çµê›íu0");
        GameObject caltropObj = Instantiate(caltrop);
        caltropObj.transform.position = user.transform.position;
        Debug.Log("Ç‹Ç´Ç—Çµê›íu1");
    }
}
