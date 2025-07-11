using System.Linq;
using UnityEngine;

public class PlayerCamea : MonoBehaviour
{
    public Transform playerBody; // プレイヤー本体のTransform
    public float sensitivity = 2.0f;
    public int playerID;

    [Header("体を非表示にする設定")]
    public SkinnedMeshRenderer bodyRenderer; // プレイヤーの体のRenderer
    public float hideDistance = 0.5f;         // 体を非表示にする距離

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

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

        // 垂直方向（カメラ上下）
        xRotation -= lookY * sensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 水平方向（プレイヤー本体を左右に回転）
        playerBody.Rotate(Vector3.up * lookX * sensitivity);

        // ───────── 体の表示制御 ─────────
        if (bodyRenderer != null)
        {
            float distance = Vector3.Distance(transform.position, bodyRenderer.transform.position);
            bodyRenderer.enabled = distance > hideDistance;
        }
    }
}
