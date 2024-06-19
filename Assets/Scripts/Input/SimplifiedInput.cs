using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimplifiedInput : MonoBehaviour
{
    #region Variables
    [Header("Player 1 or Player 2?")]
    [SerializeField]
    protected int playerNumber;
    [SerializeField]
    protected AvatarController avatarController;
    [Header("Player Controls")]
    [SerializeField]
    protected InputActionReference move;
    [SerializeField]
    protected InputActionReference destroy;
    [SerializeField]
    protected InputActionReference jam;

    protected Vector2 _movement;

    protected Action<InputAction.CallbackContext> destroyTrigger;
    protected Action<InputAction.CallbackContext> jamTrigger;

    public static event Action<int, bool> ShowOrDestroyRadius;
    #endregion
    public Vector2 Movement
    {
        get
        {
            return _movement;
        }
    }
    private void Awake()
    {
        destroyTrigger = (ctx) => DestroyBtnPressed();
        jamTrigger = (ctx) => JamBtnPressed();
    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();
    }
    #region Event Handlers
    protected void MovementInput()
    {
        _movement = move.action.ReadValue<Vector2>();
    }

    protected void DestroyBtnPressed()
    {
        ShowOrDestroyRadius?.Invoke(playerNumber, false);

        avatarController.DestroyPowerup();
    }

    protected void DestroyBtnHeld(InputAction.CallbackContext context)
    {
        ShowOrDestroyRadius?.Invoke(playerNumber, true);
    }

    protected void JamBtnPressed()
    {
        avatarController.SecondaryActionPressed();
    }
    #endregion
    #region Event Subscribing
    private void OnEnable()
    {
        destroy.action.started += DestroyBtnHeld;
        destroy.action.performed += destroyTrigger;
        destroy.action.canceled += destroyTrigger;
        jam.action.performed += jamTrigger;
    }
    private void OnDisable()
    {
        destroy.action.started -= DestroyBtnHeld;
        destroy.action.performed -= destroyTrigger;
        destroy.action.canceled -= destroyTrigger;
        jam.action.performed -= jamTrigger;
    }
    #endregion
}
