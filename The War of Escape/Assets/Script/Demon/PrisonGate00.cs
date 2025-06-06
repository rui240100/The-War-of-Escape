using UnityEngine;

public class PrisonGate00 : MonoBehaviour
{
    public GameObject protectingDemonObj;
    public GameObject player;
    public bool playerID00;

    [HideInInspector]
    public ProtectingDemon00 protectingDemon00;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("a");

        if (other.gameObject == player)
        {
            Debug.Log("i");

            playerID00 = true;

            protectingDemon00 = protectingDemonObj.GetComponent<ProtectingDemon00>();

            protectingDemon00.Launch();

            Debug.Log("u");
        }
    }
}
