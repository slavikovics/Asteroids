using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class SpaceShip : MonoBehaviour
{
    [Header("References")]
    
    [FormerlySerializedAs("DefaultLaserPrefab")] public GameObject defaultLaserPrefab;

    [FormerlySerializedAs("HyperAttackLaserPrefab")] public GameObject hyperAttackLaserPrefab;

    [FormerlySerializedAs("ShieldPrefab")] public GameObject shieldPrefab;

    [FormerlySerializedAs("TimeSlowdown")] public GameObject timeSlowdown;

    [FormerlySerializedAs("RotationSpeed")] public float rotationSpeed;
    
    [FormerlySerializedAs("TotalLives")] [Header("Settings")]
    
    public int totalLives;

    [FormerlySerializedAs("ShieldDelay")] public float shieldDelay;

    [FormerlySerializedAs("ShieldDuration")] public float shieldDuration;

    [FormerlySerializedAs("HyperAttackDelay")] public float hyperAttackDelay;

    [FormerlySerializedAs("TeleportDelay")] public float teleportDelay;

    [FormerlySerializedAs("SlowdownDelay")] public float slowdownDelay;

    private bool _hyperAttackAvailable = true; 

    private bool _slowdownAvailable = true;

    private bool _shieldAvailable = true;

    private bool _teleportAvailable = true;

    private GameObject? _currentShield = null;

    private bool _trackCollisions = false;

    private float _rotationInput;

    private Vector3 _initialPosition;

    private InputMaster _input;

    [Header("Teleportation Boundaries")]

    public float offsetLeft = 0.1f;

    public float offsetRight = 0.1f;

    public float offsetTop = 0.1f;

    public float offsetBottom = 0.1f;

    private void Awake()
    {
        _input = new InputMaster();
        SetUpInputs();
    }

    private void SetUpInputs()
    {
        _initialPosition = transform.position;
        _input.Enable();
        _input.Player.Shoot.performed += Shoot;
        _input.Player.HyperAttack.performed += HyperAttack;
        _input.Player.Rotate.performed += ctx => _rotationInput = ctx.ReadValue<float>();
        _input.Player.Rotate.canceled += ctx => _rotationInput = 0;
        _input.Player.Shield.performed += AddShield;
        _input.Player.Slowdown.performed += Slowdown;

        _input.Player.Teleport.performed += (InputAction.CallbackContext context) => 
        { 
            if (_teleportAvailable) StartCoroutine(TeleportToRandomPosition()); 
        };
    }

    private Vector2 GetRandomPositionInBounds()
    {
        float minX = _initialPosition.x - offsetLeft;
        float maxX = _initialPosition.x + offsetRight;
        float minY = _initialPosition.y - offsetBottom;
        float maxY = _initialPosition.y + offsetTop;

        return new Vector2(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY)
        );
    }

    public IEnumerator TeleportToRandomPosition()
    {
        _teleportAvailable = false;
        StartCoroutine(MakeTeleportAvailable());

        var animator = GetComponent<Animator>();
        animator.Play("Teleport", 0, 0f);

        yield return new WaitForSeconds(0.1f);
        var position = GetRandomPositionInBounds();
        transform.position = position;
        EventManager.TeleportApplied?.Invoke(teleportDelay);
    }

    private void Destruction()
    {
        _input.Disable();
        if(_currentShield != null) Destroy(_currentShield);

        var animator = GetComponent<Animator>();
        animator.SetBool("Destruction", true);
    }

    private void Start()
    {
        StartCoroutine(TrackCollisionsWithAsteroids());
    }

    private void Update()
    {
        if (_rotationInput != 0) Rotate(_rotationInput);
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        Debug.Log("Shooting performed");
        var laser = Instantiate(defaultLaserPrefab);
        EventManager.LaserShoot?.Invoke();
        laser.GetComponent<Laser>().Initialize(transform.rotation, transform.position);
    }

    private void Slowdown(InputAction.CallbackContext context)
    {
        if (!_slowdownAvailable) return;

        EventManager.SlowdownEffectApplied?.Invoke(slowdownDelay);
        var slowdown = timeSlowdown.GetComponent<TimeSlowdown>();
        StartCoroutine(slowdown.SlowTimeSmoothly());
        _slowdownAvailable = false;
        StartCoroutine(MakeSlowdownAvailable());
    }

    private void HyperAttack(InputAction.CallbackContext context)
    {
        if (!_hyperAttackAvailable) return;
        Debug.Log("HyperAttackPerformed");
        int number = 20;

        for (int i = 0; i < number; i++)
        {
            var hyperLaser = Instantiate(hyperAttackLaserPrefab);
            var angle = 360f / number * i;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            hyperLaser.GetComponent<BigLaser>().Initialize(rotation, transform.position);
        }

        _hyperAttackAvailable = false;
        EventManager.HyperAttackUsed?.Invoke(hyperAttackDelay);
        StartCoroutine(MakeHyperAttackAvailable());
    }

    private void AddShield(InputAction.CallbackContext context)
    {
        if (!_shieldAvailable) return;

        _currentShield = Instantiate(shieldPrefab);
        _currentShield.GetComponent<Shield>().Initialize(transform.position);
        _shieldAvailable = false;

        Debug.Log("Shield added");
        EventManager.ShieldUsed?.Invoke(shieldDuration + shieldDelay);

        _currentShield.transform.SetParent(this.transform);
        StartCoroutine(RemoveShield(_currentShield));
    }

    private IEnumerator MakeHyperAttackAvailable()
    {
        yield return new WaitForSeconds(hyperAttackDelay);
        _hyperAttackAvailable = true;
    }

    private IEnumerator MakeSlowdownAvailable()
    {
        yield return new WaitForSeconds(slowdownDelay);
        _slowdownAvailable = true;
    }

    private IEnumerator RemoveShield(GameObject shield)
    {
        yield return new WaitForSeconds(shieldDuration);
        Debug.Log("Shield removed");
        Destroy(shield);
        shield = null;
        StartCoroutine(MakeShieldAvailable());
    }

    private IEnumerator MakeShieldAvailable()
    {
        yield return new WaitForSeconds(shieldDelay);
        _shieldAvailable = true;
    }

    private IEnumerator MakeTeleportAvailable()
    {
        yield return new WaitForSeconds(teleportDelay);
        _teleportAvailable = true;
    }

    private void Rotate(float direction)
    {
        transform.Rotate(0, 0, direction * rotationSpeed * Time.deltaTime);
    }

    private IEnumerator TrackCollisionsWithAsteroids()
    {
        yield return new WaitForSeconds(1f);
        _trackCollisions = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_trackCollisions) return;
        totalLives--;

        EventManager.AsteroidCollidedWithSpaceShip?.Invoke();
        if (totalLives == 0) Destruction();
    }
}
