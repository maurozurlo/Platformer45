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

	bool isCharging;
	public float chargeTime = 0;

	LineRenderer lr;

	Animator boneAnim;

	Vector2 minThrowAnim = new Vector2(16, 17.9f);
	Vector2 maxThrowAnim = new Vector2(23, 29.0f);

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
		isCharging = true;
		chargeTime = 0;
		boneAnim.SetBool("charging", true);
		boneAnim.speed = 0;

		// Trajectory line
		lr.enabled = true;
	}

	void ChargeThrow()
	{
		chargeTime += Time.deltaTime * .2f;
		float normalizedChargeTime = Mathf.Min(chargeTime * throwForce, maxForce);

		perc = Mathf.Lerp(minThrowAnim.y, maxThrowAnim.y, Mathf.Min(chargeTime, 1)) * .01f;
		boneAnim.Play("throw", -1, perc);
		boneAnim.Update(0);
		Vector3 grenadeVelocity = (mainCamera.transform.forward + throwDirection).normalized * normalizedChargeTime;
		ShowTrajectory(throwPosition.position + throwPosition.forward, grenadeVelocity);
	}

	void ReleaseThrow()
	{
		ThrowGrenade(Mathf.Min(chargeTime * throwForce, maxForce));
		boneAnim.SetBool("charging", false);
		boneAnim.speed = 1;
		lr.enabled = false;
		isCharging = false;
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
}