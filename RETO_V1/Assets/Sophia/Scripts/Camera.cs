using UnityEngine;

public class Camera : MonoBehaviour
{
    [Tooltip("Drag the player GameObject here in the Inspector.")]
    public Transform target;

    [Tooltip("Optional offset from the target position.")]
    public Vector3 offset = new Vector3(0f, 2f, -10f);

    [Tooltip("How fast the camera follows the player.")]
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
