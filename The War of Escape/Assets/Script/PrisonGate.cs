using UnityEngine;

public class PrisonGate : MonoBehaviour
{
    public float holdTime ;
    public int requiredMagatamaCount = 3;

    private bool isOpening = false;
    private float holdTimer = 0f;
    private Player playerInFront;


    [SerializeField]
    GameObject doorPart;  // Inspector�Ŋč��̃h�A�����i�L���[�u�j���A�T�C��

    private void Update()
    {
        if (playerInFront != null && !isOpening)
        {
            bool holdingButton = false;

            if (playerInFront.playerID == 1)
                holdingButton = Input.GetButton("Fire2"); // B�{�^��
            else if (playerInFront.playerID == 2)
                holdingButton = Input.GetButton("Fire2_2"); // 2P�p�̃{�^���i����΁j

            if (holdingButton)
            {
                if (playerInFront.HasEnoughMagatama(requiredMagatamaCount))
                {
                    holdTimer += Time.deltaTime;
                    if (holdTimer >= holdTime)
                    {
                    �@�@Debug.Log("�J���S�}");
                        OpenPrison();
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

    private void OpenPrison()
    {
        isOpening = true;
        Debug.Log("�č����J�����I");
        // �A�j���[�V������h�A�J��������������

        if (doorPart != null)
        {
            Destroy(doorPart);
        }
        else
        {
            Debug.LogWarning("doorPart���Z�b�g����Ă��܂���I");
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
}
