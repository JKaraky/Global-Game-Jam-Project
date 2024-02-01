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
                break;
            case 1:
                playerInput1.SwitchCurrentControlScheme(p1KeyboardScheme, keyboard);
                playerInput2.SwitchCurrentControlScheme(controllerScheme, connectedControllers[0]);
                break;
            case 2:
                playerInput1.SwitchCurrentControlScheme(controllerScheme, connectedControllers[1]);
                playerInput2.SwitchCurrentControlScheme(controllerScheme, connectedControllers[0]);
                break;
        }
        Debug.Log("Player 1 using scheme " + playerInput1.currentControlScheme);
        Debug.Log("Player 2 using scheme " + playerInput2.currentControlScheme);
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
