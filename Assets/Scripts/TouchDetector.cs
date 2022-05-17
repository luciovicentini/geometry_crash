using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDetector : MonoBehaviour
{
    private Camera mainCamera;

    public static event Action<String> OnChipClicked = delegate { };

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void StartTouch(TouchData touchData)
    {
        GameObject chipClicked = Utils.DetectObject(mainCamera, touchData.Position);
        if (chipClicked == null) return;
        OnChipClicked(chipClicked.name);
    }
}
