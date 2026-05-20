using UnityEngine;

public class Movement : MonoBehaviour
{
    [Tooltip("Speed at which the object moves.")]
    public float moveSpeed = 5f;

    void Update()
    {
        // Horizontal = left / right arrows or A / D
        // Vertical = up / down arrows or W / S
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, moveY, 0f) * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }
}
