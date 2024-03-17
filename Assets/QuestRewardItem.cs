using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRewardItem : MonoBehaviour
{
	public bool shouldAppearAfterQuestIsCompleted;
	public float duration = 2f; // Duration of the scaling animation
	public AnimationCurve scaleCurve; // Animation curve for scaling

	private Vector3 initialScale; // Initial scale of the GameObject
    private bool isInitialScaleZero; // Flag to track if the initial scale is zero

 //   private void Update()
	//{
 //       if (Input.GetKeyDown(KeyCode.Tab))
 //       {
 //           DisplayOrHideObject();
 //       }
	//}

	public void DisplayOrHideObject()
	{
        if (!shouldAppearAfterQuestIsCompleted)
        {
            DestroyImmediate(gameObject);
        }
        initialScale = transform.localScale; // Store the initial scale
        isInitialScaleZero = initialScale == Vector3.zero; // Check if initial scale is zero
        StartCoroutine(ScaleOverTime());
    }

    private IEnumerator ScaleOverTime()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration; // Normalized time
            float scaleValue = scaleCurve.Evaluate(t); // Get the scale value from the curve

            // If initial scale is zero, use the curve value directly
            // Otherwise, multiply the initial scale with the curve value
            transform.localScale = isInitialScaleZero ? new Vector3(scaleValue, scaleValue, scaleValue) : initialScale * scaleValue;

            elapsedTime += Time.deltaTime; // Update elapsed time
            yield return null; // Wait for the next frame
        }

        // Ensure the final scale is correct
        transform.localScale = isInitialScaleZero ? new Vector3(scaleCurve.Evaluate(1f), scaleCurve.Evaluate(1f), scaleCurve.Evaluate(1f)) : initialScale * scaleCurve.Evaluate(1f);
    }
}
