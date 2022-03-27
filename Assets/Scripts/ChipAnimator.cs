using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipAnimator : MonoBehaviour
{
    private const string IDLE_ANIM_NAME = "Idle";
    [SerializeField] bool shouldStartAnim = false;
    [SerializeField] List<AnimationClip> animations;
    Animator chipAnimator;
    AnimatorManager animationManager;
    private string currentState;

    private void Start()
    {
        animationManager = FindObjectOfType<AnimatorManager>();
        chipAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!ShouldStartAnimation()) return;
        AnimationClip animation = GetRandomAnimation();
        String animationClipString = animation ? animation.name : GetIdleStateString();
        ChangeAnimationState(animationClipString);
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        chipAnimator.Play(newState);
        currentState = newState;
        shouldStartAnim = false;
    }

    private bool ShouldStartAnimation()
    {
        return shouldStartAnim || animationManager.ShouldStartAnimation();
    }

    internal AnimationClip GetRandomAnimation()
    {
        if (animations.Count == 0) return null;
        return animations[GetRandomAnimationIndex()];
    }

    internal string GetIdleStateString()
    {
        return IDLE_ANIM_NAME;
    }

    internal int GetRandomAnimationIndex() => UnityEngine.Random.Range(0, animations.Count);
}
