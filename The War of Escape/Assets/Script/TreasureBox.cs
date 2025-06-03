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
                if (player != null && !player.HasItem)
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

                // まず Item か KeyItem かを判定
                Item item = itemObj.GetComponent<Item>();
                KeyItem keyItem = itemObj.GetComponent<KeyItem>();

                if (item != null)
                {
                    // 通常アイテム処理
                    player.SetHeldItem(item);
                    itemObj.transform.SetParent(player.transform);
                    itemObj.transform.localPosition = Vector3.zero;
                    itemObj.GetComponent<Collider>().enabled = false;
                    itemObj.GetComponent<MeshRenderer>().enabled = false;
                }
                else if (keyItem != null)
                {
                    // 鍵を直接プレイヤーに渡す処理
                    player.AddKey();
                    Destroy(itemObj); // 見えないけど取得処理完了なので消す
                }
            }






        }

    }

}