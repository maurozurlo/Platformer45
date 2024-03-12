using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{
	[Header("Explosion Prefab")]
	public GameObject explosionEffectPrefab;
	public Vector3 explosionParticleOffeset = new Vector3(0, 1, 0);

	[Header("Explosion Settings")]
	public float explosionDelay = 3f;
	public float explosionForce = 700f;
	public float explosionRadius = 5f;

	[Header("Audio Effects")]

	private float countdown;
	private bool hasExploded;

	private void Start()
	{
		countdown = explosionDelay;
	}

	private void Update()
	{
		if (!hasExploded)
		{
			countdown -= Time.deltaTime;
			if (countdown <= 0f)
			{
				Explode();
				hasExploded = true;
			}
		}
	}

	void Explode()
	{
		GameObject explosionEffect = Instantiate(explosionEffectPrefab, transform.position + explosionParticleOffeset, Quaternion.identity);
		
		NearbyForceApply();

		Destroy(gameObject, 5f);
	}

	void NearbyForceApply()
	{

		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

		foreach (Collider nearbyObject in colliders)
		{
			Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
			if (!rb) return;
			rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
		}
	}

}
