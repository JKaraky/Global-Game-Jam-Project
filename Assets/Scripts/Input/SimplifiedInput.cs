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
    //[SerializeField]
    //private InputActionReference toggleControlPoint;

    private Vector2 _movement;

    private Action<InputAction.CallbackContext> destroyTrigger;
    private Action<InputAction.CallbackContext> hamperTrigger;
    private Action<InputAction.CallbackContext> toggleControlTrigger;

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
        //toggleControlTrigger = (ctx) => ToggleControlBtnPressed();
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

    //private void ToggleControlBtnPressed()
    //{
    //    avatarController.ToggleControlPointSlot();
    //}
    #endregion
    #region Event Subscribing
    private void OnEnable()
    {
        destroy.action.started += DestroyBtnHeld;
        destroy.action.performed += destroyTrigger;
        destroy.action.canceled += destroyTrigger;
        hamper.action.performed += hamperTrigger;
        //toggleControlPoint.action.performed += toggleControlTrigger;
    }
    private void OnDisable()
    {
        destroy.action.started -= DestroyBtnHeld;
        destroy.action.performed -= destroyTrigger;
        destroy.action.canceled -= destroyTrigger;
        hamper.action.performed -= hamperTrigger;
        //toggleControlPoint.action.performed -= toggleControlTrigger;
    }
    #endregion
}
