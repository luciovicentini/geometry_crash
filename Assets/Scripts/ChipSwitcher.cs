using System;
using System.Collections;
using System.Collections.Generic;
using CustomUtil;
using UnityEngine;

public class ChipSwitcher : MonoBehaviour
{
    [SerializeField] float switchChipWaitTime = 0.5f;

    BoardManager boardManager;
    GameScene gameScene;
    AnimatorManager animatorManager;
    Match3Logic match3Logic;
    MatchSeeker matchSeeker;

    Coroutine isSwitchingChips;

    private void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
        gameScene = FindObjectOfType<GameScene>();
        animatorManager = FindObjectOfType<AnimatorManager>();
        match3Logic = FindObjectOfType<Match3Logic>();
        matchSeeker = FindObjectOfType<MatchSeeker>();
    }

    internal bool IsSwitchingChip() => isSwitchingChips != null;

    public IEnumerator SwitchChips(Coord coordChip1, Coord coordChip2)
    {

        boardManager.SwitchChips(coordChip1, coordChip2);
        AnimateSwitching(coordChip1, coordChip2);
        yield return new WaitForSeconds(animatorManager.GetSwitchAnimationTime());

        SoundManager.PlaySound(SoundManager.Sound.ChipSwitching);
        gameScene.SwitchChips(coordChip1, coordChip2);
        yield return new WaitForSeconds(switchChipWaitTime);
        if (HasA3MatchFormed(coordChip1, coordChip2))
        {
            matchSeeker.Init();
        }
        else
        {
            boardManager.SwitchChips(coordChip2, coordChip1);
            AnimateSwitching(coordChip2, coordChip1);
            yield return new WaitForSeconds(animatorManager.GetSwitchAnimationTime());
            gameScene.SwitchChips(coordChip1, coordChip2);
            SoundManager.PlaySound(SoundManager.Sound.ChipSwitchingBack);
        }
        isSwitchingChips = null;
    }


    private bool HasA3MatchFormed(Coord coordChip1, Coord coordChip2) =>
        match3Logic.IsCoordPartOf3MatchLine(coordChip1)
        || match3Logic.IsCoordPartOf3MatchLine(coordChip2);

    private void AnimateSwitching(Coord chip1Coord, Coord chip2Coord)
    {
        GameObject chip1 = gameScene.GetChip(chip1Coord);
        GameObject chip2 = gameScene.GetChip(chip2Coord);

        if (chip1Coord.x > chip2Coord.x)
        {
            animatorManager.AnimateSwitching(chip1, Vector2.left, chip2, Vector2.right);
        }
        if (chip1Coord.x < chip2Coord.x)
        {
            animatorManager.AnimateSwitching(chip1, Vector2.right, chip2, Vector2.left);
        }
        if (chip1Coord.y > chip2Coord.y)
        {
            animatorManager.AnimateSwitching(chip1, Vector2.down, chip2, Vector2.up);
        }
        if (chip1Coord.y < chip2Coord.y)
        {
            animatorManager.AnimateSwitching(chip1, Vector2.up, chip2, Vector2.down);
        }
    }

}
