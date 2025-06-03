using UnityEngine;

public class TriggerCamera : MonoBehaviour
{
    public GameObject targetObject;
    private Player target;
    public bool demonHave;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GetComponent<Player>();
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

            if ((demonHave) && (Input.GetMouseButtonDown(0))) 
            {
                //if()
                {
                    target.Speed *= 0.1f;
                    Debug.Log("Stun");

                }
            }
        }
    }
}
