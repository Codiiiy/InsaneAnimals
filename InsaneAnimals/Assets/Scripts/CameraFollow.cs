using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Follow Settings")]
    [Tooltip("The target Transform the camera should follow (e.g., your chicken).")]
    public Transform target;

    [Tooltip("How quickly the camera catches up to the target.")]
    public float smoothTime = 0.3f;
    public float offsetY = 3f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = new Vector3(
            0.5f,                            
            target.position.y + offsetY,          
            transform.position.z             
        );

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
