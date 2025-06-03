using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject targetObject;
    public bool demonHave;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == targetObject)
        {
            Debug.Log("Trigger");

            if (demonHave)
            {
                if
            }
        }
    }
}
