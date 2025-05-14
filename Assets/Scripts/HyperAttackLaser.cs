using System.Collections;
using UnityEngine;

public class BigLaser : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    public Vector2 initialForce = new Vector2(0f, 0f); 

    public float speed = 3f;

    public void Initialize(Quaternion rotation, Vector3 position)
    {
        StartCoroutine(PlannedDestruction());
        transform.position = position;
        transform.rotation = rotation;
        transform.rotation = rotation * Quaternion.Euler(0f, 0f, 90f);
        _rigidbody = GetComponent<Rigidbody2D>();
        ApplyForce();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.name.Contains("Laser")) Destroy(gameObject);
    }

    private IEnumerator PlannedDestruction()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    public Vector2 RotateVectorCounterclockwise(Vector2 originalVector)
    {
        return new Vector2(originalVector.y, -originalVector.x);
    }

    private void ApplyForce()
    {
        Vector2 forceDirection = transform.up;
        forceDirection = RotateVectorCounterclockwise(forceDirection);
        _rigidbody.AddForce(forceDirection * speed, ForceMode2D.Impulse);
    }
}
