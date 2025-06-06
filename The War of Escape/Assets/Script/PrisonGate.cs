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

    private void OpenPrison(Player player)
    {
        if (isOpening) return;

        isOpening = true;
        playerInFront = player;

        playerID = true;


        if (player.playerID == 1)
        {
            
            Debug.Log("【1P】監獄がプレイヤー1によって開かれました！");
        }
        else if (player.playerID == 2)
        {
            Debug.Log("【2P】監獄がプレイヤー2によって開かれました！");
        }


        /*Debug.Log($"監獄がプレイヤー{player.playerID}によって開かれました！");

        protectingDemonScript = protectingDemon.GetComponent<ProtectingDemon>();
        */


        if (doorPart != null)
        {
            Destroy(doorPart);
        }

        // 鬼に知らせる
        if (protectingDemonScript != null)
        {
            //protectingDemonScript.SetOwner(player);  // ← 所有者を教える
            protectingDemonScript.Launch();
            Debug.Log("Launch呼ばれた");
            //protectingDemonScript.SetOwner(player);
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
