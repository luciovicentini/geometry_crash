using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    private Camera mainCamera;
    private InputManager inputManager;
    private TouchData touchData;

    public static event Action<String> OnChipClicked = delegate { };

    private void Awake()
    {
        mainCamera = Camera.main;
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += StartTouch;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= StartTouch;
    }

    private void StartTouch(TouchData touchData)
    {
        this.touchData = touchData;
        DetectObject();
    }


    private void DetectObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(touchData.Position);
        RaycastHit2D hits2D = Physics2D.GetRayIntersection(ray);
        if (hits2D.collider != null)
        {
            OnChipClicked(hits2D.collider.gameObject.name);
        }
    }
}
