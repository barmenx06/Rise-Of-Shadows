using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Joystick joystick;

    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector2 input = new Vector2(joystick.Horizontal, joystick.Vertical);
        rb.linearVelocity = input.normalized * moveSpeed;

        // Animasyon i�in speed parametresi g�nder
        if (animator != null)
            animator.SetFloat("Speed", input.sqrMagnitude);

        // Karakter y�n�n� sola/sa�a d�nd�r
        if (input.x > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (input.x < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);
    }
}
