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
    private int player;
    private DecalProjector projector;
    private void Start()
    {
        radius = GetComponent<AvatarController>().DestructionRadius;
        player = GetComponent<AvatarController>().PlayerNbr;
        projector = destroyCircle.GetComponent<DecalProjector>();

        projector.size.Set(radius*2, radius*2, projector.size.y);
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
