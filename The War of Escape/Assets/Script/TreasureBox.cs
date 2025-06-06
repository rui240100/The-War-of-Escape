using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    bool isOpen = false;
    public Animator animator;
    public float openDistance = 3.0f;

    [Header("出現させるアイテムのプレハブ（複数）")]
    public GameObject[] possibleItems; // ここは GameObject（プレハブ）でOK


    //public GameObject magatamaPrefab;// 確定で出す勾玉のプレハブ


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
        //プレイヤーの勾玉の数を+1させる
        player.AddMagatama(); // ← 勾玉の所持数を直接＋1！これだけでOK！

        // 鍵 or アイテムをランダムで出す
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


    void OpenChest()
    {
        isOpen = true;
        if (animator != null)
        {
            animator.SetTrigger("Open");
        }
    }

}