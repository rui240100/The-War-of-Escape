using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鍵しか出ない宝箱（KeyBox）。
/// 一番近いプレイヤーのみ、対応ボタン（Fire2 / Fire2_2）で開封可能。
/// 鍵とエフェクトを付与する。
/// </summary>
public class KeyBox : MonoBehaviour
{
    [Header("開封フラグ")]
    private bool isOpen = false;

    [Header("開封アニメーター (任意)")]
    [SerializeField] private Animator animator;

    [Header("鍵取得時のエフェクト (任意)")]
    [SerializeField] private GameObject keyVisualEffect;

    // エリア内のプレイヤー保持
    private readonly List<Player> playersInRange = new();

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
            Debug.Log($"KeyBox opened by Player {closestPlayer.playerID}");
            GiveKeyToPlayer(closestPlayer);
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

    private void GiveKeyToPlayer(Player player)
    {
        player.AddKey();  // 鍵を1つ加算

        if (keyVisualEffect != null)
        {
            Instantiate(keyVisualEffect, transform.position, Quaternion.identity);
        }
    }

    private void OpenChest()
    {
        isOpen = true;
        if (animator != null) animator.SetTrigger("Open");
    }
}
