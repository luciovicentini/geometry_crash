using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* This class detect from input manager if the user is touching or swiping.
*/
public class InputManager : MonoBehaviour
{
    private InputDetector inputDetector;
    private SwipeDetector swipeDetector;
    private TouchDetector touchDetector;

    private TouchData startTouchData;
    private TouchData endTouchData;

    // public static event Action<String> OnChipClicked = delegate { };
    // public static event Action<SwipeData> OnSwipe = delegate { };

    private void Awake()
    {
        inputDetector = InputDetector.Instance;
        swipeDetector = FindObjectOfType<SwipeDetector>();
        touchDetector = FindObjectOfType<TouchDetector>();
    }

    private void OnEnable()
    {
        inputDetector.OnStartTouch += StartTouch;
        inputDetector.OnEndTouch += EndTouch;
    }

    private void OnDisable()
    {
        inputDetector.OnStartTouch -= StartTouch;
        inputDetector.OnEndTouch -= EndTouch;
    }

    private void StartTouch(TouchData touchData)
    {
        startTouchData = touchData;
    }

    private void EndTouch(TouchData touchData)
    {
        endTouchData = touchData;
        ProcessTouches();
    }

    private void ProcessTouches()
    {
        if (swipeDetector.IsSwiping(startTouchData, endTouchData)) {
            swipeDetector.DetectSwipe(startTouchData, endTouchData);
        } else {
            touchDetector.StartTouch(startTouchData);
        }
    }
}
