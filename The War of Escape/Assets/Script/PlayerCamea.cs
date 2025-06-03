using System.Linq;
using UnityEngine;

public class PlayerCamea : MonoBehaviour
{
    public Transform playerBody; // �v���C���[�{�̂�Transform
    public float sensitivity = 2.0f;

    float xRotation = 0f;

    public int playerID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float lookX = 0f;
        float lookY = 0f;

        if (playerID == 1)
        {
            lookX = Input.GetAxis("RightStickHorizontal");
            lookY = Input.GetAxis("RightStickVertical");

            
        }
        else if (playerID == 2)
        {
            lookX = Input.GetAxis("RightStickHorizontal2");
            lookY = Input.GetAxis("RightStickVertical2");

            
        }

        // ���������i�J�����㉺�j
        xRotation -= lookY * sensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // ���������i�v���C���[�{�̂����E�ɉ�]�j
        playerBody.Rotate(Vector3.up * lookX * sensitivity);
    }
}
