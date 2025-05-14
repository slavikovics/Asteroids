using UnityEngine;
using UnityEngine.Serialization;

public class Shield : MonoBehaviour
{
    [FormerlySerializedAs("ShieldClip")] public AudioClip shieldClip;

    public void Initialize(Vector3 position)
    {
        transform.position = position;
        GetComponent<AudioSource>().PlayOneShot(shieldClip);
    }
}
