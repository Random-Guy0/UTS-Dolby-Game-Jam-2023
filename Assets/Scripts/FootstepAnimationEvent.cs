using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAnimationEvent : MonoBehaviour
{
    private void OnFootStep(AnimationEvent animationEvent)
    {
        FindObjectOfType<PlayerController>().OnFootstep(animationEvent);
    }
}
