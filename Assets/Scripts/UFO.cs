using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class UFO : MonoBehaviour
{
    [FormerlySerializedAs("PlayerToAttack")] [SerializeField]
    public GameObject playerToAttack;

    [FormerlySerializedAs("ShootingDistance")] [SerializeField]
    public float shootingDistance;

    [FormerlySerializedAs("UFOLaser")] [SerializeField]
    public GameObject ufoLaser;

    public event Action UfoDestroyed;

    private Animator _animator;

    private NavMeshAgent _navMeshAgent;

    private bool _isFalling = false;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _navMeshAgent.SetDestination(CalculateTargetPosition());
        StartCoroutine(SetNewTargetPosition());
    }

    private IEnumerator SetNewTargetPosition()
    {
        yield return new WaitForSeconds(3f);
        if (!_isFalling)
        {
            Shoot();
            _navMeshAgent.SetDestination(CalculateTargetPosition());
            StartCoroutine(SetNewTargetPosition());
        }
    }

    private Quaternion CalculateAimRotation()
    {
        Vector3 directionToPlayer = playerToAttack.transform.position - transform.position;
        
        directionToPlayer.Normalize();

        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        Quaternion adjustment = Quaternion.Euler(0, 0, -90f);
        Quaternion aimDesrutption = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-30, 30));
                
        return targetRotation * adjustment * aimDesrutption;
    }

    private void Shoot()
    {
        var rotation = CalculateAimRotation();
        var laser = Instantiate(ufoLaser);
        laser.GetComponent<Laser>().Initialize(rotation, transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _animator.SetBool("Falling", true);
        _isFalling = true;
        _navMeshAgent.ResetPath();
        _navMeshAgent.speed *= 3;
        _navMeshAgent.SetDestination(new Vector3(-15, 0, 0));
        StartCoroutine(PlannedDestruction());
    }

    private IEnumerator PlannedDestruction()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
        UfoDestroyed?.Invoke();
    }

    private Vector3 CalculateTargetPosition()
    {
        int variation = UnityEngine.Random.Range(0, 4);
        var targetPos = playerToAttack.transform.position;

        switch(variation)
        {
            case 0: targetPos += new Vector3(UnityEngine.Random.Range(-shootingDistance, shootingDistance), shootingDistance, 0);
                break;

            case 1: targetPos += new Vector3(UnityEngine.Random.Range(-shootingDistance, shootingDistance), -shootingDistance, 0);
                break;

            case 2: targetPos += new Vector3(shootingDistance, UnityEngine.Random.Range(-shootingDistance, shootingDistance), 0);
                break;

            case 3: targetPos += new Vector3(-shootingDistance, UnityEngine.Random.Range(-shootingDistance, shootingDistance), 0);
                break;
        }

        return targetPos;
    }
}
