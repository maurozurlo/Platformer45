using UnityEngine;

public class BeepIndicator : MonoBehaviour
{
    [SerializeField] private AudioClip beepClip;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float minPitch = 0.5f;
    [SerializeField] private float maxPitch = 2f;
    [SerializeField] private float minRepeatRate = 0.5f; // Minimum beeps per second when farthest

    private float acc = 0f;
    private float repeatRate;

    private void Start()
    {
        audioSource.clip = beepClip;
    }

    public void UpdateBeep(float normalizedDistance)
    {
        if (normalizedDistance == 0)
        {
            // Stop the audio if the object is out of reach
            audioSource.Stop();
            repeatRate = 0;
            return;
        }

        // Update the pitch based on the normalized distance
        audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, normalizedDistance);

        // Update the repeat rate based on the normalized distance (closer means faster beeping)
        repeatRate = Mathf.Lerp(minRepeatRate, 10f, normalizedDistance);

        // Ensure the audio is playing if it was stopped
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void Update()
    {
        if (repeatRate == 0) return;

        // Accumulate time based on deltaTime
        acc += Time.deltaTime;

        // Play the beep sound when the accumulated time reaches the repeat rate interval
        if (acc >= 1f / repeatRate)
        {
            audioSource.PlayOneShot(beepClip);
            acc = 0f; // Reset the accumulator
        }
    }
}
