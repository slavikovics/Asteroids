using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float offScreenBuffer = 2f;

    public event Action AsteroidDestroyed;

    private Camera _gameCamera;

    private Animator _animator;

    private PolygonCollider2D _collider;

    private Rigidbody2D _rigidbody;

    private Vector2 _targetPosition;

    private bool _wasExploaded = false;

    public void Start()
    {
        _gameCamera = Camera.main;
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<PolygonCollider2D>();
        _animator = GetComponent<Animator>();

        float scale = Random.Range(1f, 4f);
        transform.localScale = new Vector3(scale, scale, scale);

        _rigidbody.mass = (float) Math.Pow(scale, 2) / 4; 
        
        transform.position = RandomOffScreenPosition();
        SetRandomTargetPosition();
        Vector2 forceDirection = Random.insideUnitCircle.normalized;
        _rigidbody.AddForce(forceDirection * Random.Range(2f, 8f), ForceMode2D.Impulse);
    }

    void Update()
    {
        if (IsOffScreen())
        {
            DestroyOffScreen();
        }
    }

    void SetRandomTargetPosition()
    {
        _targetPosition = RandomOffScreenPosition();
    }

    Vector2 RandomOffScreenPosition()
    {
        float screenWidth = _gameCamera.orthographicSize * _gameCamera.aspect + offScreenBuffer;
        float screenHeight = _gameCamera.orthographicSize + offScreenBuffer;

        float x, y;
        if (Random.value < 0.5f) 
        {
            x = Random.Range(-screenWidth, screenWidth);
            y = Random.value < 0.5f ? -screenHeight : screenHeight;
        }
        else
        {
            x = Random.value < 0.5f ? -screenWidth : screenWidth;
            y = Random.Range(-screenHeight, screenHeight);
        }

        return new Vector2(x, y);
    }

    private bool IsOffScreen()
    {
        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2;
        float screenHeight = Camera.main.orthographicSize * 2;

        Vector3 position = transform.position;

        return position.x < -screenWidth / 2 - offScreenBuffer ||
               position.x > screenWidth / 2 + offScreenBuffer ||
               position.y < -screenHeight / 2 - offScreenBuffer ||
               position.y > screenHeight / 2 + offScreenBuffer;
    }

    void DestroyAsteroid()
    {
        _animator.SetBool("IsExploaded", true);
        StartCoroutine(DestroyObjectAfterDelay());
    }

    private void DestroyOffScreen()
    {
        Destroy(gameObject);
        AsteroidDestroyed?.Invoke();
    }

    private IEnumerator DestroyObjectAfterDelay()
    {
        _rigidbody.angularVelocity = 0f;
        _rigidbody.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
        AsteroidDestroyed?.Invoke();
        AsteroidDestroyed = null;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.attachedRigidbody.bodyType == RigidbodyType2D.Static) DestroyAsteroid();
        if(collision.gameObject.name.Contains("Laser"))
        {
            DestroyAsteroid();
            EventManager.AsteroidDestroyedByLaser?.Invoke();
        }
    }
}
