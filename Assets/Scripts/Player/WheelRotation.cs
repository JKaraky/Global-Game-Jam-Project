using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    public Rigidbody playerRigidbody;
    public float rotationSpeed = 100f;

    // Update is called once per frame
    void Update()
    {
        if (playerRigidbody != null)
        {
            // Calculate the speed of the player in the forward direction
            float playerSpeed = Vector3.Dot(playerRigidbody.velocity, playerRigidbody.transform.forward);

            // Calculate the rotation speed of the wheel based on the player's speed
            float wheelRotationSpeed = playerSpeed * rotationSpeed * Time.deltaTime;

            // Apply rotation to the wheel
            transform.Rotate(Vector3.right, wheelRotationSpeed);
        }
    }
}
