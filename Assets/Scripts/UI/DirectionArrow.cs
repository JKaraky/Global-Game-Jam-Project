using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DirectionArrow : MonoBehaviour
{
    [SerializeField]
    private Transform objectToRotateAround;
    [SerializeField]
    private float angle;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    [Tooltip("1 if it's on the right, -1 if it's on the left")]
    private int direction;

    public void RotateArrow(float angle)
    {
        this.angle = angle % 360;
        if (objectToRotateAround != null)
        {
            transform.localPosition = Quaternion.Euler(0, this.angle, 0) * (objectToRotateAround.localPosition + offset);
            transform.localRotation = Quaternion.Euler(90, 0, -this.angle + 90 * direction);
        }
    }
    private void Update()
    {
        //RotateArrow(angle);
    }
}
