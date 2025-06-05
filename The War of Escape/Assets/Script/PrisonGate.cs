using UnityEngine;

public class PrisonGate : MonoBehaviour
{
    public float holdTime;
    public int requiredMagatamaCount = 3;

    private bool isOpening = false;
    private float holdTimer = 0f;
    private Player playerInFront;

    [SerializeField] GameObject doorPart;

    [Header("監獄に入ってる鬼（Inspectorで指定）")]
    public ProtectingDemon imprisonedOni;  // ← ここを Inspector で指定する！

    private Player openedByPlayer;

    private void Update()
    {
        if (playerInFront != null && !isOpening)
        {
            bool holdingButton = false;

            if (playerInFront.playerID == 1)
                holdingButton = Input.GetButton("Fire2");
            else if (playerInFront.playerID == 2)
                holdingButton = Input.GetButton("Fire2_2");

            if (holdingButton)
            {
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
        openedByPlayer = player;

        Debug.Log($"監獄がプレイヤー{player.playerID}によって開かれました！");

        if (doorPart != null)
        {
            Destroy(doorPart);
        }

        // 鬼に知らせる
        if (imprisonedOni != null)
        {
            imprisonedOni.SetOwner(player);
        }
    }

    public Player GetOpenedByPlayer()
    {
        return openedByPlayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            playerInFront = player;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && player == playerInFront)
        {
            playerInFront = null;
            holdTimer = 0f;
        }
    }
}
