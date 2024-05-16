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
            try
            {
                agent.destination = target.transform.position;
            }
            catch
            {
                agent.gameObject.transform.position = target.transform.position + new Vector3(0,10, 0);
            }

        }
    }
}
