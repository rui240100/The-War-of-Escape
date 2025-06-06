using UnityEngine;

public class PrisonGate : MonoBehaviour
{
    public float holdTime;
    public int requiredMagatamaCount = 0;

    private bool isOpening = false;
    private float holdTimer = 0f;
    private Player playerInFront;

    [SerializeField] GameObject doorPart;

    [Header("監獄に入ってる鬼（Inspectorで指定）")]
    public GameObject protectingDemon;  // ← ここを Inspector で指定する！
    private ProtectingDemon protectingDemonScript;

    public bool playerID;

    //private Player openedByPlayer;

    private void Update()
    {

        //Debug.Log($"playerInFront: {playerInFront}, isOpening: {isOpening}");


        if (playerInFront != null && !isOpening)
        {
            bool holdingButton = false;

            if (playerInFront.playerID == 1)
            {
                holdingButton = Input.GetButton("Fire2");
                
                
            }
            else if (playerInFront.playerID == 2)
            {
                holdingButton = Input.GetButton("Fire2_2");
               playerID = false;
                //Debug.Log("2P呼ばれた");
            }

            if (holdingButton)
            {

                /*Debug.Log("プレイヤーの勾玉数: " + playerInFront.magatamaCount);
                Debug.Log("必要な勾玉数: " + requiredMagatamaCount);*/

                if (playerInFront.HasEnoughMagatama(requiredMagatamaCount))
                {
                    holdTimer += Time.deltaTime;
                    if (holdTimer >= holdTime)
                    {
                        OpenPrison(playerInFront);
                    }
                }
                else
                {
                    Debug.Log("勾玉が足りません！");
                    holdTimer = 0f;
                }
            }
            else
            {
                holdTimer = 0f;
            }
        }
    }

    /*private void OpenPrison(Player player)
    {
        if (isOpening) return;

        isOpening = true;
        playerInFront = player;

        playerID = true;


        if (protectingDemon != null)
        {
            protectingDemonScript = protectingDemon.GetComponent<ProtectingDemon>();
            protectingDemonScript.SetOwner(player); // ← ここが重要！
        }



        // ここ追加！！！！！
        if (protectingDemonScript == null && protectingDemon != null)
        {
            protectingDemonScript = protectingDemon.GetComponent<ProtectingDemon>();
        }

        if (player.playerID == 1)
        {
            protectingDemonScript.SetOwner(player); // ←ここ！！
            protectingDemonScript.Launch();
            Debug.Log("Launch呼ばれた");
            Debug.Log("【1P】監獄がプレイヤー1によって開かれました！");
        }
        else if (player.playerID == 2)
        {
            protectingDemonScript.SetOwner(player);
            protectingDemonScript.Launch();
            Debug.Log("Launch呼ばれた");
            Debug.Log("【2P】監獄がプレイヤー2によって開かれました！");
        }


        //Debug.Log($"監獄がプレイヤー{player.playerID}によって開かれました！");

        //protectingDemonScript = protectingDemon.GetComponent<ProtectingDemon>();
        


        if (doorPart != null)
        {
            Destroy(doorPart);
        }

        // 鬼に知らせる
        if (protectingDemonScript != null)
        {
            //protectingDemonScript.SetOwner(player);  // ← 所有者を教える
            
            Debug.Log("Launch呼ばれた");
            //protectingDemonScript.SetOwner(player);
        }
    }*/

    private void OpenPrison(Player player)
    {
        if (isOpening) return;

        isOpening = true;
        playerInFront = player;
        playerID = true;

        if (protectingDemon != null)
        {
            protectingDemonScript = protectingDemon.GetComponent<ProtectingDemon>();

            if (protectingDemonScript != null)
            {
                protectingDemonScript.SetOwner(player);
                protectingDemonScript.Launch();

                Debug.Log($"Launch呼ばれた 【{player.playerID}P】監獄がプレイヤー{player.playerID}によって開かれました！");
            }
            else
            {
                Debug.LogError("protectingDemonScript が null です。ProtectingDemon コンポーネントがありません。");
            }
        }
        else
        {
            Debug.LogError("protectingDemon が Inspector でセットされていません！");
        }

        if (doorPart != null)
        {
            Destroy(doorPart);
        }

        if (protectingDemon != null)
        {
            protectingDemonScript = protectingDemon.GetComponent<ProtectingDemon>();

            if (protectingDemonScript != null)
            {
                protectingDemonScript.SetOwner(player);
                protectingDemonScript.Launch();

                Debug.Log($"Launch呼ばれた 【{player.playerID}P】監獄がプレイヤー{player.playerID}によって開かれました！");
            }
            else
            {
                Debug.LogError("protectingDemonScript が null です。ProtectingDemon コンポーネントがありません。");
            }
        }
        else
        {
            Debug.LogError("protectingDemon が Inspector でセットされていません！");
        }

    }


    public Player GetOpenedByPlayer()
    {
        return playerInFront;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            playerInFront = player;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && player == playerInFront)
        {
            playerInFront = null;
            holdTimer = 0f;
        }
    }
}
