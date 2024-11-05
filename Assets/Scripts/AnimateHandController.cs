using UnityEngine;
using UnityEngine.InputSystem;

// Copyright (C) Tom Troeger

[RequireComponent((typeof(Animator)))]
public class AnimateHandController : MonoBehaviour
{
    private static readonly int Trigger = Animator.StringToHash("Trigger");
    private static readonly int Grip = Animator.StringToHash("Grip");

    public InputActionReference gripInputActionReference;

    public InputActionReference triggerInputActionReference;
    private Animator _handAnimator;
    private float _gripValue;
    private float _triggerValue;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        _handAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        AnimateGrip();
        AnimateTrigger();
    }

    private void AnimateTrigger()
    {
        _triggerValue = triggerInputActionReference.action.ReadValue<float>();
        _handAnimator.SetFloat(Trigger, _triggerValue);
    }

    private void AnimateGrip()
    {
        _gripValue = gripInputActionReference.action.ReadValue<float>();
        _handAnimator.SetFloat(Grip, _gripValue);
    }
}
