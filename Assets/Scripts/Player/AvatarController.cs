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
    private float _energyRegeneration = 0;
    private Coroutine _energyRoutine;
    private PointsManager _pointsManager;
    private ControlPoints _controlPoints;
    private List<Avatar> _controlledAvatars;

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
        _controlPoints = GetComponent<ControlPoints>();

        _currentEnergy = maxEnergy;

        _pointsManager = transform.parent.GetComponent<PointsManager>();

        _controlledAvatars = _pointsManager.PlayersAvatars[playerNumber];

        _controlPointSlot = playerNumber == 0 ? 1 : 0;
        _controlPoints.PointDestination(_controlPointSlot);
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentEnergy < maxEnergy)
        {
            Debug.Log("Repleneshing Energy. Rate: " + _energyRegeneration);
            _currentEnergy += _energyRegeneration;
            RefreshEnergyBarTrigger?.Invoke(playerNumber, _currentEnergy, maxEnergy);

        }
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
            Collider[] objectsAroundPlayer = Physics.OverlapSphere(transform.position, destructionRadius);

            foreach (Collider collider in objectsAroundPlayer)
            {
                // Destruction logic goes here. Have to check whether it's a powerup or not
                if (collider.tag == "CollectibleTwo" && playerNumber == 0)
                {
                    collider.GetComponent<Collectible>().ReleaseCollectible();
                    DepleteEnergy(true);
                }
                if (collider.tag == "CollectibleOne" && playerNumber == 1)
                {
                    collider.GetComponent<Collectible>().ReleaseCollectible();
                    DepleteEnergy(true);
                }
            }
        }
    }

    private void HamperPlayer()
    {
        if (_currentEnergy >= maxEnergy / _poweredUpMultiplier)
        {
            DepleteEnergy(true);
            otherPlayer.GetHampered();
        }
    }

    private void ToggleControlPointSlot()
    {
        //Check which avatars are locked
        _controlledAvatars = _pointsManager.PlayersAvatars[playerNumber];
        // Every press incerments the slider, but when it reaches 3, it goes back to 0
        _controlPointSlot = (_controlPointSlot + 1) % 3;
        foreach (Avatar avatar in _controlledAvatars)
        {
            if (_controlPointSlot == avatar.AvatarNumber && !avatar.CheckPickableAvatar(playerNumber))
            {
                _controlPointSlot = (_controlPointSlot + 1) % 3;
            }
        }
        
        _controlPoints.PointDestination(_controlPointSlot);

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
        _poweredUpMultiplier = _controlledAvatars.Count;

        maxEnergy *= _poweredUpMultiplier;
    }

    private void DepleteEnergy(bool immediately)
    {
        _energyRegeneration = 0;

        if (_energyRoutine != null) StopCoroutine(_energyRoutine);

        if (immediately)
        {
            _currentEnergy -= maxEnergy / _poweredUpMultiplier;
        }
        else
        {
            _currentEnergy -= moveEnergyConsumption;
        }

        _energyRoutine = StartCoroutine(WaitTillEnergyReplenishes(energyCooldown));
        
        RefreshEnergyBarTrigger?.Invoke(playerNumber, _currentEnergy, maxEnergy);
    }

    private void AvatarNumberChanged(int player)
    {
        if (player == playerNumber)
        {
            _controlledAvatars = _pointsManager.PlayersAvatars[playerNumber];
            Debug.Log("Player " + playerNumber + "'s avatars are now " + _controlledAvatars.Count);

            TogglePowerupState();

            foreach (Avatar av in _controlledAvatars)
            {
                if (av.AvatarNumber == _controlPointSlot)
                {
                    ToggleControlPointSlot();
                }
            }
        }
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

        PointsManager.PlayerAvatarsChanged += AvatarNumberChanged;
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

        PointsManager.PlayerAvatarsChanged += AvatarNumberChanged;
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
