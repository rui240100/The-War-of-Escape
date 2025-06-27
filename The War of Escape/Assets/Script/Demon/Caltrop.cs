using System.Collections;
using UnityEngine;

public class Caltrap : Item
{
    public GameObject caltrapPrefab; // プレハブ（Inspectorで設定）

    public override void Activate(Player user)
    {
        Vector3 spawnPosition = user.transform.position;
        Quaternion rotation = Quaternion.identity;

        // 生成してユーザーの親から切り離し
        GameObject trap = Instantiate(caltrapPrefab, spawnPosition, rotation);
        trap.transform.SetParent(null); // ワールドに置く

        Debug.Log("Caltrap: " + user.playerID + " がまきびしを設置しました");

        // アイテムは使い捨て
        Destroy(this.gameObject);
    }
}
