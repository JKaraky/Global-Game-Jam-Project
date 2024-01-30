using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    #region Variables
    [Header("Player 1 or Player 2?")]
    [SerializeField]
    private int playerNumber;
    [Header("Player Controls")]
    [SerializeField]
    private InputActionReference move;
    [SerializeField]
    private InputActionReference destroy;
    [SerializeField]
    private InputActionReference hamper;
    [SerializeField]
    private InputActionReference toggleControlPoint;

    private Vector3 _movement;

    private Action<InputAction.CallbackContext> destroyTrigger;
    private Action<InputAction.CallbackContext> hamperTrigger;
    private Action<InputAction.CallbackContext> toggleControlTrigger;

    public static event Action DestroyP1;
    public static event Action DestroyP2;
    public static event Action HamperP1;
    public static event Action HamperP2;
    public static event Action ToggleControlPointP1;
    public static event Action ToggleControlPointP2;
    public static event Action<int> ShowDestroyRadius;
    public static event Action<int> HideDestroyRadius;
    #endregion
    public Vector3 Movement
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
        toggleControlTrigger = (ctx) => ToggleControlBtnPressed();
    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();
    }
    #region Event Handlers
    private void MovementInput()
    {
        _movement = move.action.ReadValue<Vector3>();
    }

    private void DestroyBtnPressed()
    {
        HideDestroyRadius?.Invoke(playerNumber);

        if (playerNumber == 0)
        {
            DestroyP1?.Invoke();
        }
        else if (playerNumber == 1)
        {
            DestroyP2?.Invoke();
        }
    }

    private void DestroyBtnHeld(InputAction.CallbackContext context)
    {
        ShowDestroyRadius?.Invoke(playerNumber);
    }

    private void HamperBtnPressed()
    {
        if (playerNumber == 0)
        {
            HamperP1?.Invoke();
        }
        else if (playerNumber == 1)
        {
            HamperP2?.Invoke();
        }
    }

    private void ToggleControlBtnPressed()
    {
        if (playerNumber == 0)
        {
            ToggleControlPointP1?.Invoke();
        }
        else if (playerNumber == 1)
        {
            ToggleControlPointP2?.Invoke();
        }
    }
    #endregion
    #region Event Subscribing
    private void OnEnable()
    {
        destroy.action.started += DestroyBtnHeld;
        destroy.action.performed += destroyTrigger;
        destroy.action.canceled += destroyTrigger;
        hamper.action.performed += hamperTrigger;
        toggleControlPoint.action.performed += toggleControlTrigger;
    }
    private void OnDisable()
    {
        destroy.action.started -= DestroyBtnHeld;
        destroy.action.performed -= destroyTrigger;
        destroy.action.canceled -= destroyTrigger;
        hamper.action.performed -= hamperTrigger;
        toggleControlPoint.action.performed -= toggleControlTrigger;
    }
    #endregion
}
