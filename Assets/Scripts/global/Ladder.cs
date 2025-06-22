using UnityEngine;
using System.Collections;
public class Ladder : MonoBehaviour
{
    public float climbSpeed = 5f;
    private bool isClimbing = false;
    private bool isNearLadder = false;
    private GameObject player;
    private Animator playerAnimator;
    private Rigidbody playerRb;
    public float tolerance = 0.5f;
    private GameObject startHeight;

    private void Start()
    {
        startHeight = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!IsFacingLadder(other.gameObject))
            {
                return;
            }

            isNearLadder = true;
            Debug.Log("Press T to climb the ladder.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearLadder = false;
            if (isClimbing)
            {
                StopClimbing();
            }
        }
    }

    private void Update()
    {
        if (isNearLadder && !isClimbing && Input.GetKeyDown(KeyCode.T))
        {
            StartClimbing();
        }

        if (isClimbing && playerRb != null)
        {
            HandleClimbing();
        }
    }

    private void StartClimbing()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();
        playerRb = player.GetComponent<Rigidbody>();

        

        playerRb.Sleep();
        playerRb.angularVelocity = Vector3.zero;
        playerAnimator.SetFloat("InputVertical", 0);
        playerAnimator.SetFloat("InputHorizontal", 0);
        playerAnimator.SetFloat("VerticalVelocity", 0);

        player.GetComponent<Invector.CharacterController.vThirdPersonInput>().enabled = false;
        player.GetComponent<Invector.CharacterController.vThirdPersonController>().enabled = false;

        playerRb.useGravity = false;
        playerRb.velocity = Vector3.zero;

        playerRb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        player.transform.position = new Vector3(startHeight.transform.position.x, player.transform.position.y, startHeight.transform.position.z);
        player.transform.rotation = startHeight.transform.rotation;

        // Fade in the Climb layer
        StartCoroutine(FadeAnimatorLayerWeight(playerAnimator, "Climb", 1, 0.5f));

        isClimbing = true;
    }

    private void StopClimbing()
    {
        if (player != null)
        {
            isClimbing = false;
            playerRb.useGravity = true;
            playerRb.velocity = Vector3.zero;
            playerRb.constraints = RigidbodyConstraints.FreezeRotation;
            playerRb.Sleep();
            player.GetComponent<Invector.CharacterController.vThirdPersonController>().enabled = true;
            player.GetComponent<Invector.CharacterController.vThirdPersonInput>().enabled = true;

            // Fade out the Climb layer
            StartCoroutine(FadeAnimatorLayerWeight(playerAnimator, "Climb", 0, 0.5f));

            player = null;
            playerRb = null;
            playerAnimator = null;
        }
    }

    private void HandleClimbing()
    {
        float verticalInput = Input.GetAxis("Vertical"); // GetAxis returns a value between -1 and 1

        if (Mathf.Abs(verticalInput) > 0.1f) // To avoid very small values affecting the animation
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, verticalInput * climbSpeed, playerRb.velocity.z);
            playerAnimator.SetFloat("ClimbSpeed", verticalInput);
        }
        else
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);
            playerAnimator.SetFloat("ClimbSpeed", 0);
        }

        // Check if player height is within tolerance of startHeight
        float playerHeight = player.transform.position.y;
        float startHeightY = startHeight.transform.position.y;
        if (Mathf.Abs(playerHeight - startHeightY) <= tolerance)
        {
            playerAnimator.SetFloat("ClimbSpeed", 0);
        }
        //Mathf.Abs(playerHeight - startHeightY) <= tolerance && 
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StopClimbing();
        }
    }

    private bool IsFacingLadder(GameObject playerRef)
    {
        Vector3 playerForward = playerRef.transform.forward;
        Vector3 ladderForward = -transform.forward;

        // Calculate the dot product
        float dotProduct = Vector3.Dot(playerForward.normalized, ladderForward.normalized);

        // Check if the dot product is within the tolerance range
        return dotProduct > (1 - tolerance);
    }

    private IEnumerator FadeAnimatorLayerWeight(Animator animator, string layerName, float targetWeight, float duration)
    {
        int layerIndex = animator.GetLayerIndex(layerName);
        float startWeight = animator.GetLayerWeight(layerIndex);
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float weight = Mathf.Lerp(startWeight, targetWeight, time / duration);
            animator.SetLayerWeight(layerIndex, weight);
            yield return null;
        }

        animator.SetLayerWeight(layerIndex, targetWeight);
    }
}
