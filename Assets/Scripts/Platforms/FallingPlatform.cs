using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float delayBeforeSink = 1f;
    public float sinkSpeed = 1f;
    public float sinkDistance = 2f;
    public float timeBeforeReset = 2f;
    public float resetSpeed = 1f;

    private bool sinking = false;
    private bool resetting = false;

    private Vector3 startPos;
    private Vector3 targetPos;

    private void Awake()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.down * sinkDistance;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!sinking && !resetting && collision.gameObject.CompareTag("Player"))
        {
            Invoke(nameof(StartSinking), delayBeforeSink);
        }
    }

    void StartSinking()
    {
        sinking = true;
    }

    void Update()
    {
        if (sinking)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, sinkSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                sinking = false;
                Invoke(nameof(StartResetting), timeBeforeReset);
            }
        }

        if (resetting)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, resetSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, startPos) < 0.01f)
            {
                resetting = false;
            }
        }
    }

    void StartResetting()
    {
        resetting = true;
    }

    // ✅ Add this to allow external reset
    public void ResetPlatform()
    {
        CancelInvoke(); // stop any pending sinks/resets

        sinking = false;
        resetting = false;

        transform.position = startPos;

        // Optional: reset state flags if platform was disabled
        this.enabled = true;
    }
}
