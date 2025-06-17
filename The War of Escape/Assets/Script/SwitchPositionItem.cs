using UnityEngine;
using System.Collections;

public class SwitchPositionItem : Item
{
    private bool isUsed = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && !player.HasItem)
        {
            player.SetHeldItem(this);
            transform.SetParent(player.transform);
            transform.localPosition = Vector3.zero;
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public override void Activate(Player user)
    {
        if (isUsed || user.otherPlayer == null) return;

        isUsed = true;
        user.StartCoroutine(SwitchPositionsAfterDelay(user));
    }

    private IEnumerator SwitchPositionsAfterDelay(Player user)
    {
        yield return new WaitForSeconds(3f);

        Transform other = user.otherPlayer.transform;
        Vector3 tempPos = user.transform.position;
        user.transform.position = other.position;
        other.position = tempPos;

        Destroy(gameObject); // アイテムは使い捨て
    }


}
