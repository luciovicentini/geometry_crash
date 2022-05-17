using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    [SerializeField] private float minimumDistance = .2f;
    [SerializeField] private float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)] private float directionThreshold = .9f;

       
    public static event Action<SwipeData> OnSwipe = delegate { };


    public bool IsSwiping(TouchData startTouch, TouchData endTouch) => (Vector3.Distance(startTouch.Position, endTouch.Position) >= minimumDistance &&
            (endTouch.Time - startTouch.Time) <= maximumTime);


    public void DetectSwipe(TouchData startTouch, TouchData endTouch)
    {
        Vector3 direction = endTouch.Position - startTouch.Position;
        Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
        SwipeDirection? swipeDirection = GetSwipeDirection(direction2D);
        if (swipeDirection != null)
        {
            SendSwipe(startTouch, endTouch, (SwipeDirection) swipeDirection);
        }
    }

    private SwipeDirection? GetSwipeDirection(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            return (SwipeDirection.Up);
        }
        if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            return (SwipeDirection.Down);
        }
        if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            return (SwipeDirection.Left);
        }
        if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            return (SwipeDirection.Right);
        }
        return null;
    }

    private void SendSwipe(TouchData startTouchData, TouchData endTouchData, SwipeDirection swipeDirection)
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