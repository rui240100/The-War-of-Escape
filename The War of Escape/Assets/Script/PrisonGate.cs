using UnityEngine;

public class PrisonGate : MonoBehaviour
{
    public float holdTime;
    public int requiredMagatamaCount = 3;

    private bool isOpening = false;
    private float holdTimer = 0f;
    private Player playerInFront;

    [SerializeField]
    private GameObject doorPart;  // Inspectorで監獄のドア部分（キューブ）をアサイン

    private Player openedByPlayer; // 誰が開けたかを記録する変数

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
        isOpening = true;
        openedByPlayer = player;
        Debug.Log($"監獄が開いた！ 開けたのは Player{player.playerID}");

        if (doorPart != null)
        {
            Destroy(doorPart);
        }
        else
        {
            Debug.LogWarning("doorPartがセットされていません！");
        }
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

    public Player GetOpenedByPlayer()
    {
        return openedByPlayer;
    }
}
