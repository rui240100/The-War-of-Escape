using UnityEngine;

public class PrisonGate : MonoBehaviour
{
    public float holdTime;
    public int requiredMagatamaCount = 0;

    private bool isOpening = false;
    private float holdTimer = 0f;
    private Player playerInFront;

    [SerializeField] GameObject doorPart;

    [Header("�č��ɓ����Ă�S�iInspector�Ŏw��j")]
    public GameObject protectingDemon;  // �� ������ Inspector �Ŏw�肷��I
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
                //playerID = true;

            }
            else if (playerInFront.playerID == 2)
            {
                holdingButton = Input.GetButton("Fire2_2");
               // playerID = false;

            }

            if (holdingButton)
            {

                /*Debug.Log("�v���C���[�̌��ʐ�: " + playerInFront.magatamaCount);
                Debug.Log("�K�v�Ȍ��ʐ�: " + requiredMagatamaCount);*/

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
                    Debug.Log("���ʂ�����܂���I");
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

        Debug.Log($"�č����v���C���[{player.playerID}�ɂ���ĊJ����܂����I");

        protectingDemonScript = protectingDemon.GetComponent<ProtectingDemon>();
        


        if (doorPart != null)
        {
            Destroy(doorPart);
        }

        // �S�ɒm�点��
        if (protectingDemonScript != null)
        {
            //protectingDemonScript.SetOwner(player);  // �� ���L�҂�������
            protectingDemonScript.Launch();

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
