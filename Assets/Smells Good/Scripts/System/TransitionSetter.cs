using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSetter : MonoBehaviour
{
    public TransitionState transitionState;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        switch (transitionState)
        {
            case TransitionState.Disappear:
                anim.Play("Disappear");
                break;
            case TransitionState.FadeIn:
                anim.Play("FadeIn");
                break;
            case TransitionState.Appear:
                anim.Play("Appear");
                break;
            case TransitionState.FadeOut:
                anim.Play("FadeOut");
                break;
        }
    }
}

public enum TransitionState
{
    Disappear,
    FadeIn,
    Appear,
    FadeOut
}
