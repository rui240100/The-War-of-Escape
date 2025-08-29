using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 判定エリア（Trigger）に入っているプレイヤー本人だけが対応ボタンで宝箱を開ける。
/// 一番近いプレイヤーのみ開けられる仕様。
/// 宝箱に「中身あり」の間はBGMを流し、開けられたら止めて、リセット後に再び流す。
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
    [SerializeField] private AudioClip openSound;   // 開封SE
    [SerializeField] private AudioClip treasureBgm; // 宝箱BGM
    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        // 開始時は「中身あり」なのでBGMを流す
        if (audioSource != null && treasureBgm != null)
        {
            audioSource.clip = treasureBgm;
            audioSource.loop = true;

            // 🔊 3Dサウンド設定
            audioSource.spatialBlend = 1.0f;  // 1 = 完全3D
            audioSource.minDistance = 2f;     // 2m以内はフル音量
            audioSource.maxDistance = 12f;    // 12m以上離れると聞こえない

            audioSource.Play();
        }
    }

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

        // 開封SE
        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }

        // BGMを止める
        if (audioSource != null && audioSource.clip == treasureBgm)
        {
            audioSource.Stop();
        }

        // 60秒後にリセット
        StartCoroutine(ResetChestAfterDelay(60f));
    }

    private IEnumerator ResetChestAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        isOpen = false;

        // 任意：アニメーターに Close トリガーがある場合
        if (animator != null)
        {
            animator.SetTrigger("Close");
        }

        Debug.Log("TreasureBox has been reset and can be opened again.");

        // 中身が補充されたので再びBGMを流す
        if (audioSource != null && treasureBgm != null)
        {
            audioSource.clip = treasureBgm;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
