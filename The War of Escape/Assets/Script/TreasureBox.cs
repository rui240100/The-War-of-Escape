using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 判定エリア（Trigger）に入っているプレイヤー本人だけが対応ボタンで宝箱を開ける。
/// 1P は Fire2 / 2P は Fire2_2 を使う想定。
/// </summary>
public class TreasureBox : MonoBehaviour
{
    [Header("開封フラグ")] private bool isOpen = false;

    [Header("開封アニメーター (任意)")]
    [SerializeField] private Animator animator;

    [Header("出現させるアイテムのプレハブ")]
    [SerializeField] private GameObject[] possibleItems;

    // ▶ エリア内プレイヤーを保持
    private readonly List<Player> playersInRange = new();



    void Start()
    {
        foreach (string name in Input.GetJoystickNames())
        {
            Debug.Log("接続中のジョイスティック: " + name);
        }
    }





    /* ===== Update ===== */
    private void Update()
    {
        if (isOpen) return;

        foreach (Player p in playersInRange)
        {
            if (IsInteractPressed(p))
            {
                GiveItemToPlayer(p);
                OpenChest();
                break;
            }
        }
    }

    /* ===== Trigger ===== */
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player p = other.GetComponent<Player>();
            if (p != null && !playersInRange.Contains(p))
                playersInRange.Add(p);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player p = other.GetComponent<Player>();
            if (p != null) playersInRange.Remove(p);
        }
    }

    /* ===== 入力判定 ===== */
    private bool IsInteractPressed(Player p) =>
        p.playerID switch
        {
            1 => Input.GetButtonDown("Fire2"),
            2 => Input.GetButtonDown("Fire2_2"),
            _ => false
        };

    /* ===== アイテム付与 ===== */
    private void GiveItemToPlayer(Player player)
    {
        player.AddMagatama();                          // 勾玉 +1

        if (possibleItems.Length == 0) return;         // 何も設定されていなければ終了

        GameObject obj = Instantiate(
            possibleItems[Random.Range(0, possibleItems.Length)]);

        // 鍵だった場合
        if (obj.TryGetComponent<KeyItem>(out _))
        {
            player.AddKey();
            Destroy(obj);
            return;
        }

        // 通常アイテム
        if (obj.TryGetComponent<Item>(out Item item))
        {
            if (player.HasItem) Destroy(player.heldItem.gameObject);

            player.SetHeldItem(item);
            obj.transform.SetParent(player.transform);
            obj.transform.localPosition = Vector3.zero;

            obj.GetComponent<Collider>().enabled = false;
            obj.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    /* ===== 開封 ===== */
    private void OpenChest()
    {
        isOpen = true;
        if (animator != null) animator.SetTrigger("Open");
    }
}
