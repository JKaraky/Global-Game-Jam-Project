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
    private Coroutine _movementCoroutine;

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
    //void Update()
    //{
    //    MovementInput();
    //}
    #region Event Handlers
    protected void MovementInput()
    {
        _movement = move.action.ReadValue<Vector2>();
        avatarController.AttemptMovement();
    }
    private void OnMovementStarted(InputAction.CallbackContext context)
    {
        // Start the coroutine to move continuously
        if (_movementCoroutine == null)
        {
            _movementCoroutine = StartCoroutine(MoveContinuously());
        }
    }
    private void OnMovementCanceled(InputAction.CallbackContext context)
    {
        // Stop the movement coroutine when the input is canceled (keys released)
        if (_movementCoroutine != null)
        {
            StopCoroutine(_movementCoroutine);
            _movementCoroutine = null;
        }
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
    private IEnumerator MoveContinuously()
    {
        while (true)
        {
            MovementInput();

            // Wait until the next frame
            yield return null;
        }
    }
    #region Event Subscribing
    private void OnEnable()
    {
        destroy.action.started += DestroyBtnHeld;
        move.action.started += OnMovementStarted;
        move.action.canceled += OnMovementCanceled;
        destroy.action.performed += destroyTrigger;
        destroy.action.canceled += destroyTrigger;
        jam.action.performed += jamTrigger;
    }
    private void OnDisable()
    {
        destroy.action.started -= DestroyBtnHeld;
        move.action.started -= OnMovementStarted;
        move.action.canceled -= OnMovementCanceled;
        destroy.action.performed -= destroyTrigger;
        destroy.action.canceled -= destroyTrigger;
        jam.action.performed -= jamTrigger;
    }
    #endregion
}
