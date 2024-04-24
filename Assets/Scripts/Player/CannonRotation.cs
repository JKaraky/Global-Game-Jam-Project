using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonRotation : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 0.1f;
    [SerializeField]
    private bool flipped = false;
    private Transform nozzleBase;
    private Quaternion headTargetTransform;
    private Quaternion nozzleTargetTransform;
    // Start is called before the first frame update
    void Start()
    {
        nozzleBase = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, headTargetTransform, rotationSpeed);
        nozzleBase.rotation = Quaternion.Slerp(nozzleBase.rotation, nozzleTargetTransform, rotationSpeed);
    }

    public void SetTarget(Transform target)
    {
        // Calculate the direction from the cannon to the target
        Vector3 targetDirection = flipped ? target.position - transform.position : target.position;
        targetDirection.y = 0;

        // Calculate the rotation needed to point at the target
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Extract the pitch (up-down) rotation from the quaternion
        float pitch = Mathf.Asin(2 * (targetRotation.w * targetRotation.y));

        // Convert the pitch angle from radians to degrees
        float pitchDegrees = Mathf.Rad2Deg * pitch;

        // Limit the rotation to strictly up and down
        pitchDegrees = Mathf.Clamp(pitchDegrees, -90f, 90f);

        // Apply the rotation to the cannon barrel
        nozzleTargetTransform = Quaternion.Euler(0, 0, pitchDegrees);
        headTargetTransform = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
    }
}
