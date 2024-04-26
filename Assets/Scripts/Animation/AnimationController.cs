using System.Linq;
using UnityEngine;

public class AnimationController
{
    private readonly Animator animator;
    private AnimationHash currentState;

    // some common animations
    public static readonly AnimationHash WALK_ANIMATION = new("Walk");
    public static readonly AnimationHash ATTACK_ANIMATION = new("Attack");
    public static readonly AnimationHash HIT_ANIMATION = new("Hit");

    // use runtimeAnimationController instead of animator.GetCurrentAnimatorClipInfo because it may be out of sync
    public float CurrentAnimationLength => animator.runtimeAnimatorController.animationClips.Where(x => x.name == currentState.name).First().length;
    public AnimationHash CurrentAnimationState => currentState;

    private AnimationController() { }

    public AnimationController(Animator animator)
    {
        this.animator = animator;
    }

    /// <summary>
    /// Change animation state to new state passed in. If new state is the same as current state, nothing will happen.
    /// </summary>
    /// <param name="newState">State Hash name</param>
    /// <param name="time">Fixed time to play the animation in.</param>
    public void ChangeAnimationState(AnimationHash newState, float time = 0, bool transition = true)
    {
        if (currentState == newState) return;

        currentState = newState;

        if (time == 0) time = CurrentAnimationLength;

        if (transition)
            animator.CrossFade(newState.hash, 0.1f);
        else
            animator.Play(newState.hash);

        animator.speed = CurrentAnimationLength / time;
    }
}
