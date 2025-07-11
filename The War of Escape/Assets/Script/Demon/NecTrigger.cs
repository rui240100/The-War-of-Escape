using UnityEngine;

public class NecTrigger : MonoBehaviour
{
    Necromancer necromancer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        necromancer = this.GetComponentInParent<Necromancer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player playerScript = other.GetComponent<Player>();
            if (playerScript.playerID == 1)
            {

            }
            else if(playerScript.playerID == 2)
            {

            }
        }
    }
}
