using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Laser : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [FormerlySerializedAs("LaserClip")] public AudioClip laserClip;

    [FormerlySerializedAs("InitialForce")] public Vector2 initialForce; 

    [FormerlySerializedAs("Speed")] public float speed;

    public void Initialize(Quaternion rotation, Vector3 position)
    {
        StartCoroutine(PlannedDestruction());
        transform.position = position;
        transform.rotation = rotation * Quaternion.Euler(0f, 0f, 90f);
        _rigidbody = GetComponent<Rigidbody2D>();
        ApplyForce(rotation);

        GetComponent<AudioSource>().PlayOneShot(laserClip);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private IEnumerator PlannedDestruction()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void ApplyForce(Quaternion spaceShipRotation)
    {
        float angle = spaceShipRotation.eulerAngles.z * Mathf.Deg2Rad;

        Vector2 rotatedForce = new Vector2(
            initialForce.x * Mathf.Cos(angle) - initialForce.y * Mathf.Sin(angle),
            initialForce.x * Mathf.Sin(angle) + initialForce.y * Mathf.Cos(angle)
        );

        _rigidbody.AddForce(rotatedForce * speed, ForceMode2D.Impulse);
    }
}
