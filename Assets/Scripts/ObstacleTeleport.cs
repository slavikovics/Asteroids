using UnityEngine;

public class ObstacleTeleport : MonoBehaviour
{
    [SerializeField]
    private Transform _playerTransform;

    private void Awake()
    {
        EventManager.TeleportApplied += TeleportObstacle;
    }

    private void TeleportObstacle(float duration)
    {
        transform.position = _playerTransform.position;
    }


    private void OnDestroy()
    {
        EventManager.TeleportApplied -= TeleportObstacle;        
    }
}
