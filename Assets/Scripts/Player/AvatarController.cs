using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AvatarController : MonoBehaviour
{
    #region Variables
    [Header("Player 1 or Player 2?")]
    [SerializeField]
    private int playerNumber;

    [Header ("General Settings")]
    [SerializeField]
    private float maxEnergy;
    [SerializeField]
    private float moveEnergyConsumption;
    [SerializeField]
    private float energyCooldown;
    [SerializeField]
    private float energyRegenerationRate;


    [Header("Movement")]
    [SerializeField]
    private float speed = 17;
    [SerializeField]
    private SimplifiedInput inputScript;
    [SerializeField]
    private float gravityMultiplier = 1f;

    [Header("Destroying Enemy Powerup")]
    [SerializeField]
    private float destructionRadius;
    [SerializeField]
    private GameObject cannon;
    [SerializeField]
    private GameObject explosion;

    [Header("Jam Canon")]
    [SerializeField]
    private float jamCooldown; // How long does a player stay hampered

    private bool _isJammed = false;

    [Header("Enemy Player Reference")]
    [SerializeField]
    private AvatarController otherPlayer;

    private Transform playerObj;
    private Vector3 _movement;
    private Rigidbody _rb;
    private float _currentEnergy;
    private float _energyRegeneration = 0;
    private Coroutine _energyRoutine;

    #endregion
    #region Events
    public static event Action<int, float, float> RefreshEnergyBarTrigger;
    #endregion
    public int PlayerNbr
    {
        get
        {
            return playerNumber;
        }
    }

    public float DestructionRadius
    {
        get
        {
            return destructionRadius;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        playerObj = transform.parent;
        _rb = playerObj.GetComponent<Rigidbody>();

        _currentEnergy = maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentEnergy < maxEnergy)
        {
            _currentEnergy += _energyRegeneration;
            _currentEnergy = (float)Math.Clamp(Math.Round(_currentEnergy, 1, MidpointRounding.AwayFromZero), 0, maxEnergy);
            RefreshEnergyBarTrigger?.Invoke(playerNumber, _currentEnergy, maxEnergy);
        }
    }
    private void FixedUpdate()
    {
        // Simulating gravity
        _rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);

        _movement = Move();
        if (_movement != Vector3.zero && _currentEnergy >= moveEnergyConsumption)
        {
            DepleteEnergy(false); // Gradually depleting energy bar

            _rb.AddForce(_movement * speed);

            // Look away from the camera (same direction as it's pointing)
            // Get the position of the camera
            Vector3 cameraPosition = Camera.main.transform.position;

            // Calculate the direction from the player to the camera
            Vector3 directionToCamera = playerObj.position - cameraPosition;

            // Ignore the y component of the direction
            directionToCamera.y = 0;

            // Make the player object rotate only on the y-axis
            Quaternion rotation = Quaternion.LookRotation(directionToCamera);

            // Apply the rotation to the player object, only rotating around the y-axis
            playerObj.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

        }
    }
    #region Player Actions

    private Vector3 Move()
    {
        // Get the forward direction of the camera
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f; // Zero out the y component to ensure movement is in the horizontal plane

        // Get the right direction of the camera
        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0f;

        // Combine the forward and right directions based on input
        Vector3 movementDirection = (cameraForward * inputScript.Movement.y + cameraRight * inputScript.Movement.x).normalized;

        // Set the movement vector
        return new Vector3(movementDirection.x, 0, movementDirection.z);
    }

    public void DestroyPowerup()
    {
        if (!_isJammed && _currentEnergy >= maxEnergy)
        {
            Collider[] objectsAroundPlayer = Physics.OverlapSphere(transform.position, destructionRadius);

            foreach (Collider collider in objectsAroundPlayer)
            {
                // Destruction logic goes here. Have to check whether it's a powerup or not
                if (collider.tag == "CollectibleTwo" && playerNumber == 0)
                {
                    explosion.transform.localPosition = cannon.transform.localPosition;
                    explosion.SetActive(true);

                    Debug.Log("I am " + cannon.gameObject.name);

                    collider.GetComponent<Collectible>().DestroyCollectible();
                    DepleteEnergy(true);
                }
                if (collider.tag == "CollectibleOne" && playerNumber == 1)
                {
                    explosion.transform.position = cannon.transform.position;
                    explosion.SetActive(true);

                    collider.GetComponent<Collectible>().DestroyCollectible();
                    DepleteEnergy(true);
                }
            }
        }
    }

    public void JamPlayer()
    {
        Debug.Log("Player " + playerNumber + " jamming");
        if (_currentEnergy >= maxEnergy)
        {
            DepleteEnergy(true);
            otherPlayer.GetJammed();
        }
    }

    #endregion

    #region Utility Methods
    public void GetJammed()
    {
        StartCoroutine(CooldownCountdown(jamCooldown, true, _isJammed));
    }

    private void DepleteEnergy(bool immediately)
    {
        _energyRegeneration = 0;

        if (_energyRoutine != null) StopCoroutine(_energyRoutine);

        if (immediately)
        {
            _currentEnergy -= maxEnergy;
        }
        else
        {
            _currentEnergy -= moveEnergyConsumption;
        }

        _currentEnergy = (float)Math.Clamp(Math.Round(_currentEnergy, 1, MidpointRounding.AwayFromZero), 0, maxEnergy);

        _energyRoutine = StartCoroutine(WaitTillEnergyReplenishes(energyCooldown));
        
        RefreshEnergyBarTrigger?.Invoke(playerNumber, _currentEnergy, maxEnergy);
    }
    IEnumerator CooldownCountdown(float duration, bool toggling, bool variableToToggle)
    {
        variableToToggle = !variableToToggle;
        yield return new WaitForSeconds(duration);
        variableToToggle = !variableToToggle;
    }
    IEnumerator WaitTillEnergyReplenishes(float duration)
    {
        yield return new WaitForSeconds(duration);
        _energyRegeneration = energyRegenerationRate;
    }
    #endregion
    private void OnDrawGizmos()
    {
        if (playerNumber == 0)
        {
            Gizmos.color = Color.yellow;
        }
        else if (playerNumber == 1)
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(transform.position, destructionRadius);
    }
}
