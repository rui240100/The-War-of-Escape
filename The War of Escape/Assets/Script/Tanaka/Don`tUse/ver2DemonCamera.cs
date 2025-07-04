using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ver2DemonCamera : MonoBehaviour
{
    private ver2DemonAI demon;
    private List<Transform> playersInRange = new List<Transform>();

    [SerializeField] private float viewDistance = 15f;     // 視界の最大距離
    [SerializeField] private float viewAngle = 120f;       // 視野角（度数）

    void Start()
    {
        demon = GetComponentInParent<ver2DemonAI>();
        StartCoroutine(CheckVisionRoutine()); // 一定間隔で視線チェック
    }

    private IEnumerator CheckVisionRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f); // 判定間隔を0.5秒に設定（調整可能）

        while (true)
        {
            yield return wait;

            CleanUpPlayerList(); // 無効なプレイヤーを削除

            Transform closestVisiblePlayer = null;
            float closestDistance = Mathf.Infinity;

            foreach (Transform player in playersInRange)
            {
                if (player == null) continue;

                if (CanSeePlayer(player))
                {
                    float distance = Vector3.Distance(transform.position, player.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestVisiblePlayer = player;
                    }
                }
            }

            // ターゲットが変更された場合のみStartChaseを呼び出す
            if (closestVisiblePlayer != null)
            {
                if (demon.CurrentTarget != closestVisiblePlayer)  // 現在のターゲットと違う場合のみ
                {
                    demon.StartChase(closestVisiblePlayer);
                }
            }
            else if (demon.CurrentTarget != null)
            {
                demon.StopChase();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playersInRange.Contains(other.transform))
        {
            playersInRange.Add(other.transform);

            // 新しいプレイヤーが追加された場合のみ追跡開始
            if (demon.CurrentTarget == null || demon.CurrentTarget != other.transform)
            {
                demon.StartChase(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInRange.Remove(other.transform);

            // ターゲットが退出した場合、追跡を停止
            if (demon.CurrentTarget == other.transform)
            {
                demon.StopChase();
            }
        }
    }

    private void CleanUpPlayerList()
    {
        // nullになったTransformを削除（例：消滅・死亡など）
        playersInRange.RemoveAll(player => player == null);
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

        // 視線に遮蔽物がないかRaycastで確認
        Vector3 origin = transform.position + Vector3.up * 1.0f;
        Ray ray = new Ray(origin, dirToTarget.normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            return hit.collider.CompareTag("Player");
        }

        return false;
    }
}
