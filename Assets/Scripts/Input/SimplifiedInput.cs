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
    private int playerNumber;
    [SerializeField]
    private AvatarController avatarController;
    [Header("Player Controls")]
    [SerializeField]
    private InputActionReference move;
    [SerializeField]
    private InputActionReference destroy;
    [SerializeField]
    private InputActionReference hamper;

    private Vector2 _movement;

    private Action<InputAction.CallbackContext> destroyTrigger;
    private Action<InputAction.CallbackContext> hamperTrigger;

    public static event Action<int> ShowOrDestroyRadius;
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
        hamperTrigger = (ctx) => HamperBtnPressed();
    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();
    }
    #region Event Handlers
    private void MovementInput()
    {
        _movement = move.action.ReadValue<Vector2>();
    }

    private void DestroyBtnPressed()
    {
        ShowOrDestroyRadius?.Invoke(playerNumber);

        avatarController.DestroyPowerup();
    }

    private void DestroyBtnHeld(InputAction.CallbackContext context)
    {
        ShowOrDestroyRadius?.Invoke(playerNumber);
    }

    private void HamperBtnPressed()
    {
        avatarController.JamPlayer();
    }
    #endregion
    #region Event Subscribing
    private void OnEnable()
    {
        destroy.action.started += DestroyBtnHeld;
        destroy.action.performed += destroyTrigger;
        destroy.action.canceled += destroyTrigger;
        hamper.action.performed += hamperTrigger;
    }
    private void OnDisable()
    {
        destroy.action.started -= DestroyBtnHeld;
        destroy.action.performed -= destroyTrigger;
        destroy.action.canceled -= destroyTrigger;
        hamper.action.performed -= hamperTrigger;
    }
    #endregion
}
