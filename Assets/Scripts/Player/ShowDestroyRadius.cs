using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShowDestroyRadius : MonoBehaviour
{
    [SerializeField]
    private GameObject destroyCircle;
    //[SerializeField]
    //private CannonRotation cannonScript;

    private float radius;
    private float circleRadius;
    private float playerScale;
    private int player;

    //private string targetTag;
    //private bool checkForCollissions = false;
    //private DecalProjector projector;
    private void Start()
    {
        radius = transform.parent.GetComponent<PlayerAttributes>().destructionRadius;
        player = GetComponent<AvatarController>().PlayerNbr;

        //targetTag = player == 0 ? "CollectibleTwo" : "CollectibleOne";

        // Get plaeyr scale so we can now how to accurately size the destruction radius
        playerScale = transform.parent.localScale.x;

        // Setting the radius of the circle. Divide by 0.5 because that's the radius of its collider by default
        circleRadius = radius / 0.5f / playerScale;
        destroyCircle.transform.localScale = new Vector3(circleRadius, circleRadius, circleRadius);
    }
    //private void Update()
    //{
    //    if (checkForCollissions)
    //    {
    //        Collider[] objectsAroundPlayer = Physics.OverlapSphere(transform.position, radius);

    //        foreach (Collider collider in objectsAroundPlayer)
    //        {
    //            if (collider.tag == targetTag)
    //            {
    //                cannonScript.SetTarget(collider.gameObject.transform);
    //            }
    //        }
    //    }
    //}


    private void ShowOrHideDestructionCircle(int playerNB, bool activate)
    {
        if (player == playerNB)
        {
            destroyCircle.SetActive(activate);
            //if (destroyCircle.activeSelf)
            //{
            //    checkForCollissions = true;
            //}
            //else
            //    checkForCollissions= false;
        }
    }
    private void OnEnable()
    {
        SimplifiedInput.ShowOrDestroyRadius += ShowOrHideDestructionCircle;
    }
    private void OnDisable()
    {
        SimplifiedInput.ShowOrDestroyRadius -= ShowOrHideDestructionCircle;
    }
}
