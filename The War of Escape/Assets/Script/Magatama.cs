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
        //Debug.Log("�����ƂԂ�����: " + other.gameObject.name);

        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.AddMagatama();
            Destroy(gameObject); // �擾�����
        }
    }


}
