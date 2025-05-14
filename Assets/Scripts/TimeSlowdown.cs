using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeSlowdown : MonoBehaviour
{
    [FormerlySerializedAs("SlowDownFactor")] [SerializeField]
    public float slowDownFactor;

    [FormerlySerializedAs("SlowDownLength")] [SerializeField]
    public float slowDownLength;

    public IEnumerator SlowTimeSmoothly()
    {
        float originalTimeScale = Time.timeScale;
        float timer = 0f;
        Debug.Log("Slowdown applied.");
    
        while (timer < 0.5f)
        {
            timer += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(originalTimeScale, slowDownFactor, timer / 0.5f);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }
    
        yield return new WaitForSecondsRealtime(slowDownLength);
    
        timer = 0f;
        while (timer < 0.5f)
        {
            timer += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(slowDownFactor, 1f, timer / 0.5f);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }
    
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
