using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ver3DemonCamera : MonoBehaviour
{
    private ver2DemonAI demon;

    [SerializeField] private float viewDistance = 15f;     // ���E�̍ő勗��
    [SerializeField] private float viewAngle = 120f;       // ����p�i�x���j

    void Start()
    {
        demon = GetComponentInParent<ver2DemonAI>();
        StartCoroutine(CheckVisionRoutine()); // ���Ԋu�Ŏ����`�F�b�N
    }

    private IEnumerator CheckVisionRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f); // ����Ԋu��0.5�b�ɐݒ�

        while (true)
        {
            yield return wait;

            Transform closestVisiblePlayer = null;
            float closestDistance = Mathf.Infinity;

            // ���E�������̑S�R���C�_�[���擾
            Collider[] colliders = Physics.OverlapSphere(transform.position, viewDistance);

            foreach (Collider collider in colliders)
            {
                // �^�O���uPlayer1�v�܂��́uPlayer2�v�̃I�u�W�F�N�g�̂ݑΏۂƂ���
                if (collider.CompareTag("Player1") || collider.CompareTag("Player2"))
                {
                    Transform player = collider.transform;

                    if (CanSeePlayer(player))
                    {
                        // ���łɒǐՒ��̃^�[�Q�b�g������ꍇ�A�قȂ�^�O�̃v���C���[�͖�������
                        if (demon.CurrentTarget != null && player.tag != demon.CurrentTarget.tag)
                        {
                            continue;
                        }

                        // ��ԋ߂��v���C���[���L�^
                        float distance = Vector3.Distance(transform.position, player.position);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestVisiblePlayer = player;
                        }
                    }
                }
            }

            // ������v���C���[������ꍇ�̂ݒǐՊJ�n
            if (closestVisiblePlayer != null)
            {
                // ���݂̃^�[�Q�b�g�� null �܂��͈قȂ�v���C���[�̏ꍇ�̂� StartChase ���Ă�
                if (demon.CurrentTarget == null)
                {
                    demon.StartChase(closestVisiblePlayer);
                }
                else if (demon.CurrentTarget != closestVisiblePlayer)
                {
                    // �^�O�������Ȃ�ǐՌp���iStartChase �Ă΂Ȃ��j
                    if (closestVisiblePlayer.tag != demon.CurrentTarget.tag)
                    {
                        demon.StartChase(closestVisiblePlayer);
                    }
                }
            }
            else
            {
                // ���E���ɒN�����Ȃ���ΒǐՒ�~
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

        // �����`�F�b�N
        if (distance > viewDistance) return false;

        // ����p�`�F�b�N�iDemon�̑O�����ƃ^�[�Q�b�g�����Ƃ̊p�x�j
        float angle = Vector3.Angle(transform.forward, dirToTarget);
        if (angle > viewAngle * 0.5f) return false;

        // �����ɎՕ������Ȃ��� Raycast �Ŋm�F
        Vector3 origin = transform.position + Vector3.up * 1.0f;  // �����ォ��Ǝ�
        Ray ray = new Ray(origin, dirToTarget.normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            // Ray ���ŏ��ɓ��������̂��v���C���[�Ȃ��
            return hit.collider.CompareTag("Player1") || hit.collider.CompareTag("Player2");
        }

        return false;
    }
}
