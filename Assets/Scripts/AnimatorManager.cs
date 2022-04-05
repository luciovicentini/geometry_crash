using System;
using System.Collections;
using System.Collections.Generic;
using CustomUtil;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    [SerializeField][Range(0, 9999)] int animationChance = 1000;
    [SerializeField] float destroyAnimationTime = 0.1f;
    [SerializeField] float createAnimationTime = 0.1f;
    [SerializeField] float fallingAnimationTime = 0.1f;
    [SerializeField] float switchAnimationTime = 0.1f;

    [SerializeField][Range(0f, 1f)] float holderPaddingScale = 0.9f;

    public bool ShouldStartAnimation() => UnityEngine.Random.Range(0, animationChance)
        == UnityEngine.Random.Range(0, 10);

    public void AnimateChipHide(GameObject chip, bool destroyIt = false)
    {
        Debug.Log($"AnimateChipHide {chip}");
        chip.transform
            .LeanScale(Vector2.zero, destroyAnimationTime)
            .setEaseOutQuad()
            .setDestroyOnComplete(destroyIt);
    }

    public void AnimateChipShow(GameObject chip)
    {
        Vector2 finalScale = GetScale(chip);
        chip.transform.localScale = Vector2.zero;
        chip.transform
            .LeanScale(finalScale, createAnimationTime)
            .setEaseOutQuad();
    }

    private Vector3 GetScale(GameObject chip)
    {
        return chip.transform.localScale * holderPaddingScale;
    }

    internal void AnimateFallingChips(List<GameObject> chips, int amountSteps)
    {
        Debug.Log("Animating Horizontal Falling");
        foreach (GameObject chip in chips)
        {
            AnimateFallingChip(chip, amountSteps);
        }
    }

    private void AnimateFallingChip(GameObject chip, float amount)
    {
        chip.transform
            .LeanMoveLocalY(amount, fallingAnimationTime)
            .setEaseOutQuart();
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
            .setEaseInOutQuad();
    }

    internal float GetDestroyAnimationTime() => destroyAnimationTime;
    internal float GetCreateAnimationTime() => createAnimationTime;
    internal float GetFallingAnimationTime() => fallingAnimationTime;
    internal float GetSwitchAnimationTime() => switchAnimationTime;

}
