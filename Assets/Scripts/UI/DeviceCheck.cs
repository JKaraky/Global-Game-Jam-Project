using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DeviceCheck : MonoBehaviour
{
    [Header("Player Controls")]
    [SerializeField]
    private InputActionReference continuePlayerOne;
    [SerializeField]
    private InputActionReference continuePlayerTwo;

    [Header("Robot Jam Option")]
    [SerializeField]
    private GameObject robotJamOption;

    [Header("To Turn On")]
    [SerializeField]
    private List<GameObject> humanObjectsOn;
    [SerializeField]
    private List<GameObject> robotObjectsOn;

    [Header("To Turn Off")]
    [SerializeField]
    private List<GameObject> humanObjectsOff;
    [SerializeField]
    private List<GameObject> robotObjectsOff;

    [Header ("Start of Game")]
    [SerializeField]
    private List<GameObject> startGameOnList;
    [SerializeField]
    private List<GameObject> startGameOffList;

    [Header ("Turn off Start of Game")]

    [Header("Background Images")]
    [SerializeField]
    private Image humanBackground;
    [SerializeField]
    private Image robotBackground;

    [Header("Settings")]
    [SerializeField]
    private float backgroundFadeSpeed = 0.5f;
    [SerializeField]
    private float startGameDelay = 0.5f;

    private bool fadingHuman = false;
    private bool fadingRobot = false;
    private Color tempColorHuman, tempColorRobot;

    private bool humanJamOrBoost, robotJamOrBoost;

    public static event Action<bool, bool> SetPlayerAbility;

    private void Update()
    {
        if (fadingHuman)
        {
            tempColorHuman = humanBackground.color;
            tempColorHuman.a = Mathf.Lerp(tempColorHuman.a, 0.5f, backgroundFadeSpeed);

            humanBackground.color = tempColorHuman;
        }
        if (fadingRobot)
        {
            tempColorRobot = robotBackground.color;
            tempColorRobot.a = Mathf.Lerp(tempColorRobot.a, 0.5f, backgroundFadeSpeed);

            robotBackground.color = tempColorRobot;
        }
    }

    private void PlayerOnePressed (InputAction.CallbackContext context)
    {
        foreach (GameObject obj in humanObjectsOff)
        {
            obj.SetActive (false);
        }

        foreach (GameObject obj in humanObjectsOn)
        {
            obj.SetActive (true);
        }

        fadingHuman = true;

        continuePlayerOne.action.performed -= PlayerOnePressed;
        continuePlayerTwo.action.performed += PlayerTwoPressed;

        // Check what ability player chose
        HandlePlayerChoice(0);

        EventSystem.current.SetSelectedGameObject(robotJamOption);
    }
    private void PlayerTwoPressed(InputAction.CallbackContext context)
    {
        foreach (GameObject obj in robotObjectsOff)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in robotObjectsOn)
        {
            obj.SetActive(true);
        }

        fadingRobot = true;

        // Check what ability player chose
        HandlePlayerChoice(1);

        // Start game
        StartCoroutine(StartGame (startGameDelay));
    }

    private void HandlePlayerChoice(int player)
    {
        string optionName = EventSystem.current.currentSelectedGameObject.name;
        bool option;

        switch (optionName)
        {
            case "Jam Option":
                option = true;
                break;
            case "Boost Option":
                option = false;
                break;
            default:
                option = true;
                break;
        }

        if (player == 0)
        {
            humanJamOrBoost = option;
        }
        else
            robotJamOrBoost = option;
    }

    IEnumerator StartGame(float duration)
    {
        yield return new WaitForSeconds (duration);

        foreach (GameObject obj in startGameOffList)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in startGameOnList)
        {
            obj.SetActive(true);
        }

        // Send event to Avatar Controller to set their abilities. Should also change UI
        SetPlayerAbility?.Invoke(humanJamOrBoost, robotJamOrBoost);
    }
    private void OnEnable()
    {
        continuePlayerOne.action.performed += PlayerOnePressed;
    }
    private void OnDisable()
    {
        continuePlayerOne.action.performed -= PlayerOnePressed;
        continuePlayerTwo.action.performed -= PlayerTwoPressed;
    }
}
