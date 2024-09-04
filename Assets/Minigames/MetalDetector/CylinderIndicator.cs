using UnityEngine;

public class CylinderIndicator : MonoBehaviour
{
    [SerializeField] private float minSize = 0.5f;
    [SerializeField] private float absoluteMaxSize = 2f;
    [SerializeField] private float speed = 1f;

    private float maxSize;
    private float acc = 0;

	private void Start()
	{
        transform.localScale = new Vector3(minSize, .02f, minSize);
    }
	private void Update()
    {
        if (maxSize == 0) return;
        // Calculate the scale factor using a sine wave        
        acc += Time.deltaTime * speed;

        float scale = Mathf.Lerp(minSize, maxSize, (Mathf.Sin(acc) + 1f) / 2f);

        // Apply the scale to the GameObject
        transform.localScale = new Vector3(scale, .02f, scale);
    }

    public void UpdateSize(float n)
    {
        if (n == 0) {
            maxSize = 0;
            return;
        }
        maxSize = Mathf.Clamp(n * absoluteMaxSize, minSize, absoluteMaxSize);
    }
}
