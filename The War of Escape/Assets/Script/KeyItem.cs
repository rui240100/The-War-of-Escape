using UnityEngine;

public class KeyItem : MonoBehaviour
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
        //Debug.Log("���������ɐG��܂����I"); // ���O�Ȃ��ł�OK

        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.AddKey(); // �v���C���[�Ɍ���ǉ�
            Destroy(gameObject); // ��������
        }
    }


}
