using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetBool("ismoving", movement != Vector2.zero);

        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            animator.SetFloat("movex", movement.x);
            animator.SetFloat("movey", 0);
        }
        else
        {
            animator.SetFloat("movex", 0);
            animator.SetFloat("movey", movement.y);
        }
        Debug.Log($"X={movement.x} Y={movement.y}");
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement.normalized * speed;
    }
}