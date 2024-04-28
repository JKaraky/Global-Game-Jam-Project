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
    private GameObject cannonTip;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    [Tooltip("The ratio of max energy that this action will use. For example, 1 is all energy, 2 is half, etc...")]
    private int destructionEnergyConsumptionRatio = 1;

    [Header("Jam Canon")]
    [SerializeField]
    private float jamCooldown; // How long does a player stay hampered
    [SerializeField]
    private GameObject jamParticles;
    [SerializeField]
    [Tooltip("The ratio of max energy that this action will use. For example, 1 is all energy, 2 is half, etc...")]
    private int jamEnergyConsumptionRatio = 3;
    private bool _isJammed = false;

    [Header("Enemy Player Reference")]
    [SerializeField]
    private AvatarController otherPlayer;

    private Transform playerObj;
    private string _targetTag;
    private PlayerRotation rotationScript;
    private Vector3 _movement;
    private Rigidbody _rb;
    private float _currentEnergy;
    private float _energyRegeneration = 0;
    private Coroutine _energyRoutine;

    #endregion
    #region Events
    public static event Action<int, float, float> RefreshEnergyBarTrigger;
    public static event Action JammedCannon;
    public static event Action FiredCannon;
    public static event Action<int, bool> toggleJamIcon;
    public static event Action<int, bool> toggleCannonIcon;
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
        rotationScript = playerObj.GetComponent<PlayerRotation>();

        _rb = playerObj.GetComponent<Rigidbody>();

        _currentEnergy = maxEnergy;
        _targetTag = playerNumber == 0 ? "CollectibleTwo" : "CollectibleOne";
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentEnergy < maxEnergy)
        {
            _currentEnergy += _energyRegeneration;
            _currentEnergy = (float)Math.Clamp(Math.Round(_currentEnergy, 1, MidpointRounding.AwayFromZero), 0, maxEnergy);
            RefreshEnergyBarTrigger?.Invoke(playerNumber, _currentEnergy, maxEnergy);

            ActionIconHandler();
        }
    }
    private void FixedUpdate()
    {
        // Simulating gravity
        _rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);

        AttemptMovement();
    }
    #region Player Actions

    private void AttemptMovement()
    {
        _movement = Move();
        if (_movement != Vector3.zero && !_isJammed && _currentEnergy >= moveEnergyConsumption)
        {
            DepleteEnergy(0); // Gradually depleting energy bar

            _rb.AddForce(_movement * speed);

            Rotate(_movement);
        }
    }
    private Vector3 Move()
    {
        // Get the forward direction of the camera
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f; // Zero out the y component to ensure movement is in the horizontal plane
        
        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0f;

        // Combine the forward and right directions based on input
        Vector3 movementDirection = (cameraForward * inputScript.Movement.y + cameraRight * inputScript.Movement.x).normalized;
        //Vector3 movementDirection = (cameraForward * inputScript.Movement.y).normalized;

        // Set the movement vector
        return new Vector3(movementDirection.x, 0, movementDirection.z);
    }

    private void Rotate(Vector3 rotationVector)
    {
        rotationScript.RotateTowards(rotationVector);
    }

    public void DestroyPowerup()
    {
        if (!_isJammed && _currentEnergy >= maxEnergy)
        {
            Collider[] objectsAroundPlayer = Physics.OverlapSphere(transform.position, destructionRadius);

            foreach (Collider collider in objectsAroundPlayer)
            {
                if (collider.tag == _targetTag)
                {
                    explosion.transform.localPosition = cannonTip.transform.localPosition;
                    explosion.SetActive(true);

                    collider.GetComponent<Collectible>().DestroyCollectible();
                    DepleteEnergy(destructionEnergyConsumptionRatio);

                    FiredCannon?.Invoke();
                    toggleCannonIcon?.Invoke(playerNumber, false);
                }
            }
        }
    }

    public void JamPlayer()
    {
        if (_currentEnergy >= maxEnergy/3)
        {
            DepleteEnergy(jamEnergyConsumptionRatio);
            otherPlayer.GetJammed();
            JammedCannon?.Invoke();
            toggleJamIcon?.Invoke(playerNumber, false);
        }
    }

    #endregion

    #region Utility Methods

    private void RotateTowardsCamera()
    {
        rotationScript.RotateTowardsCamera();
    }
    public void GetJammed()
    {
        StartCoroutine(CooldownCountdown(jamCooldown, true, _isJammed, jamParticles));
    }

    private void ActionIconHandler()
    {
        toggleCannonIcon?.Invoke(playerNumber, _currentEnergy >= maxEnergy / destructionEnergyConsumptionRatio);
        toggleJamIcon?.Invoke(playerNumber, _currentEnergy >= maxEnergy / jamEnergyConsumptionRatio);
    }

    private void DepleteEnergy(int energyDepletionRatio)
    {
        _energyRegeneration = 0;

        if (_energyRoutine != null) StopCoroutine(_energyRoutine);

        if (energyDepletionRatio != 0)
        {
            _currentEnergy -= maxEnergy/energyDepletionRatio;
        }
        else
        {
            _currentEnergy -= moveEnergyConsumption;
        }

        _currentEnergy = (float)Math.Clamp(Math.Round(_currentEnergy, 1, MidpointRounding.AwayFromZero), 0, maxEnergy);

        _energyRoutine = StartCoroutine(WaitTillEnergyReplenishes(energyCooldown));
        
        RefreshEnergyBarTrigger?.Invoke(playerNumber, _currentEnergy, maxEnergy);
    }

    private void ToggleParticles(GameObject particles)
    {
        particles.SetActive(!particles.activeSelf);
    }
    IEnumerator CooldownCountdown(float duration, bool toggling, bool variableToToggle, GameObject particles)
    {
        ToggleParticles(particles);
        variableToToggle = !variableToToggle;
        yield return new WaitForSeconds(duration);
        variableToToggle = !variableToToggle;
        ToggleParticles(particles);
    }
    IEnumerator WaitTillEnergyReplenishes(float duration)
    {
        yield return new WaitForSeconds(duration);
        _energyRegeneration = energyRegenerationRate;
    }
    #endregion
    private void OnDrawGizmosSelected()
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
