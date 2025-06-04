using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    bool isOpen = false;
    public Animator animator;
    public float openDistance = 3.0f;

    [Header("出現させるアイテムのプレハブ（複数）")]
    public GameObject[] possibleItems; // ここは GameObject（プレハブ）でOK


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

            void OpenChest()
            {
                isOpen = true;
                if (animator != null)
                {
                    animator.SetTrigger("Open");
                }
            }

            void GiveItemToPlayer(Player player)
            {
                if (possibleItems.Length == 0) return;

                int index = Random.Range(0, possibleItems.Length);
                GameObject itemObj = Instantiate(possibleItems[index]);

                // 出てきたのが鍵だった場合
                if (itemObj.TryGetComponent<KeyItem>(out var keyItem))
                {
                    player.AddKey();        // 鍵だけ追加
                    Destroy(itemObj);       // プレハブは即削除
                    return;
                }

                // 通常アイテムだった場合
                if (itemObj.TryGetComponent<Item>(out var item))
                {
                    // すでにアイテムを持っている場合は削除して入れ替え
                    if (player.HasItem)
                    {
                        Destroy(player.heldItem.gameObject);
                    }

                    player.SetHeldItem(item);
                    itemObj.transform.SetParent(player.transform);
                    itemObj.transform.localPosition = Vector3.zero;

                    // 表示や物理演算を無効化（拾った状態）
                    itemObj.GetComponent<Collider>().enabled = false;
                    itemObj.GetComponent<MeshRenderer>().enabled = false;
                }
            }






        }

    }

}