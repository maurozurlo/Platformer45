using UnityEngine;
using UnityEngine.UI;

public class GradientTest : MonoBehaviour
{
	public Gradient gradient;
	public Material mat;


	public float strobeDuration = 2f;

	void Awake()
	{
		mat.EnableKeyword("_EMISSION");
	}

	public void Update()
	{
		float t = Mathf.PingPong(Time.time / strobeDuration, 1f);
		mat.SetColor("_EmissionColor", gradient.Evaluate(t));
	}
}