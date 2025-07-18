using System.Collections.Generic;
using UnityEngine;

public class GhostItem : Item
{
    [Header("Steal Settings")]
    [SerializeField] private int stealAmount = 1;

    [Header("Consolation Items")]
    [SerializeField] private List<GameObject> consolationItems = new();

    private Player playerScriptRo;
    private string useMessageRo1 = "";
    private string useMessageRo2 = "";
    private GameObject itemUseRo;
    private ItemUse itemUseScRo;

    private void Start()
    {
        // シーン内のMessageDisplayManagerを探す
        itemUseRo = GameObject.Find("ItemUse");
        itemUseScRo = itemUseRo.GetComponent<ItemUse>();
        if (itemUseScRo == null)
        {
            Debug.LogError("MessageDisplayManagerがシーンにありません！");
        }
    }

    public override void Activate(Player user)
    {
        if (user == null || user.otherPlayer == null)
        {
            Debug.LogWarning("GhostItem: userまたはotherPlayerがnullです。");
            return;
        }

        Player target = user.otherPlayer;

        if (target.keyCount > 0)
        {
            int keysToSteal = stealAmount <= 0 ? target.keyCount: Mathf.Min(stealAmount, target.keyCount);

            for (int i = 0; i < keysToSteal; i++)
            {
                target.RemoveKey();
                user.AddKey();
            }

            Debug.Log($"GhostItem: {keysToSteal} 鍵を Player{target.playerID} から Player{user.playerID} に奪いました。");
        }
        else
        {
            Debug.Log($"GhostItem: Player{target.playerID} は鍵を持っていません。代替アイテムを探します。");

            if (consolationItems != null && consolationItems.Count > 0)
            {
                int index = Random.Range(0, consolationItems.Count);
                GameObject selectedPrefab = consolationItems[index];

                if (selectedPrefab == null)
                {
                    Debug.LogWarning("GhostItem: 選ばれたプレハブが null です！");
                    return;
                }

                GameObject obj = Instantiate(selectedPrefab);
                Debug.Log($"GhostItem: {selectedPrefab.name} を生成しました。");

                if (obj.TryGetComponent<Item>(out Item newItem))
                {
                    if (user.HasItem)
                    {
                        Debug.Log($"GhostItem: 既存アイテム {user.heldItem.name} を削除します。");
                        Destroy(user.heldItem.gameObject);
                    }

                    user.SetHeldItem(newItem);
                    obj.transform.SetParent(user.transform);
                    obj.transform.localPosition = Vector3.zero;

                    var col = obj.GetComponent<Collider>();
                    if (col) col.enabled = false;

                    var mesh = obj.GetComponent<MeshRenderer>();
                    if (mesh) mesh.enabled = false;

                    Debug.Log($"GhostItem: {newItem.name} を Player{user.playerID} に渡しました。");
                }
                else
                {
                    Debug.LogWarning("GhostItem: インスタンスに Item スクリプトが付いていません！");
                    Destroy(obj);
                }
            }
            else
            {
                Debug.LogWarning("GhostItem: consolationItems が空です。何も渡せません。");
            }
        }

        playerScriptRo = user.GetComponent<Player>();

        if (itemUseScRo != null)
        {
            if (playerScriptRo.playerID == 1)
            {
                string message1 = useMessageRo1;
                string message2 = useMessageRo2;
                itemUseScRo.ShowMessage(message1, message2);
            }
            else if (playerScriptRo.playerID == 2)
            {
                string message1 = useMessageRo2;
                string message2 = useMessageRo1;
                itemUseScRo.ShowMessage(message1, message2);
            }
        }

        Destroy(gameObject);
    }
}
