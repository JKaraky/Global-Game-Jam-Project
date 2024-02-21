using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayerToCamera : MonoBehaviour
{
    private Camera mainCam;
    private float newYRotation;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        newYRotation = mainCam.transform.eulerAngles.y;

        transform.Rotate(0, newYRotation, 0);
    }
}
