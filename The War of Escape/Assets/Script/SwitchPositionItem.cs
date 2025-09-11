using System.Collections;
using UnityEngine;
using TMPro;

public class SwitchPositionItem : Item
{
    private static GameObject countdownUI;
    private static TextMeshProUGUI countdownText;
    private bool isUsed = false;

    private Player playerScriptSw;
    private string useMessageSw1 = "";
    private string useMessageSw2 = "";
    private GameObject itemUseSw;
    private ItemUse itemUseScSw;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip countdownBeep;   // 3,2,1 で鳴る音
    [SerializeField] private AudioClip countdownFinal;  // 0 で鳴る音
    [SerializeField] private AudioClip useSound;           // 使用時に鳴らす音
    [SerializeField] private AudioSource audioSource;   // Inspectorでセット
    [Range(0f, 1f)] public float soundVolume = 1f;

    void Start()
    {
        // UI探し
        if (countdownUI == null)
        {
            GameObject uiRoot = GameObject.Find("SwitchPositionCountdownUI");
            if (uiRoot != null)
            {
                countdownUI = uiRoot.transform.Find("SwitchPositionCountdown")?.gameObject;

                if (countdownUI != null)
                {
                    countdownText = countdownUI.transform.Find("CountText")
                                    ?.GetComponent<TextMeshProUGUI>();

                    countdownUI.SetActive(false);
                }
            }
        }

        // メッセージ用
        itemUseSw = GameObject.Find("ItemUse");
        itemUseScSw = itemUseSw.GetComponent<ItemUse>();
        if (itemUseScSw == null)
        {
            Debug.LogError("MessageDisplayManagerがシーンにありません！");
        }
    }

    public override void Activate(Player user)
    {
        if (audioSource != null && useSound != null)
        {
            audioSource.PlayOneShot(useSound);
        }

        if (isUsed || user.otherPlayer == null) return;

        isUsed = true;
        user.StartCoroutine(SwitchPositionsAfterDelay(user));

        playerScriptSw = user.GetComponent<Player>();

        if (itemUseScSw != null)
        {
            if (playerScriptSw.playerID == 1)
            {
                itemUseScSw.ShowMessage(useMessageSw1, useMessageSw2);
            }
            else if (playerScriptSw.playerID == 2)
            {
                itemUseScSw.ShowMessage(useMessageSw2, useMessageSw1);
            }
        }
    }

    private IEnumerator SwitchPositionsAfterDelay(Player user)
    {
        if (countdownUI != null && countdownText != null)
        {
            countdownUI.SetActive(true);

            countdownText.text = $"{user.playerID + 1}Pが位置入れ替えアイテムを使用！";
            yield return new WaitForSeconds(2f);

            // 3
            countdownText.text = "3...";
            PlayBeep();
            yield return new WaitForSeconds(1f);

            // 2
            countdownText.text = "2...";
            PlayBeep();
            yield return new WaitForSeconds(1f);

            // 1
            countdownText.text = "1...";
            PlayBeep();
            yield return new WaitForSeconds(1f);

            // 0
            countdownText.text = "0...";
            PlayFinal();
        }

        // プレイヤー位置を入れ替え
        Transform other = user.otherPlayer.transform;
        Debug.Log($"Before Swap: User={user.transform.position}, Other={other.position}");

        Vector3 temp = user.transform.position;
        user.transform.position = other.position;
        other.position = temp;

        Debug.Log($"After Swap: User={user.transform.position}, Other={other.position}");

        yield return new WaitForSeconds(0.5f);

        if (countdownUI != null)
        {
            countdownUI.SetActive(false);
        }

        user.SetHeldItem(null);
    }

    private void PlayBeep()
    {
        if (countdownBeep != null && audioSource != null)
        {
            audioSource.PlayOneShot(countdownBeep, soundVolume);
        }
    }

    private void PlayFinal()
    {
        if (countdownFinal != null && audioSource != null)
        {
            audioSource.PlayOneShot(countdownFinal, soundVolume);
        }
    }
}
