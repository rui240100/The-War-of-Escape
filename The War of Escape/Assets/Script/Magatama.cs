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
        //Debug.Log("�Փˑ���: " + other.name); // �� ����ǉ��I

        Player player = other.GetComponentInParent<Player>();
        if (player != null)
        {

            //Debug.Log($"PlayerID: {player.playerID} �����ʂ��擾");
            player.AddMagatama();
            Destroy(gameObject); // �擾�����
        }
    }


}
