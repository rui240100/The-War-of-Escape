using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
//using System.Diagnostics;

public class DemonCamera : MonoBehaviour
{
    private DemonAI demon;
    private List<Transform> playersInRange = new List<Transform>();
    public GameObject eyeposition;
    private float nextActionTime = 0f;
    public float interval = 1f; // 1秒ごと

    public float detectionRadius = 10f; // 半径範囲内のプレイヤーを検知

    Vector3 origin; //レイの発射位置

    void Start()
    {
        demon = GetComponentInParent<DemonAI>();
    }

    private void Update()
    {
        origin = eyeposition.transform.position;
    }

    void OnTriggerStay(Collider other)
    {
        if (demon.isChasing)
            return; // すでに追跡中なら何もしない

        if (other.CompareTag("Player"))
        {
            Debug.Log("視界内または範囲内：" + other.name);

            // プレイヤーとの距離をチェック（半径範囲内にいるか）
            if (Vector3.Distance(origin, other.transform.position) <= detectionRadius)
            {
                Debug.Log("範囲内のプレイヤー発見！");

                // 視界内でもあるか確認（レイキャスト）
                if (CanSeePlayer(other.transform))
                {
                    // 視界内であれば追跡を開始
                    demon.StartChase(other.transform);
                }
                else
                {
                    // 視界内でなくても範囲内であれば追跡開始（ただしレイキャストで何も障害物がない）
                    if (IsClearPathToPlayer(other.transform))
                    {
                        demon.StartChase(other.transform);
                    }
                }
            }
        }

        nextActionTime = Time.time + interval; // 次のアクションまでの時間を設定
    }

    // プレイヤーがトリガー範囲から出た場合
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            demon.StopChase(); // プレイヤーが範囲外に出たので追跡を停止
        }
    }

    // プレイヤーが視界内にいるかどうかを確認
    private bool CanSeePlayer(Transform target)
    {
        Vector3 targetPos = target.position + Vector3.up * 0.5f; // プレイヤーの位置を少し上げて確認
        Vector3 dir = targetPos - origin;

        Debug.DrawRay(origin, dir, Color.red, 0.1f); // レイを可視化
        Ray ray = new Ray(origin, dir.normalized);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, dir.magnitude)) // プレイヤーの方にレイを飛ばす
        {
            Debug.Log("Hit object: " + hit.collider.name);
            return hit.collider.CompareTag("Player"); // プレイヤーにヒットしたら視界内
        }

        return false;
    }

    // プレイヤーとデーモンの間に障害物がないかをチェック
    private bool IsClearPathToPlayer(Transform target)
    {
        Vector3 dir = target.position - origin; // デーモンの目からプレイヤーまでのベクトル

        Ray ray = new Ray(origin, dir.normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, dir.magnitude))
        {
            if (hit.collider.CompareTag("Player"))
            {
                // プレイヤーにヒットした場合は障害物がない
                return true;
            }
            else
            {
                // 何か他のオブジェクトにヒットした場合は障害物がある
                Debug.Log("障害物があるため、追跡できません: " + hit.collider.name);
                return false;
            }
        }

        return true; // レイが何もヒットしない場合は障害物なし
    }
}
