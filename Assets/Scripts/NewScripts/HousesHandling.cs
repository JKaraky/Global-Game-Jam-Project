using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousesHandling : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject humanCity;
    [SerializeField]
    private GameObject robotCity;
    [SerializeField]
    private GameObject takeoverHumanCity;
    [SerializeField]
    private GameObject takeoverRobotCity;


    private List<GameObject> humanHouses = new List<GameObject>();
    private List<GameObject> robotHouses = new List<GameObject>();
    private List<GameObject> humanTakeoverHouses = new List<GameObject>();
    private List<GameObject> robotTakeoverHouses = new List<GameObject>();

    private int pointsToWin;
    private int housesToActivate;
    private int activatedHumanHouses = 0;
    private int activatedRobotHouses = 0;
    #endregion

    #region Start
    private void Start()
    {
        // Fill houses list
        fillList(humanCity, humanHouses);
        fillList(robotCity, robotHouses);
        fillList(takeoverHumanCity, humanTakeoverHouses);
        fillList(takeoverRobotCity, robotTakeoverHouses);

        // get how many points to win and correlate it with how many houses to turn on for each point
        pointsToWin = transform.parent.gameObject.GetComponent<Points>().StartingPoints;
        housesToActivate = humanHouses.Count / pointsToWin;

        if(housesToActivate < 1)
        {
            Debug.LogError("Check starting points, must not be below zero or more than amount of houses");
        }
    }
    #endregion

    #region Setup Methods
    private void fillList(GameObject gameObject, List<GameObject> list)
    {
        foreach(Transform child in gameObject.transform)
        {
            list.Add(child.gameObject);
        }
    }
    #endregion

    #region Handling Methods
    private void AddHouses(Players player)
    {
        if(player == Players.PlayerOne)
        {
            ActivateHumanHouses();
        }
        else if(player == Players.PlayerTwo)
        {
            ActivateRobotHouses();
        }
        else
        {
            Debug.LogError("HousesHandling got sent Nobody as a player that gained a point");
        }
    }
    
    private void ActivateHumanHouses()
    {
        if(activatedRobotHouses <= 0)
        {
            for (int i = activatedHumanHouses; i < activatedHumanHouses + housesToActivate; i++)
            {
                humanTakeoverHouses[i].SetActive(true);
                robotHouses[i].SetActive(false);
            }

            activatedHumanHouses += housesToActivate;
        }
        else
        {
            for (int i = activatedRobotHouses - housesToActivate; i < activatedRobotHouses; i++)
            {
                robotTakeoverHouses[i].SetActive(false);
                humanHouses[i].SetActive(true);
            }

            activatedRobotHouses -= housesToActivate;
        }
    }

    private void ActivateRobotHouses()
    {
        if (activatedHumanHouses <= 0)
        {
            for (int i = activatedRobotHouses; i < activatedRobotHouses + housesToActivate; i++)
            {
                robotTakeoverHouses[i].SetActive(true);
                humanHouses[i].SetActive(false);
            }

            activatedRobotHouses += housesToActivate;
        }
        else
        {
            for (int i = activatedHumanHouses - housesToActivate; i < activatedHumanHouses; i++)
            {
                humanTakeoverHouses[i].SetActive(false);
                robotHouses[i].SetActive(true);
            }

            activatedHumanHouses -= housesToActivate;
        }
    }

    // In case game is won and there are still inactive takeover houses due to division being a float
    private void ActivateRest(Players player)
    {
        if(player == Players.PlayerOne)
        {
            if(activatedHumanHouses < humanTakeoverHouses.Count)
            {
                for(int i = activatedHumanHouses; i < humanTakeoverHouses.Count; i++)
                {
                    humanTakeoverHouses[i].SetActive(true);
                    robotHouses[i].SetActive(false);
                }
            }
        }
        else if(player == Players.PlayerTwo)
        {
            if (activatedRobotHouses < robotTakeoverHouses.Count)
            {
                for (int i = activatedRobotHouses; i < robotTakeoverHouses.Count; i++)
                {
                    robotTakeoverHouses[i].SetActive(true);
                    humanHouses[i].SetActive(false);
                }
            }
            Debug.Log("Activated rest of robot houses");
        }
        else
        {
            Debug.LogError("HousesHandling received that Nobody won the game");
        }
    }
    #endregion

    #region OnEnable and OnDisable
    private void OnEnable()
    {
        Points.PointForPlayer += AddHouses;
        Points.PlayerWon += ActivateRest;
    }

    private void OnDisable()
    {
        Points.PointForPlayer -= AddHouses;
        Points.PlayerWon += ActivateRest;
    }
    #endregion
}
