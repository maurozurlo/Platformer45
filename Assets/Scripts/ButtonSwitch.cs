using UnityEngine;

public class ButtonSwitch : MonoBehaviour
{
    public GameObject triggerTarget;
    private bool hasBeenActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenActivated) return;
        if (!other.CompareTag("Player") && !other.CompareTag("PlayerSkull")) return;

        hasBeenActivated = true;
        TryActivateTarget();
    }

    private void TryActivateTarget()
    {
        if (triggerTarget == null)
        {
            Debug.LogWarning("ButtonSwitch: triggerTarget is not assigned.");
            return;
        }

        var component = triggerTarget.GetComponent<MonoBehaviour>();
        switch (component)
        {
            case DoorController door:
                door.OpenDoor();
                break;

            // You can add more cases here for other component types
            default:
                Debug.Log("ButtonSwitch: Trigger target activated, but no recognized component found.");
                break;
        }
    }
}
