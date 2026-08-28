using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 3, -8);
    public float smoothSpeed = 10f;
    
    // Tilt settings
    public float tiltMultiplier = 2f;
    public float maxTilt = 5f;

    private void LateUpdate()
    {
        if (target == null) return;

        // Smooth follow
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Subtle camera tilt based on player's XY position
        float tiltX = -Mathf.Clamp(target.position.y * tiltMultiplier, -maxTilt, maxTilt);
        float tiltZ = -Mathf.Clamp(target.position.x * tiltMultiplier, -maxTilt, maxTilt);
        
        Quaternion targetRotation = Quaternion.Euler(tiltX, 0, tiltZ);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }
}
