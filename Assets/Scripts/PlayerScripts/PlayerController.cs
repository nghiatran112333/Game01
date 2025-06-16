using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float m_Speed = 4.0f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rollForce = 12f;
    [SerializeField] private float rollDuration = 0.5f;
    [SerializeField] private float comboResetTime = 1.0f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isRolling = false;
    private float rollTimer = 0f;
    private int currentAttack = 0;
    private float lastAttackTime = 0f;
    private bool isAttacking = false;
    private bool isDead = false;
    private bool isBlocking = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Always update ground state first
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (isDead) return; // Stop all logic if dead
        HandleBlock();
        HandleMovement();
        HandleJump();
        HandleRoll();
        HandleAttack();
        UpdateAnimation();
    }

    private void HandleMovement()
    {
        if (isRolling || isAttacking || isBlocking || isDead) return;

        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * m_Speed, rb.linearVelocity.y);

        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isRolling)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump"); // Gọi Trigger Jump
        }
    }

    private void HandleRoll()
    {
        if (isRolling)
        {
            rollTimer -= Time.deltaTime;

            // Giữ nguyên vận tốc roll trong suốt thời gian roll
            float rollDirection = Mathf.Sign(transform.localScale.x);
            rb.linearVelocity = new Vector2(rollDirection * rollForce, rb.linearVelocity.y);

            if (rollTimer <= 0f)
            {
                isRolling = false;
                animator.SetBool("Roll", false);

                // Ngừng đẩy khi roll kết thúc
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }

            return;
        }

        // Nhấn Shift + đang đứng đất + không tấn công + không chết
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded && !isAttacking && !isDead)
        {
            isRolling = true;
            rollTimer = rollDuration;

            animator.SetBool("Roll", true);

            // Xác định hướng roll (hướng quay mặt)
            float rollDirection = Mathf.Sign(transform.localScale.x);

            // Đẩy nhân vật ngay lập tức
            rb.linearVelocity = new Vector2(rollDirection * rollForce, rb.linearVelocity.y);
        }
    }



    private void HandleAttack()
    {
        if (isDead || isRolling || isAttacking || isBlocking) return;

        if (Input.GetMouseButtonDown(0))
        {
            float timeSinceLastAttack = Time.time - lastAttackTime;

            // Nếu thời gian combo reset → reset lại combo
            if (timeSinceLastAttack > comboResetTime)
            {
                currentAttack = 1;
            }
            else
            {
                currentAttack++;
                if (currentAttack > 3)
                    currentAttack = 1;
            }
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            // Gọi animation tấn công tương ứng: Attack1, Attack2, Attack3
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");
            animator.ResetTrigger("Attack3");
            animator.SetTrigger("Attack" + currentAttack);

            isAttacking = true;
            lastAttackTime = Time.time;

        }
    }

    // Hàm được gọi từ Animation Event ở cuối mỗi clip attack
    public void EndAttack()
    {
        isAttacking = false;
    }

    private void HandleBlock()
    {
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("Idle",false);
            animator.SetBool("IdleBlock",true);
            isBlocking = true;
        }
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        if (Input.GetMouseButtonUp(1)) // Nhả chuột: tắt block
        {
            animator.SetBool("Idle", true);
            animator.SetBool("IdleBlock", false); ;
            isBlocking = false;
        }
    }


    public void Die()
    {
        if (isDead) return;

        isDead = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetTrigger("Die");
    }
    private void UpdateAnimation()
    {
        animator.SetBool("Grounded", isGrounded);
        animator.SetFloat("AirSpeed", rb.linearVelocity.y);
        animator.SetBool("isRunning", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
    }
}
