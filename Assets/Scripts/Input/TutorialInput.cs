using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialInput : SimplifiedInput
{
    #region Variables
    [Header("Additional Action Reference")]
    [SerializeField]
    private InputActionReference continueButton;

    [Header("Actions To Listen To")]
    [HideInInspector]
    public bool moveListen;
    [HideInInspector]
    public bool destroyListen;
    [HideInInspector]
    public bool hamperListen;

    public static event Action<int> ContinuePressed;
    private Action<InputAction.CallbackContext> continueTrigger;
    #endregion
    public new Vector2 Movement
    {
        get
        {
            return _movement;
        }
    }
    private void Awake()
    {
        destroyTrigger = (ctx) => DestroyBtnPressed();
        jamTrigger = (ctx) => HamperBtnPressed();
        continueTrigger = (ctx) => ContinueButtonPressed();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveListen)
        {
            MovementInput();
        }
    }

    private void ContinueButtonPressed()
    {
        ContinuePressed?.Invoke(playerNumber);
    }
    #region Event Subscribing
    private void OnEnable()
    {
        if (destroyListen)
        {
            destroy.action.started += DestroyBtnHeld;
            destroy.action.performed += destroyTrigger;
            destroy.action.canceled += destroyTrigger;
        }
        if (hamperListen)
        {
            jam.action.performed += jamTrigger;
        }

        continueButton.action.performed += continueTrigger;
    }
    private void OnDisable()
    {
        if (destroyListen)
        {
            destroy.action.started -= DestroyBtnHeld;
            destroy.action.performed -= destroyTrigger;
            destroy.action.canceled -= destroyTrigger;
        }
        if (hamperListen)
        {
            jam.action.performed -= jamTrigger;
        }

        continueButton.action.performed -= continueTrigger;
    }
    #endregion
}
