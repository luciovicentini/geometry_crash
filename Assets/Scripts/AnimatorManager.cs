using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    [SerializeField] List<AnimationClip> animations; 
    
    private int GetRandomAnimationIndex() => UnityEngine.Random.Range(0, animations.Count);

    internal void SetAnimation(Animator chipAnimator) => chipAnimator.SetInteger("animationIndex", GetRandomAnimationIndex());
}
