using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputDetector : Singleton<InputDetector>
{

    #region Events
    public delegate void StartTouch(TouchData touchData);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(TouchData touchData);
    public event EndTouch OnEndTouch;
    #endregion

    private TouchControls touchControls;
    private Camera mainCamera;

    private void Awake()
    {
        touchControls = new TouchControls();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        touchControls.Enable();
    }

    private void OnDisable()
    {
        touchControls.Disable();
    }

    private void Start()
    {
        touchControls.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        touchControls.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
    }

    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null)
        {
            Vector2 position = touchControls.Touch.PrimaryPosition.ReadValue<Vector2>();
            float startTime = (float)context.startTime;
            TouchData data = new TouchData { Position = position, Time = startTime };
            OnStartTouch(data);
        }
    }

    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null)
        {
            Vector2 position = touchControls.Touch.PrimaryPosition.ReadValue<Vector2>();
            float time = (float)context.time;
            TouchData data = new TouchData { Position = position, Time = time };
            OnEndTouch(data);
        }
    }

    public Vector2 PrimaryPosition()
    {
        return Utils.ScreenToWorld(mainCamera, touchControls.Touch.PrimaryPosition.ReadValue<Vector2>());
    }
}

public struct TouchData
{
    public Vector2 Position;
    public float Time;

    public override string ToString()
    {
        return $"(Position: (x:{this.Position.x},y:{Position.y}), Time: {this.Time})";
    }
}