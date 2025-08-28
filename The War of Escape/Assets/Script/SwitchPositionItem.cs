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

    [Header("サウンド設定")]
    public AudioClip countdownBeep;   // 3,2,1 で鳴る音
    public AudioClip countdownFinal;  // 0 で鳴る音
    [Range(0f, 1f)] public float soundVolume = 1f;

    private static AudioSource uiAudioSource; // 共有AudioSource（2D再生）

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

        // 2D用AudioSourceを確保
        if (uiAudioSource == null)
        {
            GameObject go = new GameObject("CountdownAudioSource");
            DontDestroyOnLoad(go); // シーン切り替えでも残す
            uiAudioSource = go.AddComponent<AudioSource>();
            uiAudioSource.spatialBlend = 0f; // 2Dサウンド
            uiAudioSource.playOnAwake = false;
        }
    }

    public override void Activate(Player user)
    {
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

        // 入れ替え処理
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
        if (countdownBeep != null && uiAudioSource != null)
        {
            uiAudioSource.PlayOneShot(countdownBeep, soundVolume);
        }
    }

    private void PlayFinal()
    {
        if (countdownFinal != null && uiAudioSource != null)
        {
            uiAudioSource.PlayOneShot(countdownFinal, soundVolume);
        }
    }
}
