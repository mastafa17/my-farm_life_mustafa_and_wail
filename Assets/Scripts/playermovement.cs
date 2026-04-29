using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 1.5f;
    public float runSpeed = 3.5f;
    public float rotationSpeed = 3f;

    public float gravity = -20f;
    public float jumpHeight = 1.5f;

    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    public Animator animator;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
        }
    }

    void Update()
    {
        // Ground Check
        if (groundCheck != null)
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        else
            isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            if (animator != null)
                animator.SetBool("isJumping", false);
        }

        // Input
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        bool isWalking = Mathf.Abs(v) > 0.1f;
        bool isRunning = isWalking && Input.GetKey(KeyCode.LeftShift);

        // Rotate A / D
        if (Mathf.Abs(h) > 0.1f)
        {
            transform.Rotate(Vector3.up * h * rotationSpeed * 100f * Time.deltaTime);
        }

        // Move W / S
        if (isWalking)
        {
            float speed = isRunning ? runSpeed : walkSpeed;
            Vector3 move = transform.forward * v;
            controller.Move(move * speed * Time.deltaTime);
        }

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (animator != null)
                animator.SetBool("isJumping", true);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Animator
        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking && !isRunning);
            animator.SetBool("isRunning", isRunning);
        }
    }
}