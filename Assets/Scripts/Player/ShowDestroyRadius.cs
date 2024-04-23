using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShowDestroyRadius : MonoBehaviour
{
    [SerializeField]
    private GameObject destroyCircle;

    private float radius;
    private float playerScale;
    private int player;
    //private DecalProjector projector;
    private void Start()
    {
        radius = GetComponent<AvatarController>().DestructionRadius;
        player = GetComponent<AvatarController>().PlayerNbr;

        // Get plaeyr scale so we can now how to accurately size the destruction radius
        playerScale = transform.parent.localScale.x;
        //projector = destroyCircle.GetComponent<DecalProjector>();

        //projector.size.Set(radius*2, radius*2, projector.size.y);

        // Setting the radius of the circle. Divide by 0.5 because that's the radius of its collider by default
        radius = radius / 0.5f / playerScale;
        destroyCircle.transform.localScale = new Vector3(radius, radius, radius);
    }


    private void ShowOrHideDestructionCircle(int playerNB)
    {
        if (player == playerNB) destroyCircle.SetActive(!destroyCircle.activeSelf);
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
