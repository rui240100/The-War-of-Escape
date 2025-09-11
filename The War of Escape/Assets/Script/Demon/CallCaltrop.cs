using UnityEngine;

public class CallCaltrop : Item
{
    [Header("Caltrop Settings")]
    [SerializeField] private GameObject caltrop;

    private Player playerScriptCo;

    private string useMessageCa1 = "まきびしを設置しました";
    private string useMessageCa2 = "";
    private GameObject itemUseCo;
    private ItemUse itemUseScCo;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip useSound;           // 使用時に鳴らす音
    [SerializeField] private AudioSource audioSource;      // AudioSourceをInspectorでセット

    private void Start()
    {
        // シーン内のMessageDisplayManagerを探す
        itemUseCo = GameObject.Find("ItemUse");
        itemUseScCo = itemUseCo.GetComponent<ItemUse>();
        if (itemUseScCo == null)
        {
            Debug.Log("MessageDisplayManager が見つかりません");
        }
        else
        {
            Debug.Log("MessageDisplayManager を見つけました");
        }
    }

    public override void Activate(Player user)
    {
        // 🎵 使用音を再生
        if (audioSource != null && useSound != null)
        {
            audioSource.PlayOneShot(useSound);
        }

        Debug.Log("まきびし設置0");
        GameObject caltropObj = Instantiate(caltrop);
        caltropObj.transform.position = new Vector3(
            user.transform.position.x,
            user.transform.position.y - 1.1f,
            user.transform.position.z
        );
        Debug.Log("まきびし設置1");

        playerScriptCo = user.GetComponent<Player>();

        if (itemUseScCo != null)
        {
            if (playerScriptCo.playerID == 1)
            {
                itemUseScCo.ShowMessage(useMessageCa1, useMessageCa2);
            }
            else if (playerScriptCo.playerID == 2)
            {
                itemUseScCo.ShowMessage(useMessageCa2, useMessageCa1);
            }
            Debug.Log("メッセージ渡した");
        }
        else
        {
            Debug.Log("メッセージ渡せなかった");
        }

        user.SetHeldItem(null);
    }
}
