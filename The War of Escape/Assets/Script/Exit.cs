using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro用

public class Exit : MonoBehaviour
{
    private bool isPlayerInTrigger = false;
    private Player currentPlayer;
    private float holdTime = 0f;

    [Header("滞在に必要な秒数")]
    [SerializeField] private float requiredHoldTime = 7f;

    [Header("必要な鍵の数")]
    [SerializeField] private int requiredKeys = 3;

    [Header("UI表示用")]
    public TextMeshProUGUI countdownText; // インスペクターで設定

    [Header("サウンド設定")]
    [SerializeField] private AudioClip lockedSound;       // 鍵不足のときの音
    [SerializeField] private AudioClip clearSound;        // カウントダウン完了時の音
    [SerializeField] private AudioClip countdownTickSound; // 秒ごとに鳴らす音
    private AudioSource audioSource;

    private bool hasPlayedLockedSound = false; // 鍵不足音用フラグ
    private bool hasPlayedClearSound = false;  // クリア音用フラグ
    private int lastRemainingTime = -1;        // カウントダウン音用

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    [System.Obsolete]
    void Update()
    {
        if (isPlayerInTrigger && currentPlayer != null)
        {
            // 🔑 鍵の数チェック
            if (currentPlayer.keyCount < requiredKeys)
            {
                // 鍵不足メッセージを表示
                if (countdownText != null)
                {
                    countdownText.text = $"鍵が足りません！（{currentPlayer.keyCount}/{requiredKeys}）";
                }

                // 範囲に入った瞬間だけ2回音を鳴らす
                if (!hasPlayedLockedSound && lockedSound != null)
                {
                    StartCoroutine(PlayLockedSoundTwice());
                    hasPlayedLockedSound = true;
                }

                // カウントダウンは進めない
                holdTime = 0f;
                lastRemainingTime = -1;
                hasPlayedClearSound = false;
                return;
            }

            // ✅ 鍵が足りる場合 → カウントダウン処理
            holdTime += Time.deltaTime;

            int remainingTime = Mathf.CeilToInt(requiredHoldTime - holdTime);
            if (remainingTime < 0) remainingTime = 0;

            // 秒が減ったらカウントダウン音を鳴らす
            if (remainingTime != lastRemainingTime)
            {
                if (countdownTickSound != null)
                {
                    audioSource.PlayOneShot(countdownTickSound);
                }
                lastRemainingTime = remainingTime;
            }

            if (countdownText != null)
            {
                countdownText.text = remainingTime.ToString("0");
            }

            // 指定時間経過 → クリア処理
            if (holdTime >= requiredHoldTime)
            {
                if (!hasPlayedClearSound && clearSound != null)
                {
                    audioSource.PlayOneShot(clearSound);
                    hasPlayedClearSound = true;
                }

                ResultData.escapedPlayerID = currentPlayer.playerID;
                FadeManager.Instance.LoadScene("Clear", 2.0f);

                ResetTrigger();
            }
        }
        else
        {
            // 範囲外では非表示
            if (countdownText != null)
            {
                countdownText.text = "";
            }
            holdTime = 0f;
            hasPlayedLockedSound = false;
            hasPlayedClearSound = false;
            lastRemainingTime = -1;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            currentPlayer = player;
            isPlayerInTrigger = true;
            holdTime = 0f;
            hasPlayedLockedSound = false;
            hasPlayedClearSound = false;
            lastRemainingTime = -1;
            Debug.Log("出口範囲に入りました: " + player.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && player == currentPlayer)
        {
            ResetTrigger();
            Debug.Log("出口範囲から出ました: " + player.name);
        }
    }

    private void ResetTrigger()
    {
        isPlayerInTrigger = false;
        currentPlayer = null;
        holdTime = 0f;
        hasPlayedLockedSound = false;
        hasPlayedClearSound = false;
        lastRemainingTime = -1;

        if (countdownText != null)
        {
            countdownText.text = "";
        }
    }

    private IEnumerator PlayLockedSoundTwice()
    {
        audioSource.PlayOneShot(lockedSound);
        yield return new WaitForSeconds(0.2f);
        audioSource.PlayOneShot(lockedSound);
    }
}
