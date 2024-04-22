using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.InputSystem.PlayerInput playerInput1, playerInput2;
    [SerializeField]
    private string p1KeyboardScheme, p2KeyboardScheme, controllerScheme;
    private InputDevice keyboard;
    private List<InputDevice> connectedControllers;

    [Header("For Main Menu")]
    [SerializeField]
    private MenuManager menu;
    
    [Header("For Arena")]
    [SerializeField]
    private UIController ui;
    private void Awake()
    {
        connectedControllers = new List<InputDevice>();

        foreach(InputDevice inputDevice in InputSystem.devices)
        {
            if (inputDevice.name == "Keyboard")
            {
                keyboard = inputDevice;
            }
            if (inputDevice is Gamepad)
            {
                connectedControllers.Add(inputDevice);
            }
        }
        SwitchControllerScheme();
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad gamepad)
        {
            // This event is related to the specific player's gamepad
            if (change == InputDeviceChange.Added)
            {
                connectedControllers.Add(gamepad);
            }
            else if (change == InputDeviceChange.Removed)
            {
                connectedControllers.Remove(gamepad);
            }
            SwitchControllerScheme();
        }
    }

    private void SwitchControllerScheme()
    {
        switch (connectedControllers.Count)
        {
            case 0:
                playerInput1.SwitchCurrentControlScheme(p1KeyboardScheme, keyboard);
                playerInput2.SwitchCurrentControlScheme(p2KeyboardScheme, keyboard);
                if (menu != null)
                    menu.OnDeviceChange("keyboard", "keyboard");
                if (ui != null)
                    ui.OnDeviceChange("keyboard", "keyboard");
                break;
            case 1:
                playerInput1.SwitchCurrentControlScheme(p1KeyboardScheme, keyboard);
                playerInput2.SwitchCurrentControlScheme(controllerScheme, connectedControllers[0]);
                if (menu != null)
                    menu.OnDeviceChange("keyboard", "controller");
                if (ui != null)
                    ui.OnDeviceChange("keyboard", "controller");
                break;
            case 2:
                playerInput1.SwitchCurrentControlScheme(controllerScheme, connectedControllers[1]);
                playerInput2.SwitchCurrentControlScheme(controllerScheme, connectedControllers[0]);
                if (menu != null)
                    menu.OnDeviceChange("controller", "controller");
                if (ui != null)
                    ui.OnDeviceChange("controller", "controller");
                break;
        }
    }

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }
}
