using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowPlayer : MonoBehaviour
{
    #region Variables
    private NavMeshAgent agent;
    [SerializeField] private GameObject target;

    public GameObject Target { 
        get { 
            return target; 
        } 
        set {
            target = value; 
        } 
    }
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.enabled == true && target != null)
        {
            if (agent.isOnNavMesh)
            {
                agent.destination = target.transform.position;
            }
            else
            {
                RaycastHit hit;
                bool didHit;
                didHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 100f, 1 << 0);
                if (didHit)
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);

                    agent.Warp(hit.point + Vector3.up);
                }
            }

        }
    }
}
