using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool _poweredUp; // This is Avatar 3
    private Rigidbody _rb;
    private int _controlPointSlot = 0;
    private float _currentEnergy;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _currentEnergy = maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        _movement = inputScript.Movement;
        if (_currentEnergy != 0 && _movement != Vector3.zero)
        {
            DepleteEnergy(false); // Gradually depleting energy bar
            if (_isHampered)
            {
                _rb.AddForce(_movement*hamperPercentage * speed);
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
        if (_currentEnergy != 0)
        {
            DepleteEnergy(true);
            Collider[] objectsAroundPlayer = Physics.OverlapSphere(transform.position, destructionRadius);

            foreach (Collider collider in objectsAroundPlayer)
            {
                // Destruction logic goes here. Have to check whether it's a powerup or not
            }
        }
    }

    private void HamperPlayer()
    {
        if (_currentEnergy != 0)
        {
            DepleteEnergy(true);
            otherPlayer.GetHampered();
        }
    }

    private void ToggleControlPointSlot()
    {
        // Every press incerments the slider, but when it reaches 3, it goes back to 0
        _controlPointSlot = (_controlPointSlot + 1) % 3;
    }

    #endregion

    #region Utility Methods
    public void GetHampered()
    {
        CooldownCountdown(hamperCooldown, true, _isHampered);
    }
    private void TogglePowerupState() // This is for gaining or losing Avatar 3
    {

    }

    private void DepleteEnergy(bool immediately)
    {
        if (immediately)
        {
            _currentEnergy = 0;
        }
        else
        {
            _currentEnergy -= moveEnergyConsumption;
        }

        if (_currentEnergy == 0)
        {
            CooldownCountdown(energyCooldown, false, false);
        }
    }
    IEnumerator CooldownCountdown(float duration, bool toggling, bool variableToToggle)
    {
        variableToToggle = !variableToToggle;
        yield return new WaitForSeconds(duration);
        variableToToggle = !variableToToggle;

        if (!toggling)
        {
            _currentEnergy = maxEnergy;
        }
    }
    #endregion

    #region Collision Handling
    #endregion

    #region Subscribing To Events
    private void OnEnable()
    {
        if (playerNumber == 1)
        {
            PlayerInput.DestroyP1 += DestroyPowerup;
            PlayerInput.HamperP1 += HamperPlayer;
            PlayerInput.ToggleControlPointP1 += ToggleControlPointSlot;
        }
        else if (playerNumber == 2)
        {
            PlayerInput.DestroyP2 += DestroyPowerup;
            PlayerInput.HamperP2 += HamperPlayer;
            PlayerInput.ToggleControlPointP2 += ToggleControlPointSlot;
        }
    }
    private void OnDisable()
    {
        if (playerNumber == 1)
        {
            PlayerInput.DestroyP1 -= DestroyPowerup;
            PlayerInput.HamperP1 -= HamperPlayer;
            PlayerInput.ToggleControlPointP1 -= ToggleControlPointSlot;
        }
        else if (playerNumber == 2)
        {
            PlayerInput.DestroyP2 -= DestroyPowerup;
            PlayerInput.HamperP2 -= HamperPlayer;
            PlayerInput.ToggleControlPointP2 -= ToggleControlPointSlot;
        }
    }

    #endregion
}
