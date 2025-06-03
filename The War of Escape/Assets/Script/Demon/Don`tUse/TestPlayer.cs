using UnityEngine;

public class MoveNegativeZ : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        //transform.position += Vector3.left * speed * Time.deltaTime;

        Destroy(this.gameObject,10);
    }
}
