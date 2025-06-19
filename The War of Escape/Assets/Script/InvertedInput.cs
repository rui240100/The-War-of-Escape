using UnityEngine;

public class InvertedInput : MonoBehaviour
{
    private bool isInverted = false;
    private Player player;

    void Awake()
    {
        player = GetComponent<Player>();
    }

    public void EnableInversion()
    {
        isInverted = true;
    }

    public void DisableInversion()
    {
        isInverted = false;
    }

    public float GetAxisRaw(string axisName)
    {
        float value = Input.GetAxisRaw(axisName);
        return isInverted ? -value : value;
    }
}
