using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public GameObject player1, player2;


    GameObject p1Point, p2Point;

    // 3Dワールドの範囲
    private float worldXMin = -12f;
    private float worldXMax = 38f;
    private float worldZMin = -4.5f;
    private float worldZMax = 51.5f;

    // 2Dミニマップの範囲
    private float mapXMin = -155f;
    private float mapXMax = 155f;
    private float mapYMin = -170f;
    private float mapYMax = 170f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        p1Point = transform.GetChild(0).gameObject;
        p2Point = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        p1Point.GetComponent<RectTransform>().anchoredPosition = WorldToMapPosition(player1.transform.position);
        p2Point.GetComponent<RectTransform>().anchoredPosition = WorldToMapPosition(player2.transform.position);
    }

    /// <summary>
    /// 3Dのワールド座標を2Dミニマップ座標に変換する
    /// </summary>
    /// <param name="worldPos">ワールド空間の位置</param>
    /// <returns>ミニマップ上の2D位置（Vector2）</returns>
    public Vector2 WorldToMapPosition(Vector3 worldPos)
    {
        float mapX = Remap(worldPos.x, worldXMin, worldXMax, mapXMin, mapXMax);
        float mapY = Remap(worldPos.z, worldZMin, worldZMax, mapYMin, mapYMax);

        return new Vector2(mapX, mapY);
    }

    /// <summary>
    /// 値を別の範囲に線形補間して変換
    /// </summary>
    private float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }
}
