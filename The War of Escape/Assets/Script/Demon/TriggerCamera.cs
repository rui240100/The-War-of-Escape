using UnityEngine;

public class TriggerCamera : MonoBehaviour
{
    public GameObject targetObject;
    private Player target;
    private TriggerCamera targetCamera;
    public bool demonHave;

    private ProtectingDemon protectingDemon; 
    private DemonAI demon;

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
            target = other.GetComponent<Player>();
            targetCamera = target.GetComponent<TriggerCamera>();
            Debug.Log("Trigger");

            if (demonHave)
            {
                if (!targetCamera.demonHave)
                {
                    target.Speed *= 0.1f;
                    //Time
                }
                else
                {
                    //Ç±Ç±Ç…å›Ç¢ÇÃéÁåÏãSÇDestroyÇ∑ÇÈÇÃèëÇ≠
                }
            }
        }
        else if (other.CompareTag("Demon"))
        {
            demon = other.GetComponent<DemonAI>();
            demon.chaseSpeed *= 0.1f;
            //Time
            demon.StopChase();
        }
    }
}
