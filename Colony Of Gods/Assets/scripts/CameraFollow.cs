using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    void Start()
    {
        // This attempts to find an initial player if one exists
        if (target == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) target = p.transform;
        }
    }

    void LateUpdate()
    {
        // This code makes the camera follow the target smoothly over time
        if (target == null) return;
        
        // Calculate where the camera should be (target position + offset)
        Vector3 desired = target.position + offset;
        
        // Smoothly move the camera's current position toward the desired position
        Vector3 smoothed = Vector3.Lerp(transform.position, desired, smoothSpeed);
        transform.position = smoothed;
    }

    // NEW FUNCTION: Instantly moves the camera to the target position
    public void JumpToTarget()
    {
        if (target == null) return;
        
        // Calculate where the camera should be
        Vector3 desired = target.position + offset;
        
        // Set the camera's position instantly, ignoring smoothing
        transform.position = desired;
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }
}
