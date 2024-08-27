using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private Vector3 _originalPos;

    void Awake()
    {
        // Enforce Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

	public static void Shake(float duration, float amount)
    {
        if (Instance != null)
        {
            Instance.StopAllCoroutines();
            Instance.StartCoroutine(Instance.ShakeCoroutine(duration, amount));
        }
    }

    private IEnumerator ShakeCoroutine(float duration, float amount)
    {
        float endTime = Time.realtimeSinceStartup + duration;
        float timeAtLastFrame = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup < endTime)
        {
            float fakeDelta = Time.realtimeSinceStartup - timeAtLastFrame;
            timeAtLastFrame = Time.realtimeSinceStartup;

            transform.localPosition = _originalPos + Random.insideUnitSphere * amount;

            yield return null;
        }

        transform.localPosition = _originalPos;
    }

    public void SetOriginalPosition(Vector3 pos)
    {
        _originalPos = pos;
    }
}
