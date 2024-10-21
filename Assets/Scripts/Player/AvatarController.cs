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
    private PlayerValues playerAttributesObject;

    private float maxEnergy;
    private float moveEnergyConsumption;
    private float energyCooldown;
    private float energyRegenerationRate;


    [Header("Movement")]
    [SerializeField]
    private SimplifiedInput inputScript;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float gravityMultiplier;

    [Header("Destroying Enemy Powerup")]
    [SerializeField]
    private GameObject cannonTip;
    [SerializeField]
    private GameObject explosion;

    private float destructionRadius;
    private int destructionEnergyConsumptionRatio = 1;

    [Header("Jam Canon")]
    [SerializeField]
    private GameObject jamParticles;

    private float secondaryActionCooldown; // How long does a player stay jammed or boosted
    private int sAEnergyConsumptionRatio = 3; // Secondary Action
    private bool _isJammed = false;

    [Header("Boost")]
    [SerializeField]
    private GameObject boostParticles;

    // true means jam, false means boost
    private bool jamOrBoost;
    // If player choose Boost ability
    private float _speedBoostMultiplier; // multiplies speed by this number

    [Header("Enemy Player Reference")]
    [SerializeField]
    private AvatarController otherPlayer;

    private Transform playerObj;
    private string _targetTag;
    [SerializeField]
    private PlayerRotation rotationScript;
    private Vector3 _movement;
    private Rigidbody _rb;
    private float _currentEnergy;
    private float _energyRegeneration = 0;
    private Coroutine _energyRoutine;

    private Vector3 _cameraForward, _cameraRight, _movementDirection, _moveReturn;

    private Camera _mainCamera;

    // For ShowArrow Script
    public Vector3 playerMovement = Vector3.one;

    #endregion
    #region Events
    public static event Action<int, float, float> RefreshEnergyBarTrigger;
    public static event Action JammedCannon; // Triggers jam audio
    public static event Action BoostSpeed; // Triggers boost audio
    public static event Action FiredCannon;
    public static event Action<int, bool> toggleSecondaryActionIcon;
    public static event Action<int, bool> toggleCannonIcon;
    public static event Action<int, bool> greyOutUI;
    #endregion
    public int PlayerNbr
    {
        get
        {
            return playerNumber;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadVariables();

        playerObj = transform.parent;
        //rotationScript = playerObj.GetComponent<PlayerRotation>();

        _mainCamera = Camera.main;

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

        //AttemptMovement();
    }
    #region Player Actions

    public void AttemptMovement()
    {
        _movement = Move();
        if (_movement != Vector3.zero && !_isJammed && _currentEnergy >= moveEnergyConsumption)
        {
            playerMovement = _movement;

            DepleteEnergy(0); // Gradually depleting energy bar

            _rb.AddForce(_movement * speed);

            Rotate(_movement);
        }
    }
    private Vector3 Move()
    {
        // Get the forward direction of the camera
        _cameraForward = _mainCamera.transform.forward;
        _cameraForward.y = 0f; // Zero out the y component to ensure movement is in the horizontal plane
        
        _cameraRight = _mainCamera.transform.right;
        _cameraRight.y = 0f;

        // Combine the forward and right directions based on input
        _movementDirection = (_cameraForward * inputScript.Movement.y + _cameraRight * inputScript.Movement.x).normalized;
        //Vector3 movementDirection = (cameraForward * inputScript.Movement.y).normalized;

        // Set the movement vector
        _moveReturn.x = _movementDirection.x;
        _moveReturn.y = 0;
        _moveReturn.z = _movementDirection.z;
        return _moveReturn;
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

    // When jamming or boosting
    public void SecondaryActionPressed()
    {
        if (!_isJammed && _currentEnergy >= maxEnergy/sAEnergyConsumptionRatio)
        {
            DepleteEnergy(sAEnergyConsumptionRatio);
            if (jamOrBoost)
            {
                otherPlayer.GetJammed();
                JammedCannon?.Invoke();
            }
            else
            {
                StartCoroutine(BoostCooldownCountdown(secondaryActionCooldown, boostParticles));
                BoostSpeed?.Invoke();
            }
            toggleSecondaryActionIcon?.Invoke(playerNumber, false);
        }
    }

    #endregion

    #region Utility Methods

    private void LoadVariables()
    {
        maxEnergy = playerAttributesObject.maxEnergy;
        moveEnergyConsumption = playerAttributesObject.moveEnergyConsumption;
        energyCooldown = playerAttributesObject.energyCooldown;
        energyRegenerationRate = playerAttributesObject.energyRegenerationRate;

        speed = playerAttributesObject.speed;
        gravityMultiplier = playerAttributesObject.gravityMultiplier;

        destructionRadius = playerAttributesObject.destructionRadius;
        destructionEnergyConsumptionRatio = playerAttributesObject.destructionEnergyConsumptionRatio;

        secondaryActionCooldown = playerAttributesObject.secondaryActionCooldown;
        sAEnergyConsumptionRatio = playerAttributesObject.sAEnergyConsumptionRatio;

        _speedBoostMultiplier = playerAttributesObject.speedBoostMultiplier;
    }
    public void GetJammed()
    {
        StartCoroutine(JamCooldownCountdown(secondaryActionCooldown, jamParticles));
    }

    private void ActionIconHandler()
    {
        toggleCannonIcon?.Invoke(playerNumber, _currentEnergy >= maxEnergy / destructionEnergyConsumptionRatio);
        toggleSecondaryActionIcon?.Invoke(playerNumber, _currentEnergy >= maxEnergy / sAEnergyConsumptionRatio);
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

    private void ToggleParticles(GameObject particles, bool onOrOff)
    {
        particles.SetActive(onOrOff);
    }

    private void GreyOutUI(bool toggle)
    {
        greyOutUI?.Invoke(playerNumber, toggle);
    }

    private void SetSecondaryAbility(bool abilityHuman, bool abilityRobot)
    {
        if (playerNumber == 0)
            jamOrBoost = abilityHuman;
        else
            jamOrBoost = abilityRobot;
    }
    IEnumerator JamCooldownCountdown(float duration, GameObject particles)
    {
        GreyOutUI(true);

        ToggleParticles(particles, true);

        _isJammed = true;
        yield return new WaitForSeconds(duration);
        _isJammed = false;

        GreyOutUI(false);

        ToggleParticles(particles, false);
    }
    IEnumerator BoostCooldownCountdown(float duration, GameObject particles)
    {
        ToggleParticles(particles, true);

        speed *= _speedBoostMultiplier;
        yield return new WaitForSeconds(duration);
        speed /= _speedBoostMultiplier;

        ToggleParticles(particles, false);
    }
    IEnumerator WaitTillEnergyReplenishes(float duration)
    {
        yield return new WaitForSeconds(duration);
        _energyRegeneration = energyRegenerationRate;
    }
    #endregion

#if UNITY_EDITOR
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
#endif
    private void OnEnable()
    {
        DeviceCheck.SetPlayerAbility += SetSecondaryAbility;
    }
    private void OnDisable()
    {
        DeviceCheck.SetPlayerAbility -= SetSecondaryAbility;
    }
}
