using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    [SerializeField][Range(0, 9999)] int animationChance = 1000;
    [SerializeField] float destroyAnimationTime = 0.1f;
    [SerializeField] float createAnimationTime = 0.1f;
    [SerializeField] float fallingAnimationTime = 0.1f;
    [SerializeField] float switchAnimationTime = 0.1f;
    [SerializeField] float selectionAnimationTime = 0.1f;

    [SerializeField][Range(0f, 1f)] float holderPadding = 0.9f;

    static Color transparent = new Color(255f, 255f, 255f, 0f);
    static Color opaque = new Color(255f, 255f, 255f, 1f);

    private enum AnimationManagerState
    {
        Running,
        Idle,
    }

    private AnimationManagerState state;

    public void AnimateChipHide(GameObject chip, bool destroyIt = false)
    {
        SoundManager.PlaySound(SoundManager.Sound.ChipDestroying, chip.transform.position);
        chip.transform
            .LeanScale(Vector2.zero, destroyAnimationTime)
            .setEaseOutQuad()
            .setOnStart(AnimationStateRunning)
            .setOnComplete(AnimationStateIdling)
            .setDestroyOnComplete(destroyIt);
    }

    private void AnimationStateRunning()
    {
        state = AnimationManagerState.Running;
    }


    private void AnimationStateIdling()
    {
        state = AnimationManagerState.Idle;
    }

    public void AnimateChipShow(GameObject chip)
    {
        SoundManager.PlaySound(SoundManager.Sound.ChipCreating, chip.transform.position);
        Vector2 finalScale = GetScale(chip);
        chip.transform.localScale = Vector2.zero;
        chip.transform
            .LeanScale(finalScale, createAnimationTime)
            .setEaseOutQuad()
            .setOnStart(AnimationStateRunning)
            .setOnComplete(AnimationStateIdling);
    }

    private Vector3 GetScale(GameObject chip)
    {
        return chip.transform.localScale * holderPadding;
    }

    internal void AnimateFallingChips(List<GameObject> chips, int amountSteps)
    {
        foreach (GameObject chip in chips)
        {
            AnimateFallingChip(chip, amountSteps);
        }
    }

    private void AnimateFallingChip(GameObject chip, float amount)
    {
        chip.transform
            .LeanMoveLocalY(amount * -1, fallingAnimationTime)
            .setEaseOutQuart()
            .setOnStart(AnimationStateRunning)
            .setOnComplete(AnimationStateIdling);
    }

    internal void AnimateSwitching(GameObject chip1, Vector2 chip1To, GameObject chip2, Vector2 chip2To)
    {
        AnimateChipMoving(chip1, chip1To);
        AnimateChipMoving(chip2, chip2To);
    }

    private void AnimateChipMoving(GameObject chip, Vector2 to)
    {
        chip.transform
            .LeanMoveLocal(to, switchAnimationTime)
            .setEaseInOutQuad()
            .setOnStart(AnimationStateRunning)
            .setOnComplete(AnimationStateIdling);
    }

    public void AnimateShowSelectionSprite(GameObject sprite)
    {

        sprite.LeanColor(opaque, selectionAnimationTime)
            .setOnStart(AnimationStateRunning)
            .setOnComplete(AnimationStateIdling)
            .setEaseInOutBack();
    }

    public void AnimateHideSelectionSprite(GameObject sprite)
    {
        sprite.LeanColor(transparent, selectionAnimationTime)
            .setOnStart(AnimationStateRunning)
            .setOnComplete(AnimationStateIdling)
            .setEaseInOutBack();
    }

    internal float GetDestroyAnimationTime() => destroyAnimationTime;
    internal float GetCreateAnimationTime() => createAnimationTime;
    internal float GetFallingAnimationTime() => fallingAnimationTime;
    internal float GetSwitchAnimationTime() => switchAnimationTime;
    internal float GetSelectAnimationTime() => selectionAnimationTime;

    internal bool AnimationsFinished()
    {
        return state == AnimationManagerState.Idle;
    }

}
