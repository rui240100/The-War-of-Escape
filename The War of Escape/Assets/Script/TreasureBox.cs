using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 判定エリア（Trigger）に入っているプレイヤー本人だけが対応ボタンで宝箱を開ける。
/// 1P は Fire2 / 2P は Fire2_2 を使う想定。
/// 一番近いプレイヤーのみ開けられる仕様に改修済み。
/// </summary>
public class TreasureBox : MonoBehaviour
{
    [Header("開封フラグ")]
    private bool isOpen = false;

    [Header("開封アニメーター (任意)")]
    [SerializeField] private Animator animator;

    [Header("出現させるアイテムのプレハブ")]
    [SerializeField] private GameObject[] possibleItems;

    private readonly List<Player> playersInRange = new();

    [Header("サウンド")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioSource audioSource;

    void Update()
    {
        if (isOpen) return;
        if (playersInRange.Count == 0) return;

        Vector3 chestPos = transform.position;

        Player closestPlayer = null;
        float minDist = float.MaxValue;

        foreach (var p in playersInRange)
        {
            float dist = Vector3.Distance(p.transform.position, chestPos);
            if (dist < minDist)
            {
                minDist = dist;
                closestPlayer = p;
            }
        }

        if (closestPlayer != null && IsInteractPressed(closestPlayer))
        {
            Debug.Log($"TreasureBox opened by Player {closestPlayer.playerID}");
            GiveItemToPlayer(closestPlayer);
            OpenChest();
        }
    }

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

    private bool IsInteractPressed(Player p)
    {
        return (Input.GetButtonDown("Fire2") && p.playerID == 1) ||
               (Input.GetButtonDown("Fire2_2") && p.playerID == 2);
    }

    private void GiveItemToPlayer(Player player)
    {
        //player.AddMagatama(); // 勾玉 +1

        if (possibleItems.Length == 0) return;

        GameObject obj = Instantiate(possibleItems[Random.Range(0, possibleItems.Length)]);

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
            if (player.HasItem)
            {
                Destroy(player.heldItem.gameObject);
            }

            player.SetHeldItem(item);
            obj.transform.SetParent(player.transform);
            obj.transform.localPosition = Vector3.zero;

            var collider = obj.GetComponent<Collider>();
            if (collider != null) collider.enabled = false;

            var meshRenderer = obj.GetComponent<MeshRenderer>();
            if (meshRenderer != null) meshRenderer.enabled = false;
        }
    }

    private void OpenChest()
    {
        isOpen = true;

        if (animator != null) animator.SetTrigger("Open");

        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }

        // 30秒後に宝箱をリセットして再び開けられるようにする
        StartCoroutine(ResetChestAfterDelay(60f));
    }

    private IEnumerator ResetChestAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        isOpen = false;

        // 任意：アニメーターに Close トリガーがある場合はこれも実行
        if (animator != null)
        {
            animator.SetTrigger("Close"); // Closeアニメーションを再生（あれば）
        }

        Debug.Log("TreasureBox has been reset and can be opened again.");
    }
}
