using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertControlItem : Item
{
    [Header("Invert Settings")]
    [SerializeField] private float duration = 10f; // 反転が続く時間

    private Player playerScriptRe;
    private string useMessageRe1 = "操作反転アイテムを使用しました";
    private string useMessageRe2 = "移動操作が反転されました";
    private GameObject itemUseRe;
    private ItemUse itemUseScRe;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip useSound;           // 使用時に鳴らす音
    [SerializeField] private AudioSource audioSource;      // AudioSourceをInspectorでセット

    private void Start()
    {
        // シーン内のMessageDisplayManagerを探す
        itemUseRe = GameObject.Find("ItemUse");
        itemUseScRe = itemUseRe.GetComponent<ItemUse>();
        if (itemUseScRe == null)
        {
            Debug.LogError("MessageDisplayManagerがシーンにありません！");
        }
    }

    public override void Activate(Player user)
    {
        // 🎵 使用音を再生
        if (audioSource != null && useSound != null)
        {
            audioSource.PlayOneShot(useSound);
        }

        if (user.otherPlayer != null)
        {
            user.StartCoroutine(InvertControl(user.otherPlayer, user));
        }

        playerScriptRe = user.GetComponent<Player>();

        if (itemUseScRe != null)
        {
            if (playerScriptRe.playerID == 1)
            {
                itemUseScRe.ShowMessage(useMessageRe1, useMessageRe2);
            }
            else if (playerScriptRe.playerID == 2)
            {
                itemUseScRe.ShowMessage(useMessageRe2, useMessageRe1);
            }
        }

        user.SetHeldItem(null);
    }

    private IEnumerator InvertControl(Player targetPlayer, Player user)
    {
        InvertedInput inverted = targetPlayer.gameObject.GetComponent<InvertedInput>();
        if (inverted == null)
        {
            inverted = targetPlayer.gameObject.AddComponent<InvertedInput>();
        }

        inverted.EnableInversion();

        yield return new WaitForSeconds(duration);

        inverted.DisableInversion();
    }
}
