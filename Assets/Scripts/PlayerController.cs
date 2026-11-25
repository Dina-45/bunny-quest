
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerStats stats;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    Vector2 moveInput;
    bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump()
    {
        if (!isGrounded) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.jumpForce);
        SoundManager.Play(Sfx.Jump);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * stats.moveSpeed, rb.linearVelocity.y);

        if (moveInput.x != 0)
            sr.flipX = moveInput.x < 0;

        anim.SetFloat("Speed", Mathf.Abs(moveInput.x));
        anim.SetBool("IsGrounded", isGrounded);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Ground")) return;

        bool wasInAir = !isGrounded;
        isGrounded = true;

        if (wasInAir && col.relativeVelocity.y >= FxManager.LandMinSpeed)
        {
            Collider2D myCol = GetComponent<Collider2D>();
            float feetY = myCol != null ? myCol.bounds.min.y : transform.position.y;

            FxManager.Spawn(Fx.Land, new Vector3(transform.position.x, feetY, 0f));
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}
