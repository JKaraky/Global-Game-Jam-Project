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
    [SerializeField]
    public bool moveListen;
    [SerializeField]
    public bool destroyListen;
    [SerializeField]
    public bool hamperListen;
    [SerializeField]
    public bool continueListen;

    public static event Action<int> ContinuePressed;
    public static event Action Moved;
    public static event Action Destroyed;
    public static event Action Jammed;
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

    private void MoveButtonPressed()
    {
        Moved?.Invoke();
    }

    private void JamButtonPressed()
    {
        Jammed?.Invoke();
    }

    private void DestroyButtonPressed()
    {
        Destroyed?.Invoke();
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
        if (continueListen)
        {
            continueButton.action.performed += continueTrigger;
        }
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
        if (continueListen)
        {
            continueButton.action.performed -= continueTrigger;
        }
    }
    #endregion
}
