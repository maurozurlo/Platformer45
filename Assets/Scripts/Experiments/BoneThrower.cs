using System.Collections;
using UnityEngine;
public class BoneThrower: MonoBehaviour
{
	[Header("Bone Prefab")]
	public GameObject bonePrefab;

	[Header("Bone Settings")]
	private KeyCode throwKey = KeyCode.LeftControl;
	public Transform throwPosition;
	public Vector3 throwDirection = new Vector3(0, 1, 0);


	[Header("Bone Settings")]
	public float throwForce = 10f;
	public float maxForce = 20f;

	public bool isCharging;
	public float chargeTime = 0;

	public bool isCooldownTime;

	public LineRenderer lr;

	Animator boneAnim;

	Vector2 throwAnim = new Vector2(20.2f, 29);

	public float perc;

	Camera mainCamera;

	private void Start()
	{
		mainCamera = Camera.main;
		lr = GetComponent<LineRenderer>();
		boneAnim = GetComponentInChildren<Animator>();
	}


	private void Update()
	{
		if (isCooldownTime) return;

		if (Input.GetKeyDown(throwKey))
		{
			StartThrowing();
		}

		if (isCharging)
		{
			ChargeThrow();
		}

		if (Input.GetKeyUp(throwKey)) {
			ReleaseThrow();
		}

	}

	void StartThrowing() {
		
		chargeTime = 0;
		perc = 0;
		boneAnim.SetBool("charging", true);
		boneAnim.SetFloat("charge", 0);
		isCharging = true;
		lr.enabled = true;
	}

	void ChargeThrow()
	{
		chargeTime += Time.deltaTime * .5f;
		float normalizedChargeTime = Mathf.Min(chargeTime * throwForce, maxForce);
		perc = Mathf.Lerp(throwAnim.y, throwAnim.y, Mathf.Min(chargeTime, 1)) * .01f;
		boneAnim.SetFloat("charge", perc);
		Vector3 grenadeVelocity = (mainCamera.transform.forward + throwDirection).normalized * normalizedChargeTime;
		ShowTrajectory(throwPosition.position + throwPosition.forward, grenadeVelocity);
	}

	void ReleaseThrow()
	{
		isCooldownTime = true;
		isCharging = false;
		StartCoroutine(SmoothlyTransitionToOne(1));
		ThrowGrenade(Mathf.Min(chargeTime * throwForce, maxForce));
		lr.enabled = false;
		StartCoroutine(Cooldown(3));

	}

	void ThrowGrenade(float force) {
		Vector3 spawnPosition = throwPosition.position + mainCamera.transform.forward;
		GameObject bone = Instantiate(bonePrefab, spawnPosition, mainCamera.transform.rotation);

		Rigidbody rb = bone.GetComponent<Rigidbody>();
		Vector3 finalThrowDirection = (mainCamera.transform.forward + throwDirection).normalized;

		rb.AddForce(finalThrowDirection * force, ForceMode.VelocityChange);
	}

	void ShowTrajectory(Vector3 origin, Vector3 speed)
	{
		Vector3[] points = new Vector3[100];
		lr.positionCount = points.Length;

		for (int i = 0; i < points.Length; i++)
		{
			float time = i * 0.1f;
			points[i] = origin + speed * time + .5f * Physics.gravity * time * time;
		}

		lr.SetPositions(points);
	}

	IEnumerator SmoothlyTransitionToOne(float duration)
	{
		float startTime = Time.time;
		float startValue = perc;

		while (Time.time < startTime + duration)
		{
			perc = Mathf.Lerp(startValue, 1f, (Time.time - startTime) / duration);
			boneAnim.SetFloat("charge", perc);
			yield return null;
		}
		boneAnim.SetBool("charging", false);
		boneAnim.SetFloat("charge", 1);
	}

	IEnumerator Cooldown(float duration)
	{
		yield return new WaitForSeconds(duration);
		isCooldownTime = false;
	}
}