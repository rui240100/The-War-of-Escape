using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;  // UI関連を使うので追加


public class Player : MonoBehaviour
{
    public int playerID;
    public float Speed;
    public Transform cameraTransform; // カメラのTransformを入れるための変数
    public float lookSensitivity = 3.0f;
    CharacterController characterController;
    //public bool HasItem = false;
    public Player otherPlayer;

    bool isSlowing = false;
    public int keyCount = 0; // 所持している鍵の数
    public Item heldItem; // 拾ったアイテムの参照
    public bool HasItem => heldItem != null;

    public Image itemIconUI; //UIのImageを入れる
    public Sprite defaultItemIcon;// プレハブに持たせるアイコン画像

    public Image keyIconUI; // 鍵アイコンを表示するImage（プレイヤーごとに設定）
    public Sprite keyIconSprite; // 鍵を持ったときに表示するアイコン
    public Sprite defaultKeyIcon; // 鍵がないときのデフォルトアイコン
    public Sprite[] keyIcons; // 鍵の所持数に応じたアイコンを入れる（0〜5）

    public Text keyCountText;       // 所持数表示用のテキスト

    public int magatamaCount = 0; // 勾玉の所持数

    public Image[] magatamaIcons;  // Inspectorで複数の勾玉アイコンをアサイン（例：5個）

    public GameObject pd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        characterController = GetComponent<CharacterController>();

        StartCoroutine(FindOtherPlayerWithDelay());


        //ゲーム開始時に何も持ってないときのアイテムUIを表示する
        if (itemIconUI != null && defaultItemIcon != null)
        {
            itemIconUI.sprite = defaultItemIcon;
            itemIconUI.enabled = true; 
        }



        Application.targetFrameRate = 60;


        UpdateMagatamaUI();

        UpdateKeyUI();
    }

    // Update is called once per frame
    void Update()
    {
        float x = 0f;
        float z = 0f;
        float mouseX = 0f;
        float mouseY = 0f;



        if (playerID == 1)
        {
            x = Input.GetAxisRaw("Horizontal1"); // 左右入力
            z = Input.GetAxisRaw("Vertical1");   // 前後入力
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }
        else if (playerID == 2)
        {
            x = Input.GetAxisRaw("Horizontal2");
            z = Input.GetAxisRaw("Vertical2");
            mouseX = Input.GetAxis("Mouse X2");
            mouseY = Input.GetAxis("Mouse Y2");

        }

        // Debug.Log(x + "," + z);


        // カメラの前方向・右方向を取得
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // 上下成分を消す（水平移動だけにする）
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // 入力と方向ベクトルを合成
        Vector3 move = (forward * z + right * x).normalized;


        //  真上・真下を向いているとき対策 
        if (forward == Vector3.zero)
        {
            // カメラが真上か真下を向いてるなら、プレイヤーのローカル軸で動かす
            move = transform.right * x + transform.forward * z;
            move = move.normalized;
        }

        characterController.SimpleMove(move * Speed);



        if (playerID == 1 && Input.GetButtonDown("Fire3") && HasItem) // Fire3は1PのRT
        {
            UseItem();

            //Debug.Log("動くな");
        }
        else if (playerID == 2 && Input.GetButtonDown("Fire3_2") && HasItem)
        {
            UseItem();

            //Debug.Log("動くな");
        }




    }




    IEnumerator FindOtherPlayerWithDelay()
    {
        yield return new WaitForSeconds(0.1f); // 少し待つ（0.1秒）

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
        heldItem?.Activate(this); // 自分を渡してアイテムを起動
        SetHeldItem(null);        // ← これでアイテムを消費して、UIもリセット！
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

    public void AddKey()
    {
        keyCount = Mathf.Clamp(keyCount + 1, 0, 5);

        if (keyIconUI != null && keyIcons != null && keyIcons.Length > keyCount)
        {
            keyIconUI.sprite = keyIcons[keyCount];
        }

        if (keyCountText != null)
        {
            keyCountText.text = keyCount.ToString();
        }
    }




    

    public void RemoveKey()
    {
        keyCount = Mathf.Max(0, keyCount - 1);

        if (keyIconUI != null && keyIcons != null && keyIcons.Length > keyCount)
        {
            keyIconUI.sprite = keyIcons[keyCount];
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
            }
            else
            {
                itemIconUI.sprite = defaultItemIcon;
            }

            itemIconUI.enabled = true;
        }
    }

    //鍵の処理
    void UpdateKeyUI()
    {
        if (keyIconUI != null && keyIcons != null && keyIcons.Length > keyCount)
        {
            keyIconUI.sprite = keyIcons[keyCount];
        }

        if (keyCountText != null)
        {
            keyCountText.text = keyCount.ToString();
        }
    }

    //勾玉の処理
   

    public bool HasEnoughMagatama(int required)
    {
        return magatamaCount >= required;
    }



    public void UpdateMagatamaUI()
    {
        for (int i = 0; i < magatamaIcons.Length; i++)
        {
            if (magatamaIcons[i] != null)
            {
                // 勾玉数より小さい数のアイコンは表示、大きい数は非表示
                magatamaIcons[i].enabled = (i < magatamaCount);
            }
        }
    }

    public void AddMagatama()
    {
        magatamaCount = Mathf.Clamp(magatamaCount + 1, 0, magatamaIcons.Length);
        Debug.Log($"AddMagatama() 呼ばれた。現在の勾玉数: {magatamaCount}");
        UpdateMagatamaUI();
    }








}