using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    [SerializeField] private float minimumDistance = .2f;
    [SerializeField] private float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)] private float directionThreshold = .9f;

    private InputManager inputManager;
    private TouchData startTouchData;
    private TouchData endTouchData;

    public static event Action<SwipeData> OnSwipe = delegate { };

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(TouchData touchData) => startTouchData = touchData;

    private void SwipeEnd(TouchData touchData)
    {
        endTouchData = touchData;
        DetectSwipe();
    }

    private void DetectSwipe()
    {

        if (Vector3.Distance(startTouchData.Position, endTouchData.Position) >= minimumDistance &&
            (endTouchData.Time - startTouchData.Time) <= maximumTime)
        {
            Vector3 direction = endTouchData.Position - startTouchData.Position;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            GetSwipeDirection(direction2D);
        }
    }

    private void GetSwipeDirection(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            SendSwipe(SwipeDirection.Up);
        }
        if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            SendSwipe(SwipeDirection.Down);
        }
        if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            SendSwipe(SwipeDirection.Left);
        }
        if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            SendSwipe(SwipeDirection.Right);
        }
    }

    private void SendSwipe(SwipeDirection swipeDirection)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = swipeDirection,
            StartPosition = startTouchData.Position,
            EndPosition = endTouchData.Position,
        };
        OnSwipe(swipeData);
    }
}

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right,
}