using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBox : MonoBehaviour
{
    [Header("開封フラグ")]
    private bool isOpen = false;

    [Header("開封アニメーター (任意)")]
    [SerializeField] private Animator animator;

    [Header("鍵取得時のエフェクト (任意)")]
    [SerializeField] private GameObject keyVisualEffect;

    [Header("再使用までの待ち時間（秒）")]
    [SerializeField] private float reopenDelay = 60f;

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
        player.AddKey();

        if (keyVisualEffect != null)
        {
            Instantiate(keyVisualEffect, transform.position, Quaternion.identity);
        }
    }

    private void OpenChest()
    {
        isOpen = true;
        if (animator != null) animator.SetTrigger("Open");
        StartCoroutine(ReopenAfterDelay());
    }

    private IEnumerator ReopenAfterDelay()
    {
        yield return new WaitForSeconds(reopenDelay);

        if (animator != null) animator.SetTrigger("Close");
        isOpen = false;
    }
}
