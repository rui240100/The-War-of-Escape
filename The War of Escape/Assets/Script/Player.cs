using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int playerID;
    public float Speed = 5f; // 最大移動速度
    public Transform cameraTransform;
    public float lookSensitivity = 3.0f;
    CharacterController characterController;
    Animator animator;

    public Player otherPlayer;
    bool isSlowing = false;

    public int keyCount = 0;
    public Item heldItem;

    public bool HasItem => heldItem != null;

    public Image itemIconUI;
    public Sprite defaultItemIcon;
    public Image itemBackgroundUI;

    //public Image keyIconUI;
    public Sprite keyIconSprite;
    public Sprite defaultKeyIcon;
    //public Sprite[] keyIcons;
    public Text keyCountText;

    public int magatamaCount = 0;
    public MagatamaUIManager magatamaUIManager;

    public GameObject pd;

    public float idleAnimationSpeed = 2.0f;  // Idle速度
    public float runAnimationSpeed = 2.0f;   // Run速度

    public Sprite itemBackgroundSprite; // 宝箱の画像（常に表示したいやつ）

    public bool buttonDown = false;
    public GameObject itemManu;
    public DemonAI demonAI;
    public GameDrector gameDrector;
    bool playerStart = false;
    public TextMove textMove;
    public Player player;
    private bool playerStart2;


    /*[Header("プレイヤーの視点関係")]
    public float verticalRotation = 0f;
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;*/

    [Header("足音の設定")]
    public AudioSource footstepAudioSource;
    public AudioClip footstepClip;
    public float footstepInterval = 0.5f;
    private float footstepTimer = 0f;

    // エディタで調整できるピッチ倍率
    [Range(0.1f, 3f)]
    public float baseFootstepPitch = 1.0f;


    [Header("鍵UI関連")]
    public Image keyIconImage; // 鍵の表示を切り替えるImage
    public Sprite[] keySprites; // 鍵0〜5個用のスプライト（6枚）
    public GameObject keyUIContainer; // 背景の箱画像（常に表示したいなら）

    [Header("アイテム使用時の効果音")]
    public AudioSource itemUseSound;


    [Header("エフェクト関連")]
    public ParticleSystem speedUpEffect; // スピードアップ時のパーティクル

    void Start()
    {

        textMove.StartDesu();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        StartCoroutine(FindOtherPlayerWithDelay());

        /*if (itemIconUI != null && defaultItemIcon != null)
        {
            itemIconUI.enabled = true;
        }*/

        if (itemIconUI != null)
        {
            itemIconUI.enabled = false;  // 最初は非表示にしておく！
        }


        Application.targetFrameRate = 60;
        UpdateKeyUI();

        if (magatamaUIManager != null)
        {
            magatamaUIManager.ResetMagatamaUI();
        }

        if (itemBackgroundUI != null && itemBackgroundSprite != null)
        {
            itemBackgroundUI.sprite = itemBackgroundSprite;
            itemBackgroundUI.enabled = true; // 念のため表示ONに
        }

        if (keyUIContainer != null)
        {
            keyUIContainer.SetActive(true); 
        }


        if (footstepAudioSource != null)
        {
            footstepAudioSource.clip = footstepClip;
            footstepAudioSource.loop = false;
        }


    }

    void Update()
    {
        //if(!playerStart)
        //    Speed = 0.0f;
        //if (playerStart && playerStart2 == false)
        //{
        //    Speed = 5.0f;
        //    playerStart2 = true;
        //}
            

        float x = 0f;
        float z = 0f;
        float mouseX = 0f;
        float mouseY = 0f;




        //if (playerStart)
        {
            if (playerID == 1)
            {
                InvertedInput inv = GetComponent<InvertedInput>();
                x = (inv != null) ? inv.GetAxisRaw("Horizontal1") : Input.GetAxis("Horizontal1");
                z = (inv != null) ? inv.GetAxisRaw("Vertical1") : Input.GetAxis("Vertical1");

                mouseX = Input.GetAxis("Mouse X");
                mouseY = Input.GetAxis("Mouse Y");
            }
            else if (playerID == 2)
            {
                InvertedInput inv = GetComponent<InvertedInput>();
                x = (inv != null) ? inv.GetAxisRaw("Horizontal2") : Input.GetAxis("Horizontal2");
                z = (inv != null) ? inv.GetAxisRaw("Vertical2") : Input.GetAxis("Vertical2");

                mouseX = Input.GetAxis("Mouse X2");
                mouseY = Input.GetAxis("Mouse Y2");
            }
        }
        

    




        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 move = (forward * z + right * x).normalized;



        // ★ 倒し具合に応じたスピード計算
        float magnitude = Mathf.Clamp01(new Vector2(x, z).magnitude);
        float currentSpeed = Speed * magnitude;

        // 入力方向を正規化したベクトルに掛ける
        characterController.SimpleMove(move * currentSpeed);

        // アニメーション制御（IdleとRunを個別に調整）
        if (animator != null)
        {
            if (move.magnitude > 0.1f)
            {
                animator.speed = runAnimationSpeed;
                animator.Play("Run");
            }
            else
            {
                animator.speed = idleAnimationSpeed;
                animator.Play("Idle");
            }
        }

        if (((playerID == 1 && Input.GetButtonDown("Fire2")) ||
             (playerID == 2 && Input.GetButtonDown("Fire2_2"))))
        {
            //if (!buttonDown)
            //{
            //    if (itemManu != null)
            //    {
            //        Destroy(itemManu);
            //    }

            //    buttonDown = true;
            //    demonAI.startDemon = true;
            //    //if (gameDrector.startGame == false)
            //    {
                    
            //    }
            //    gameDrector.startGame = true;
            //    playerStart = true;
            //    player.playerStart = true;
            //    player.buttonDown = true;
            //}
        }

        if (((playerID == 1 && Input.GetButtonDown("Fire3")) ||
             (playerID == 2 && Input.GetButtonDown("Fire3_2"))) && HasItem)
        {
            UseItem();
        }


        // 足音の再生（ピッチ調整あり）
        if (move.magnitude > 0.1f && characterController.isGrounded)
        {
            footstepTimer += Time.deltaTime;
            if (move.magnitude > 0.1f && characterController.isGrounded)
            {
                footstepTimer += Time.deltaTime;

                if (footstepTimer >= footstepInterval)
                {
                    // ピッチ調整（←ここ！）
                    float pitch = isSlowing ? baseFootstepPitch * 0.7f : baseFootstepPitch * Mathf.Lerp(1.0f, 1.3f, move.magnitude);
                    footstepAudioSource.pitch = pitch;

                    footstepAudioSource.PlayOneShot(footstepClip);
                    footstepTimer = 0f;
                }
            }
        }
        else
        {
            footstepTimer = footstepInterval;
        }





    }

    IEnumerator FindOtherPlayerWithDelay()
    {
        yield return new WaitForSeconds(0.1f);

        Player[] allPlayers = FindObjectsByType<Player>(FindObjectsSortMode.None);
        foreach (var p in allPlayers)
        {
            if (p.playerID != this.playerID)
            {
                otherPlayer = p;
                break;
            }
        }
    }

    void UseItem()
    {
        Item used = heldItem;

        if (itemUseSound != null)
        {
            itemUseSound.Play();
        }

        heldItem?.Activate(this);

        if (heldItem == used)
        {
            //SetHeldItem(null);
        }
    }

    public IEnumerator SlowDown(float multiplier, float duration)
    {
        if (isSlowing) yield break;

        isSlowing = true;
        float originalSpeed = Speed;
        Speed *= multiplier;

        yield return new WaitForSeconds(duration);

        Speed = originalSpeed;
        isSlowing = false;
    }


    //鍵関係
    public void AddKey()
    {
        keyCount = Mathf.Clamp(keyCount + 1, 0, keySprites.Length - 1);
        UpdateKeyUI();
    }

    public void RemoveKey()
    {
        keyCount = Mathf.Max(0, keyCount - 1);
        UpdateKeyUI();
    }

    public void UpdateKeyUI()
    {
        if (keyIconImage != null && keySprites != null)
        {
            if (keyCount == 0)
            {
                keyIconImage.enabled = false;  // 鍵なしなら非表示
            }
            else if (keyCount > 0 && keyCount < keySprites.Length)
            {
                keyIconImage.enabled = true;
                keyIconImage.sprite = keySprites[keyCount];
            }
        }

        if (keyCountText != null)
        {
            keyCountText.text = keyCount.ToString();
        }
    }




    public void SetHeldItem(Item item)
    {
        heldItem = item;

        if (itemIconUI != null)
        {
            if (item != null && item.icon != null)
            {
                itemIconUI.sprite = item.icon;
                itemIconUI.color = new Color(1, 1, 1, 1); // 不透明に
                itemIconUI.enabled = true;               // 表示ON！
            }
            else
            {
                itemIconUI.enabled = false;              // 表示OFF！（白いのも消える）
            }
        }

        // 🔻 宝箱背景は「常にON」に固定
        if (itemBackgroundUI != null && !itemBackgroundUI.enabled)
        {
            itemBackgroundUI.enabled = true;
        }
    }



    

    public bool HasEnoughMagatama(int required)
    {
        return magatamaCount >= required;
    }

    /*public void AddMagatama()
    {
        magatamaCount = Mathf.Clamp(magatamaCount + 1, 0, 3);

        if (magatamaUIManager != null)
        {
            magatamaUIManager.UpdateMagatamaUI(magatamaCount);
        }
    }

    public void ResetMagatama()
    {
        magatamaCount = 0;

        if (magatamaUIManager != null)
        {
            magatamaUIManager.UpdateMagatamaUI(magatamaCount);
        }
    }*/
}
