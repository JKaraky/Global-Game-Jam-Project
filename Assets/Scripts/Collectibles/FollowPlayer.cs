using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowPlayer : MonoBehaviour
{
    #region Variables
    private NavMeshAgent agent;
    private GameObject target;

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
        agent.destination = target.transform.position;
    }
}
