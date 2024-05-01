using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialInput : SimplifiedInput
{
    #region Variables
    [Header("Actions To Listen To")]
    [SerializeField]
    private bool moveListen;
    [SerializeField]
    private bool destroyListen;
    [SerializeField]
    private bool hamperListen;

    public static event Action<int> ShowOrDestroyRadius;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (moveListen)
        {
            MovementInput();
        }
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
    }
    #endregion
}
