using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowArrows : MonoBehaviour
{
    [SerializeField]
    private AvatarController humanInput, robotInput;
    [SerializeField]
    private SimplifiedInput humanIn, robotIn;
    [SerializeField]
    private GameObject humanArrow, robotArrow;

    private Vector3 humanMove, robotMove;
    private DirectionArrow humanArrowScript, robotArrowScript;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        humanArrowScript = humanArrow.GetComponent<DirectionArrow>();
        robotArrowScript = robotArrow.GetComponent<DirectionArrow>();
    }
    void Update()
    {
        humanMove = humanInput.playerMovement;
        robotMove = robotInput.playerMovement;

        bool checkOne = humanMove != Vector3.zero && robotMove != Vector3.zero;
        bool checkTwo = (humanMove + robotMove).magnitude == 0;

        if (rb.velocity.magnitude >= 0 && rb.velocity.magnitude <= 0.01f && checkTwo) //Stalemate
        {
            humanArrow.SetActive(true);
            robotArrow.SetActive(true);

            humanArrowScript.RotateArrow(Vector3.SignedAngle(-transform.right, humanIn.Movement, Vector3.left));
            robotArrowScript.RotateArrow(Vector3.SignedAngle(transform.right, robotIn.Movement, Vector3.left));
        }
        else
        {
            humanArrow.SetActive(false);
            robotArrow.SetActive(false);
        }
    }
}
