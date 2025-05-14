using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action AsteroidDestroyedByLaser;

    public static Action AsteroidCollidedWithSpaceShip;

    public static Action SceneChangeStarted;

    public static Action LaserShoot;

    public static Action<float> SlowdownEffectApplied;

    public static Action<float> TeleportApplied;

    public static Action<float> ShieldUsed;

    public static Action<float> HyperAttackUsed;
}
