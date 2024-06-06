using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsUI : MonoBehaviour
{
    #region Variables
    private int fullFill;
    private float fillAmount;

    [SerializeField]
    private Image playerOneHuman;
    [SerializeField]
    private Image playerOneRobot;
    [SerializeField]
    private Image playerTwoHuman;
    [SerializeField]
    private Image playerTwoRobot;
    [SerializeField]
    private Slider pointsSlider;

    [SerializeField]
    private Points pointsScript;
    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        fullFill = pointsScript.StartingPoints;

        if(fullFill > 0)
        {
            fillAmount = (float) 1 / fullFill;
        }
        else
        {
            Debug.LogError("Points UI is unable to get amount of starting points and is currently set to " + fullFill);
        }
    }
    #endregion

    #region Points Management Methods
    private void PlayerOnePoint()
    {
        pointsSlider.value++;
        if(playerOneRobot.fillAmount > 0)
        {
            playerOneRobot.fillAmount -= fillAmount;
            playerOneHuman.fillAmount += fillAmount;
        }
        else
        {
            playerTwoHuman.fillAmount += fillAmount;
            playerTwoRobot.fillAmount -= fillAmount;
        }
    }

    private void PlayerTwoPoint()
    {
        pointsSlider.value--;
        if (playerTwoHuman.fillAmount > 0)
        {
            playerTwoHuman.fillAmount -= fillAmount;
            playerTwoRobot.fillAmount += fillAmount;
        }
        else
        {
            playerOneRobot.fillAmount += fillAmount;
            playerOneHuman.fillAmount -= fillAmount;
        }
    }

    private void GivePoint(Players player)
    {
        if(player == Players.PlayerOne)
        {
            PlayerOnePoint();
        }
        else if(player == Players.PlayerTwo)
        {
            PlayerTwoPoint();
        }
        else
        {
            Debug.LogError("PointsUI script is receiving Nobody as an input for giving points");
        }
    }
    #endregion

    #region OnEnable and OnDisable
    private void OnEnable()
    {
        Points.PointForPlayer += GivePoint;
    }

    private void OnDisable()
    {
        Points.PointForPlayer -= GivePoint;
    }
    #endregion
}
