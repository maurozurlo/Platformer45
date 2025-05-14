using System.Collections;
using UnityEngine;

public class CamSwitcher : MonoBehaviour
{
    public enum CameraType { Main, Skull }

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera skullCamera;
    [SerializeField] private Camera transitionCamera;

    [SerializeField] public float transitionDuration = 1f;

    private CameraType activeCamera = CameraType.Main;
    private bool canSwitch = false;
    private bool isTransitioning = false;

    void Start()
    {
        ActivateCamera(CameraType.Main);
    }

    void Update()
    {
        if (!canSwitch || isTransitioning)
            return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            CameraType nextCamera = activeCamera == CameraType.Main ? CameraType.Skull : CameraType.Main;
            SwitchTo(nextCamera);
        }
    }

    public void SwitchTo(CameraType target)
    {
        if (target == activeCamera || isTransitioning) return;

        StartCoroutine(TransitionTo(target));
    }

    public void LockSwitching(bool locked)
    {
        canSwitch = !locked;
    }

    private IEnumerator TransitionTo(CameraType targetCameraType)
    {
        isTransitioning = true;

        Camera fromCam = GetCamera(activeCamera);
        Camera toCam = GetCamera(targetCameraType);

        // Set transition cam to current cam position/state
        transitionCamera.transform.position = fromCam.transform.position;
        transitionCamera.transform.rotation = fromCam.transform.rotation;
        transitionCamera.fieldOfView = fromCam.fieldOfView;
        transitionCamera.enabled = true;
        transitionCamera.depth = 10;

        fromCam.enabled = false;
        toCam.enabled = false;

        float elapsed = 0f;
        Vector3 startPos = fromCam.transform.position;
        Quaternion startRot = fromCam.transform.rotation;
        float startFOV = fromCam.fieldOfView;

        Vector3 endPos = toCam.transform.position;
        Quaternion endRot = toCam.transform.rotation;
        float endFOV = toCam.fieldOfView;

        while (elapsed < transitionDuration)
        {
            float t = elapsed / transitionDuration;
            transitionCamera.transform.position = Vector3.Lerp(startPos, endPos, t);
            transitionCamera.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            transitionCamera.fieldOfView = Mathf.Lerp(startFOV, endFOV, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transitionCamera.enabled = false;
        toCam.enabled = true;
        toCam.depth = 1;

        activeCamera = targetCameraType;
        isTransitioning = false;
    }

    private Camera GetCamera(CameraType type)
    {
        return type == CameraType.Main ? mainCamera : skullCamera;
    }

    private void ActivateCamera(CameraType type)
    {
        mainCamera.enabled = (type == CameraType.Main);
        skullCamera.enabled = (type == CameraType.Skull);
        transitionCamera.enabled = false;
        activeCamera = type;
    }
}
