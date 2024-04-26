using UnityEngine;

public class PlayerAnimation
{
    private readonly Animator animator;
    private int currentState = IDLE;

    public static readonly int IDLE = Animator.StringToHash("Idle");
    public static readonly int WALK = Animator.StringToHash("Walk");
    public static readonly int RUN = Animator.StringToHash("Run");
    public static readonly int ATTACK = Animator.StringToHash("Attack");

    public int CurrentState => currentState;

    private PlayerAnimation() { }

    public PlayerAnimation(Animator animator)
    {
        this.animator = animator;
    }

    public void ChangeAnimationState(int newState, float? normalizedTime = null)
    {
        if (currentState == newState) return;

        currentState = newState;

        animator.CrossFade(newState, 0, 0, normalizedTime == null ? 1 : normalizedTime.Value);
    }
}
