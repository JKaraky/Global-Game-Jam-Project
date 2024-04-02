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

    //private int _poweredUpMultiplier = 1;


    [Header("Movement")]
    [SerializeField]
    private float speed = 17;
    [SerializeField]
    private SimplifiedInput inputScript;

    [Header("Destroying Enemy Powerup")]
    [SerializeField]
    private float destructionRadius;

    [Header("Jam Canon")]
    //[SerializeField]
    //private float hamperPercentage;
    //[SerializeField]
    private float jamCooldown; // How long does a player stay hampered

    private bool _isJammed = false;

    [Header("Enemy Player Reference")]
    [SerializeField]
    private AvatarController otherPlayer;

    private Vector3 _movement;
    private Rigidbody _rb;
    //private int _controlPointSlot = 0;
    private float _currentEnergy;
    private float _energyRegeneration = 0;
    private Coroutine _energyRoutine;
    private PointsManager _pointsManager;
    private ControlPoints _controlPoints;
    //private List<Avatar> _controlledAvatars;

    #endregion
    #region Events
    public static event Action<int, float, float> RefreshEnergyBarTrigger;
    //public static event Action<int, int> ControlSlotToggleTrigger;
    #endregion
    public int PlayerNbr
    {
        get
        {
            return playerNumber;
        }
    }
    //public int ControlPointSlot
    //{
        //get
        //{
            //return _controlPointSlot;
        //}
    //}

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
        _rb = transform.parent.GetComponent<Rigidbody>();
        _controlPoints = GetComponent<ControlPoints>();

        _currentEnergy = maxEnergy;

        //_pointsManager = transform.parent.GetComponent<PointsManager>();

        //_controlledAvatars = _pointsManager.PlayersAvatars[playerNumber];

        //_controlPointSlot = playerNumber == 0 ? 1 : 0;
        //_controlPoints.PointDestination(_controlPointSlot);
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
        _movement = Move();
        if (_movement != Vector3.zero && _currentEnergy >= moveEnergyConsumption)
        {
            DepleteEnergy(false); // Gradually depleting energy bar
            if (_isJammed)
            {
                //_rb.AddForce(_movement*hamperPercentage/100 * speed * Time.deltaTime);
            }
            else
            {
                _rb.AddForce(_movement * speed * Time.deltaTime);
            }

            // Look away from the camera (same direction as it's pointing)
            this.transform.LookAt(transform.position - (Camera.main.transform.position - transform.position));
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
        Debug.Log("Player " + playerNumber + " destroying");
        if (_currentEnergy >= maxEnergy/1)
        {
            Collider[] objectsAroundPlayer = Physics.OverlapSphere(transform.position, destructionRadius);

            foreach (Collider collider in objectsAroundPlayer)
            {
                // Destruction logic goes here. Have to check whether it's a powerup or not
                if (collider.tag == "CollectibleTwo" && playerNumber == 0)
                {
                    collider.GetComponent<Collectible>().DestroyCollectible();
                    DepleteEnergy(true);
                }
                if (collider.tag == "CollectibleOne" && playerNumber == 1)
                {
                    collider.GetComponent<Collectible>().DestroyCollectible();
                    DepleteEnergy(true);
                }
            }
        }
    }

    public void JamPlayer()
    {
        Debug.Log("Player " + playerNumber + " hampering");
        if (_currentEnergy >= maxEnergy / 1)
        {
            DepleteEnergy(true);
            otherPlayer.GetJammed();
        }
    }

    //public void ToggleControlPointSlot()
    //{
    //    Debug.Log("Player " + playerNumber + " toggling");
    //    //Check which avatars are locked
    //    _controlledAvatars = _pointsManager.PlayersAvatars[playerNumber];
    //    // Every press incerments the slider, but when it reaches 3, it goes back to 0
    //    _controlPointSlot = (_controlPointSlot + 1) % 3;
    //    foreach (Avatar avatar in _controlledAvatars)
    //    {
    //        if (_controlPointSlot == avatar.AvatarNumber && !avatar.CheckPickableAvatar(playerNumber))
    //        {
    //            _controlPointSlot = (_controlPointSlot + 1) % 3;
    //        }
    //    }
        
    //    _controlPoints.PointDestination(_controlPointSlot);

    //    ControlSlotToggleTrigger?.Invoke(_controlPointSlot, PlayerNbr);
    //}

    #endregion

    #region Utility Methods
    public void GetJammed()
    {
        StartCoroutine(CooldownCountdown(jamCooldown, true, _isJammed));
    }
    //private void TogglePowerupState() // This is for gaining or losing Avatar 3
    //{
    //    maxEnergy /= _poweredUpMultiplier;

    //    // Get new multiplier from nbr of controlles avatars
    //    _poweredUpMultiplier = _controlledAvatars.Count;

    //    maxEnergy *= _poweredUpMultiplier;
    //}

    private void DepleteEnergy(bool immediately)
    {
        _energyRegeneration = 0;

        if (_energyRoutine != null) StopCoroutine(_energyRoutine);

        if (immediately)
        {
            _currentEnergy -= maxEnergy / 1;
        }
        else
        {
            _currentEnergy -= moveEnergyConsumption;
        }

        _currentEnergy = (float)Math.Clamp(Math.Round(_currentEnergy, 1, MidpointRounding.AwayFromZero), 0, maxEnergy);

        _energyRoutine = StartCoroutine(WaitTillEnergyReplenishes(energyCooldown));
        
        RefreshEnergyBarTrigger?.Invoke(playerNumber, _currentEnergy, maxEnergy);
    }

    //private void AvatarNumberChanged(int player)
    //{
    //    if (player == playerNumber)
    //    {
    //        _controlledAvatars = _pointsManager.PlayersAvatars[playerNumber];
    //        Debug.Log("Player " + playerNumber + "'s avatars are now " + _controlledAvatars.Count);

    //        TogglePowerupState();

    //        foreach (Avatar av in _controlledAvatars)
    //        {
    //            if (av.AvatarNumber == _controlPointSlot)
    //            {
    //                ToggleControlPointSlot();
    //            }
    //        }
    //    }
    //}
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
    //private void OnEnable()
    //{
    //    PointsManager.PlayerAvatarsChanged += AvatarNumberChanged;
    //}
    //private void OnDisable()
    //{
    //    PointsManager.PlayerAvatarsChanged += AvatarNumberChanged;
    //}

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
