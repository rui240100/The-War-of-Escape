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
        //Debug.Log("âΩÇ©Ç∆Ç‘Ç¬Ç©Ç¡ÇΩ: " + other.gameObject.name);

        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.AddMagatama();
            Destroy(gameObject); // éÊìæå„è¡Ç∑
        }
    }


}
