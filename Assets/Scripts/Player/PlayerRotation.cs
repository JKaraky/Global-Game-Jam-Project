using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [Header("Rotation Speeds")]
    [SerializeField]
    private float movementRotationSpeed = 0.1f;
    [SerializeField]
    private float rotationSpeed = 5f;

    private Transform _playerObj;
    private Quaternion _targetRotation;
    private Camera _mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        _playerObj = transform;

        //_targetRotation = transform.rotation;

        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        _playerObj.rotation = Quaternion.Slerp(_playerObj.rotation, _targetRotation, movementRotationSpeed);
    }

    public void RotateTowardsCamera()
    {
        // Look away from the camera (same direction as it's pointing)

        // Get the position of the camera
        Vector3 cameraPosition = _mainCamera.transform.position;

        // Calculate the direction from the player to the camera
        Vector3 directionToCamera = _playerObj.position - cameraPosition;

        // Ignore the y component of the direction
        directionToCamera.y = 0;

        RotateTowards(directionToCamera);
    }

    public void RotateTowards(Vector3 rotationValue)
    {
        Quaternion rotation = Quaternion.LookRotation(rotationValue);

        _targetRotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

        //Debug.Log("Rotating: " + _targetRotation);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with " + collision.gameObject.name);
    }
}
