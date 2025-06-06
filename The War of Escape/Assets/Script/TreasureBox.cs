using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    bool isOpen = false;
    public Animator animator;
    public float openDistance = 3.0f;

    [Header("�o��������A�C�e���̃v���n�u�i�����j")]
    public GameObject[] possibleItems; // ������ GameObject�i�v���n�u�j��OK


    //public GameObject magatamaPrefab;// �m��ŏo�����ʂ̃v���n�u


    void Update()
    {
        if (isOpen) return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerObj in players)
        {
            float distance = Vector3.Distance(transform.position, playerObj.transform.position);

            if (distance <= openDistance && Input.GetButtonDown("Fire2"))
            {
                Player player = playerObj.GetComponent<Player>();
                if (player != null)
                {

                    GiveItemToPlayer(player);
                    OpenChest();
                }
                break;
            }

            



        }


    }


    void GiveItemToPlayer(Player player)
    {
        //�v���C���[�̌��ʂ̐���+1������
        player.AddMagatama(); // �� ���ʂ̏������𒼐ځ{1�I���ꂾ����OK�I

        // �� or �A�C�e���������_���ŏo��
        if (possibleItems.Length == 0) return;

        int index = Random.Range(0, possibleItems.Length);
        GameObject itemObj = Instantiate(possibleItems[index]);

        // �o�Ă����̂����������ꍇ
        if (itemObj.TryGetComponent<KeyItem>(out var keyItem))
        {
            player.AddKey();        // �������ǉ�
            Destroy(itemObj);       // �v���n�u�͑��폜
            return;
        }

        // �ʏ�A�C�e���������ꍇ
        if (itemObj.TryGetComponent<Item>(out var item))
        {
            // ���łɃA�C�e���������Ă���ꍇ�͍폜���ē���ւ�
            if (player.HasItem)
            {
                Destroy(player.heldItem.gameObject);
            }

            player.SetHeldItem(item);
            itemObj.transform.SetParent(player.transform);
            itemObj.transform.localPosition = Vector3.zero;

            // �\���╨�����Z�𖳌����i�E������ԁj
            itemObj.GetComponent<Collider>().enabled = false;
            itemObj.GetComponent<MeshRenderer>().enabled = false;
        }
    }


    void OpenChest()
    {
        isOpen = true;
        if (animator != null)
        {
            animator.SetTrigger("Open");
        }
    }

}