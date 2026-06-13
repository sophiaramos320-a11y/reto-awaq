using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;

    [Header("Map Limits")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void LateUpdate()
    {
        // 1. Calculate where the camera *wants* to go based on the player
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10f);

        // 2. Smoothly blend the camera towards that target position (Your original Lerp)
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        // 3. THE MAGIC LINES: Lock the smooth position so it never crosses the limits
        float clampedX = Mathf.Clamp(smoothedPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(smoothedPosition.y, minY, maxY);

        // 4. Move the camera to the final, safe position
        transform.position = new Vector3(clampedX, clampedY, -10f);
    }
}