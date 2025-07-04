using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ver3DemonCamera : MonoBehaviour
{
    private ver2DemonAI demon;

    [SerializeField] private float viewDistance = 15f;     // 視界の最大距離
    [SerializeField] private float viewAngle = 120f;       // 視野角（度数）

    void Start()
    {
        demon = GetComponentInParent<ver2DemonAI>();
        StartCoroutine(CheckVisionRoutine()); // 一定間隔で視線チェック
    }

    private IEnumerator CheckVisionRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f); // 判定間隔を0.5秒に設定

        while (true)
        {
            yield return wait;

            Transform closestVisiblePlayer = null;
            float closestDistance = Mathf.Infinity;

            // 視界距離内の全コライダーを取得
            Collider[] colliders = Physics.OverlapSphere(transform.position, viewDistance);

            foreach (Collider collider in colliders)
            {
                // タグが「Player1」または「Player2」のオブジェクトのみ対象とする
                if (collider.CompareTag("Player1") || collider.CompareTag("Player2"))
                {
                    Transform player = collider.transform;

                    if (CanSeePlayer(player))
                    {
                        // すでに追跡中のターゲットがいる場合、異なるタグのプレイヤーは無視する
                        if (demon.CurrentTarget != null && player.tag != demon.CurrentTarget.tag)
                        {
                            continue;
                        }

                        // 一番近いプレイヤーを記録
                        float distance = Vector3.Distance(transform.position, player.position);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestVisiblePlayer = player;
                        }
                    }
                }
            }

            // 見えるプレイヤーがいる場合のみ追跡開始
            if (closestVisiblePlayer != null)
            {
                // 現在のターゲットが null または異なるプレイヤーの場合のみ StartChase を呼ぶ
                if (demon.CurrentTarget == null)
                {
                    demon.StartChase(closestVisiblePlayer);
                }
                else if (demon.CurrentTarget != closestVisiblePlayer)
                {
                    // タグが同じなら追跡継続（StartChase 呼ばない）
                    if (closestVisiblePlayer.tag != demon.CurrentTarget.tag)
                    {
                        demon.StartChase(closestVisiblePlayer);
                    }
                }
            }
            else
            {
                // 視界内に誰もいなければ追跡停止
                if (demon.CurrentTarget != null)
                {
                    demon.StopChase();
                }
            }
        }
    }

    private bool CanSeePlayer(Transform target)
    {
        Vector3 dirToTarget = target.position - transform.position;
        float distance = dirToTarget.magnitude;

        // 距離チェック
        if (distance > viewDistance) return false;

        // 視野角チェック（Demonの前方向とターゲット方向との角度）
        float angle = Vector3.Angle(transform.forward, dirToTarget);
        if (angle > viewAngle * 0.5f) return false;

        // 視線に遮蔽物がないか Raycast で確認
        Vector3 origin = transform.position + Vector3.up * 1.0f;  // 少し上から照射
        Ray ray = new Ray(origin, dirToTarget.normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            // Ray が最初に当たったのがプレイヤーなら可視
            return hit.collider.CompareTag("Player1") || hit.collider.CompareTag("Player2");
        }

        return false;
    }
}
