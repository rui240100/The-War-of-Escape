using UnityEngine;

/// <summary>
/// DemonAIの状態に応じてアニメーションを制御するスクリプト
/// Animatorやパラメータ名をインスペクター上から調整可能
/// </summary>
[RequireComponent(typeof(Necromancer))]
public class NecAnima : MonoBehaviour
{
    [Header("アニメーター本体")]
    [SerializeField] private Animator animator;

    [Header("パラメータ名（Bool型）")]
    [SerializeField] private string patrolParam = "IsPatrolling";
    [SerializeField] private string chaseParam = "IsChasing";
    [SerializeField] private string stunParam = "IsStunned";

    private Necromancer necromancer;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        necromancer = GetComponent<Necromancer>();
    }

    private void Update()
    {
        if (necromancer.NecStun)
        {
            SetAnimationState(false, false, true);
        }
        else
        {
            SetAnimationState(false, true, false);
        }

    }

    /// <summary>
    /// 3つの状態を排他的に制御
    /// </summary>
    private void SetAnimationState(bool patrolling, bool chasing, bool stunned)
    {
        animator.SetBool(patrolParam, patrolling);
        animator.SetBool(chaseParam, chasing);
        animator.SetBool(stunParam, stunned);
    }
}
