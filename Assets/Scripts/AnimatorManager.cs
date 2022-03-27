using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    [SerializeField][Range(1, 9999)] int animationChance = 1000;

    public bool ShouldStartAnimation() => UnityEngine.Random.Range(0, animationChance) == 0;
}
