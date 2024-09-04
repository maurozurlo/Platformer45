using UnityEngine;

public class MetalDetector : MonoBehaviour
{
	[SerializeField] private float detectionRange = 5f;
	[SerializeField] private LayerMask discoverableLayer;

	[SerializeField] private CylinderIndicator cylinderIndicator;
	[SerializeField] private BeepIndicator beepIndicator;

	private Transform closestObject;
	private float closestDistance;

	private void Update()
	{
		CheckForObjects();
		float dist = closestObject == null ? detectionRange : closestDistance;
		UpdateIndicators(dist);
	}

	private void CheckForObjects()
	{
		closestObject = null;
		closestDistance = detectionRange; // Default to the max range

		RaycastHit[] hits = Physics.SphereCastAll(transform.position, detectionRange, Vector3.forward, detectionRange, discoverableLayer);
		foreach (RaycastHit hit in hits)
		{
			if (hit.collider.CompareTag("Discoverable"))
			{
				float distance = Vector3.Distance(transform.position, hit.transform.position);
				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestObject = hit.transform;
				}
			}
		}
	}

	private void UpdateIndicators(float distance)
	{
		if (cylinderIndicator != null && beepIndicator != null)
		{
			float normalizedDistance = Mathf.Clamp01(1 - (distance / detectionRange)); // Normalize the distance
			cylinderIndicator.UpdateSize(normalizedDistance);
			beepIndicator.UpdateBeep(normalizedDistance);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, detectionRange);
	}
}
