using UnityEngine;

public class AnimationController
{
    private readonly Animator animator;
    private int currentState;

    // some common animations
    public static readonly int WALK_ANIMATION = Animator.StringToHash("Walk");
    public static readonly int ATTACK_ANIMATION = Animator.StringToHash("Attack");

    public int CurrentState => currentState;

    private AnimationController() { }

    public AnimationController(Animator animator)
    {
        this.animator = animator;
    }

    public void ChangeAnimationState(int newState, float? normalizedTime = null)
    {
        if (currentState == newState) return;

        currentState = newState;

        animator.CrossFade(newState, 0.3f, 0, normalizedTime == null ? 1 : normalizedTime.Value);
    }
}
