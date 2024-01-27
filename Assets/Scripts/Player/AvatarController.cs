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

    private int _poweredUpMultiplier = 1;


    [Header("Movement")]
    [SerializeField]
    private float speed = 17;
    [SerializeField]
    private PlayerInput inputScript;

    [Header("Destroying Enemy Powerup")]
    [SerializeField]
    private float destructionRadius;

    [Header("Hamper Enemy Player Movement")]
    [SerializeField]
    private float hamperPercentage;
    [SerializeField]
    private float hamperCooldown; // How long does a player stay hampered

    private bool _isHampered = false;

    [Header("Enemy Player Reference")]
    [SerializeField]
    private AvatarController otherPlayer;

    private Vector3 _movement;
    private Rigidbody _rb;
    private int _controlPointSlot = 0;
    private float _currentEnergy;
    private PointsManager _pointsManger;

    #endregion
    #region Events
    public static event Action<int, float, float> RefreshEnergyBarTrigger;
    public static event Action<int, int> ControlSlotToggleTrigger;
    #endregion
    public int PlayerNbr
    {
        get
        {
            return playerNumber;
        }
    }
    public int ControlPointSlot
    {
        get
        {
            return _controlPointSlot;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _rb = transform.parent.GetComponent<Rigidbody>();

        _currentEnergy = maxEnergy;

        _pointsManger = transform.parent.GetComponent<PointsManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        _movement = inputScript.Movement;
        if (_currentEnergy > 0 && _movement != Vector3.zero)
        {
            DepleteEnergy(false); // Gradually depleting energy bar
            if (_isHampered)
            {
                _rb.AddForce(_movement*hamperPercentage/100 * speed);
            }
            else
            {
                _rb.AddForce(_movement * speed);
            }
        }
    }
    #region Player Actions

    void DestroyPowerup()
    {
        if (_currentEnergy >= maxEnergy/_poweredUpMultiplier)
        {
            DepleteEnergy(true);
            Collider[] objectsAroundPlayer = Physics.OverlapSphere(transform.position, destructionRadius);

            foreach (Collider collider in objectsAroundPlayer)
            {
                // Destruction logic goes here. Have to check whether it's a powerup or not
                if (collider.tag == "CollectibleTwo" && playerNumber == 0)
                {
                    collider.GetComponent<Collectible>().ReleaseCollectible();
                }
                if (collider.tag == "CollectibleOne" && playerNumber == 1)
                {
                    collider.GetComponent<Collectible>().ReleaseCollectible();
                }
            }

            Debug.Log("Player " + (playerNumber + 1) + " destroyed  powerup");
        }
    }

    private void HamperPlayer()
    {
        if (_currentEnergy >= maxEnergy / _poweredUpMultiplier)
        {
            DepleteEnergy(true);
            otherPlayer.GetHampered();

            Debug.Log("Player " + (playerNumber + 1) + " hampered other player");
        }
    }

    private void ToggleControlPointSlot()
    {
        // Every press incerments the slider, but when it reaches 3, it goes back to 0
        _controlPointSlot = (_controlPointSlot + 1) % 3;

        ControlSlotToggleTrigger?.Invoke(_controlPointSlot, PlayerNbr);
    }

    #endregion

    #region Utility Methods
    public void GetHampered()
    {
        StartCoroutine(CooldownCountdown(hamperCooldown, true, _isHampered));
    }
    private void TogglePowerupState() // This is for gaining or losing Avatar 3
    {
        maxEnergy /= _poweredUpMultiplier;

        // Get new multiplier from nbr of controlles avatars

        maxEnergy *= _poweredUpMultiplier;
    }

    private void DepleteEnergy(bool immediately)
    {
        if (immediately)
        {
            _currentEnergy -= maxEnergy / _poweredUpMultiplier;
        }
        else
        {
            _currentEnergy -= moveEnergyConsumption;
        }

        if (_currentEnergy <= 0)
        {
            StartCoroutine(CooldownCountdown(energyCooldown, false, false));
        }

        RefreshEnergyBarTrigger?.Invoke(playerNumber, _currentEnergy, maxEnergy);
    }
    IEnumerator CooldownCountdown(float duration, bool toggling, bool variableToToggle)
    {
        variableToToggle = !variableToToggle;
        yield return new WaitForSeconds(duration);
        variableToToggle = !variableToToggle;

        if (!toggling)
        {
            _currentEnergy = maxEnergy;
            RefreshEnergyBarTrigger?.Invoke(playerNumber, _currentEnergy, maxEnergy);
        }
    }
    #endregion

    #region Subscribing To Events
    private void OnEnable()
    {
        if (playerNumber == 0)
        {
            PlayerInput.DestroyP1 += DestroyPowerup;
            PlayerInput.HamperP1 += HamperPlayer;
            PlayerInput.ToggleControlPointP1 += ToggleControlPointSlot;
        }
        else if (playerNumber == 1)
        {
            PlayerInput.DestroyP2 += DestroyPowerup;
            PlayerInput.HamperP2 += HamperPlayer;
            PlayerInput.ToggleControlPointP2 += ToggleControlPointSlot;
        }
    }
    private void OnDisable()
    {
        if (playerNumber == 0)
        {
            PlayerInput.DestroyP1 -= DestroyPowerup;
            PlayerInput.HamperP1 -= HamperPlayer;
            PlayerInput.ToggleControlPointP1 -= ToggleControlPointSlot;
        }
        else if (playerNumber == 1)
        {
            PlayerInput.DestroyP2 -= DestroyPowerup;
            PlayerInput.HamperP2 -= HamperPlayer;
            PlayerInput.ToggleControlPointP2 -= ToggleControlPointSlot;
        }
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
