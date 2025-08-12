using UnityEngine;

public class Magatama : MonoBehaviour
{
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
        //Debug.Log("Õ“Ë‘Šè: " + other.name); // © ‚±‚ê’Ç‰ÁI

        Player player = other.GetComponentInParent<Player>();
        if (player != null)
        {

            //Debug.Log($"PlayerID: {player.playerID} ‚ªŒù‹Ê‚ğæ“¾");
            //player.AddMagatama();
            Destroy(gameObject); // æ“¾ŒãÁ‚·
        }
    }


}
