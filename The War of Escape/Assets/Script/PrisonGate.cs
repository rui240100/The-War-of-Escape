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
                //Debug.Log("2P�Ă΂ꂽ");
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

    /*private void OpenPrison(Player player)
    {
        if (isOpening) return;

        isOpening = true;
        playerInFront = player;

        playerID = true;


        if (protectingDemon != null)
        {
            protectingDemonScript = protectingDemon.GetComponent<ProtectingDemon>();
            protectingDemonScript.SetOwner(player); // �� �������d�v�I
        }



        // �����ǉ��I�I�I�I�I
        if (protectingDemonScript == null && protectingDemon != null)
        {
            protectingDemonScript = protectingDemon.GetComponent<ProtectingDemon>();
        }

        if (player.playerID == 1)
        {
            protectingDemonScript.SetOwner(player); // �������I�I
            protectingDemonScript.Launch();
            Debug.Log("Launch�Ă΂ꂽ");
            Debug.Log("�y1P�z�č����v���C���[1�ɂ���ĊJ����܂����I");
        }
        else if (player.playerID == 2)
        {
            protectingDemonScript.SetOwner(player);
            protectingDemonScript.Launch();
            Debug.Log("Launch�Ă΂ꂽ");
            Debug.Log("�y2P�z�č����v���C���[2�ɂ���ĊJ����܂����I");
        }


        //Debug.Log($"�č����v���C���[{player.playerID}�ɂ���ĊJ����܂����I");

        //protectingDemonScript = protectingDemon.GetComponent<ProtectingDemon>();
        


        if (doorPart != null)
        {
            Destroy(doorPart);
        }

        // �S�ɒm�点��
        if (protectingDemonScript != null)
        {
            //protectingDemonScript.SetOwner(player);  // �� ���L�҂�������
            
            Debug.Log("Launch�Ă΂ꂽ");
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

                Debug.Log($"Launch�Ă΂ꂽ �y{player.playerID}P�z�č����v���C���[{player.playerID}�ɂ���ĊJ����܂����I");
            }
            else
            {
                Debug.LogError("protectingDemonScript �� null �ł��BProtectingDemon �R���|�[�l���g������܂���B");
            }
        }
        else
        {
            Debug.LogError("protectingDemon �� Inspector �ŃZ�b�g����Ă��܂���I");
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

                Debug.Log($"Launch�Ă΂ꂽ �y{player.playerID}P�z�č����v���C���[{player.playerID}�ɂ���ĊJ����܂����I");
            }
            else
            {
                Debug.LogError("protectingDemonScript �� null �ł��BProtectingDemon �R���|�[�l���g������܂���B");
            }
        }
        else
        {
            Debug.LogError("protectingDemon �� Inspector �ŃZ�b�g����Ă��܂���I");
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
