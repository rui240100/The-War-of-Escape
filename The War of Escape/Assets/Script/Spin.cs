using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f; // インスペクター上で調整可能

    void Update()
    {
        // Z軸を中心に回転
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
