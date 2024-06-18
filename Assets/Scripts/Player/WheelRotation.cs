using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    public Rigidbody playerRigidbody;
    public float rotationSpeed = 100f;

    private float _playerSpeed, _wheelRotationSpeed;

    // Update is called once per frame
    void LateUpdate()
    {
        if (playerRigidbody.velocity != Vector3.zero && playerRigidbody != null)
        {
            // Calculate the speed of the player in the forward direction
            _playerSpeed = Vector3.Dot(playerRigidbody.velocity, playerRigidbody.transform.forward);

            // Calculate the rotation speed of the wheel based on the player's speed
            _wheelRotationSpeed = _playerSpeed * rotationSpeed * Time.deltaTime;

            // Apply rotation to the wheel
            transform.Rotate(Vector3.right, _wheelRotationSpeed);
        }
    }
}
