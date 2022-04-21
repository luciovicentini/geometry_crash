using System;
using System.Collections;
using System.Collections.Generic;
using CustomUtil;
using UnityEngine;

public class ChipSwitcher : MonoBehaviour
{
    [SerializeField] float switchChipWaitTime = 0.5f;
    GameObject chip1;

    GameObject chip2;

    BoardManager boardManager;
    GameScene gameScene;
    AnimatorManager animatorManager;
    Match3Logic match3Logic;
    MatchSeeker matchSeeker;
    private void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
        gameScene = FindObjectOfType<GameScene>();
        animatorManager = FindObjectOfType<AnimatorManager>();
        match3Logic = FindObjectOfType<Match3Logic>();
        matchSeeker = FindObjectOfType<MatchSeeker>();
    }

    public void SetChipClicked(GameObject selectedChip)
    {
        if (chip1 == null)
        {
            chip1 = selectedChip;
        }
        else
        {
            if (chip1 == selectedChip)
            {
                chip1 = null;
            }
            else
            {
                chip2 = selectedChip;
            }
        }

        if (chip1 != null && chip2 != null)
        {
            StartCoroutine(SwitchChips(selectedChip));
        }
    }

    private IEnumerator SwitchChips(GameObject selectedChip)
    {
        Coord coordChip1 = GetCoordFromChip(chip1.transform.parent.gameObject);
        Coord coordChip2 = GetCoordFromChip(chip2.transform.parent.gameObject);
        if (AreContinuous(coordChip1, coordChip2))
        {
            boardManager.SwitchChips(coordChip1, coordChip2);
            AnimateSwitching(coordChip1, coordChip2);
            yield return new WaitForSeconds(animatorManager.GetSwitchAnimationTime());
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
            }
            ForgetSelectedChips();
        }
        else
        {
            chip1.transform.parent.GetComponent<ClickDetector>().ResetSelection();
            chip1 = selectedChip;
            chip2 = null;
            yield return null;
        }
    }


    private bool HasA3MatchFormed(Coord coordChip1, Coord coordChip2) =>
        match3Logic.IsCoordPartOf3MatchLine(coordChip1)
        || match3Logic.IsCoordPartOf3MatchLine(coordChip2);

    private bool AreContinuous(Coord chip1Coords, Coord chip2Coords)
    {

        if ((chip1Coords.x + 1 == chip2Coords.x
            || chip1Coords.x - 1 == chip2Coords.x)
            && chip1Coords.y == chip2Coords.y)
        {
            return true;
        }
        if ((chip1Coords.y + 1 == chip2Coords.y
            || chip1Coords.y - 1 == chip2Coords.y)
            && chip1Coords.x == chip2Coords.x)
        {
            return true;
        }
        return false;
    }

    private void ForgetSelectedChips()
    {
        chip1.transform.parent.GetComponent<ClickDetector>().ResetSelection();
        chip1 = null;

        chip2.transform.parent.GetComponent<ClickDetector>().ResetSelection();
        chip2 = null;
    }

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

    private Coord GetCoordFromChip(GameObject holderChip) => Coord.GetCoordFromChipHolderName(holderChip.name);
}
