using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [Header("Rotation Speeds")]
    [SerializeField]
    private float movementRotationSpeed = 0.1f;
    private float _angleDiff;

    private Transform _playerObj;
    private Quaternion _targetRotation, _temp;
    private Camera _mainCamera;
    private Vector3 _cameraPosition, _directionToCamera;
    // Start is called before the first frame update
    void Start()
    {
        _playerObj = transform;

        //_targetRotation = transform.rotation;

        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _angleDiff = Quaternion.Angle(_playerObj.rotation, _targetRotation);

        // If the angle difference is small, snap to the target rotation
        if (_angleDiff < 0.1f)
        {
            _playerObj.rotation = _targetRotation;
        }
        else
        {
            _playerObj.rotation = Quaternion.Slerp(_playerObj.rotation, _targetRotation, Mathf.Clamp01(movementRotationSpeed * Time.deltaTime));
        }
    }

    //public void RotateTowardsCamera()
    //{
    //    // Look away from the camera (same direction as it's pointing)

    //    // Get the position of the camera
    //    _cameraPosition = _mainCamera.transform.position;

    //    // Calculate the direction from the player to the camera
    //    _directionToCamera = _playerObj.position - _cameraPosition;

    //    // Ignore the y component of the direction
    //    _directionToCamera.y = 0;

    //    RotateTowards(_directionToCamera);
    //}

    public void RotateTowards(Vector3 rotationValue)
    {
        rotationValue = rotationValue.normalized;
        _temp = Quaternion.LookRotation(rotationValue);

        if (Quaternion.Angle(_playerObj.rotation, _temp) > 0.1f)
            _targetRotation = Quaternion.Euler(0, _temp.eulerAngles.y, 0);
    }
}
