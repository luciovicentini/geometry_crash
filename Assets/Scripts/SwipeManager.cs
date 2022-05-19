using System;
using System.Collections;
using System.Collections.Generic;
using CustomUtil;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    AnimatorManager am;
    GameScene gameScene;
    ChipSelectionManager chipSelectionManager;
    Camera main;

    bool isSwiping;

    void Awake()
    {
        gameScene = FindObjectOfType<GameScene>();
        chipSelectionManager = FindObjectOfType<ChipSelectionManager>();
        am = FindObjectOfType<AnimatorManager>();
        main = Camera.main;
    }

    private void OnEnable() {
        SwipeDetector.OnSwipe += OnSwipe;
    }

    private void OnDisable() {
        SwipeDetector.OnSwipe -= OnSwipe;
    }

    private void OnSwipe(SwipeData data)
    {
        if (isSwiping) return;
        GameObject swippedChipHolder = Utils.DetectObject(main, data.StartPosition);
        if (swippedChipHolder == null) return;
        StartCoroutine(ProcessSwipe(swippedChipHolder, data));
    }

    private IEnumerator ProcessSwipe(GameObject swippedChipHolder, SwipeData data)
    {
        isSwiping = true;
        ChipHolderSelect chip1 = swippedChipHolder.GetComponent<ChipHolderSelect>();
        chipSelectionManager.SetChip(chip1);
        yield return new WaitForSeconds(AnimatorManager.GetSelectAnimationTime());
        Coord chip1Coord = Coord.GetCoordFromChip(chip1);
        Coord chip2Coord = chip1Coord.GetCoordBySwipeDirection(data.Direction);
        GameObject chip2 = gameScene.GetHolderChipFromPosition(chip2Coord);
        chipSelectionManager.SetChip(chip2.GetComponent<ChipHolderSelect>());
        yield return new WaitForSeconds(AnimatorManager.GetSelectAnimationTime());
        isSwiping = false;
    }
}
