using UnityEngine;

public class Item : MonoBehaviour
{
    public float slowDuration = 5f;
    public float slowMultiplier = 0.2f;

    public Sprite icon;
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
        Player player = other.GetComponent<Player>();
        if (player != null && !player.HasItem)
        {
            player.SetHeldItem(this); 
            transform.SetParent(player.transform);
            transform.localPosition = Vector3.zero;
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }



    /*void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && !player.HasItem)
        {
            player.heldItem = this;
            transform.SetParent(player.transform); // ������ɂ�������i�C�Ӂj
            transform.localPosition = Vector3.zero; // �\���ʒu��������
            GetComponent<Collider>().enabled = false; // �E�����瓖���蔻��𖳌���
            GetComponent<MeshRenderer>().enabled = false; // �����Ȃ�����i�C�Ӂj
        }
    }*/

    public void Activate(Player user)
    {
        if (user.otherPlayer != null)
        {
            user.StartCoroutine(user.otherPlayer.SlowDown(slowMultiplier, slowDuration));
        }

        Destroy(gameObject); // �A�C�e���͎g���̂�
    }
}




