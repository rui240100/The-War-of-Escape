using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ver2DemonCamera : MonoBehaviour
{
    private ver2DemonAI demon;
    private List<Transform> playersInRange = new List<Transform>();

    [SerializeField] private float viewDistance = 15f;     // ���E�̍ő勗��
    [SerializeField] private float viewAngle = 120f;       // ����p�i�x���j

    void Start()
    {
        demon = GetComponentInParent<ver2DemonAI>();
        StartCoroutine(CheckVisionRoutine()); // ���Ԋu�Ŏ����`�F�b�N
    }

    private IEnumerator CheckVisionRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f); // ����Ԋu��0.5�b�ɐݒ�i�����\�j

        while (true)
        {
            yield return wait;

            CleanUpPlayerList(); // �����ȃv���C���[���폜

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

            // �^�[�Q�b�g���ύX���ꂽ�ꍇ�̂�StartChase���Ăяo��
            if (closestVisiblePlayer != null)
            {
                if (demon.CurrentTarget != closestVisiblePlayer)  // ���݂̃^�[�Q�b�g�ƈႤ�ꍇ�̂�
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

            // �V�����v���C���[���ǉ����ꂽ�ꍇ�̂ݒǐՊJ�n
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

            // �^�[�Q�b�g���ޏo�����ꍇ�A�ǐՂ��~
            if (demon.CurrentTarget == other.transform)
            {
                demon.StopChase();
            }
        }
    }

    private void CleanUpPlayerList()
    {
        // null�ɂȂ���Transform���폜�i��F���ŁE���S�Ȃǁj
        playersInRange.RemoveAll(player => player == null);
    }

    private bool CanSeePlayer(Transform target)
    {
        Vector3 dirToTarget = target.position - transform.position;
        float distance = dirToTarget.magnitude;

        // �����`�F�b�N
        if (distance > viewDistance) return false;

        // ����p�`�F�b�N�iDemon�̑O�����ƃ^�[�Q�b�g�����Ƃ̊p�x�j
        float angle = Vector3.Angle(transform.forward, dirToTarget);
        if (angle > viewAngle * 0.5f) return false;

        // �����ɎՕ������Ȃ���Raycast�Ŋm�F
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
