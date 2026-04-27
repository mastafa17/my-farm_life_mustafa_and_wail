using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 3.5f;
    public float rotationSpeed = 3f;

    [Header("Jump")]
    public float gravity = -20f;
    public float jumpHeight = 1.5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    [Header("Animation")]
    public Animator animator;

    [Header("Audio")]
    public AudioSource footstepSource;
    public AudioSource sfxSource;
    public AudioClip walkSound;
    public AudioClip runSound;
    public AudioClip jumpSound;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (footstepSource != null)
        {
            footstepSource.loop = true;
            footstepSource.playOnAwake = false;
        }

        if (sfxSource != null)
        {
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }

        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
        }
    }

    void Update()
    {
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

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        bool isWalking = Mathf.Abs(v) > 0.1f;
        bool isRunning = isWalking && Input.GetKey(KeyCode.LeftShift);

        if (Mathf.Abs(h) > 0.1f)
        {
            transform.Rotate(Vector3.up * h * rotationSpeed * 100f * Time.deltaTime);
        }

        if (isWalking)
        {
            float speed = isRunning ? runSpeed : walkSpeed;
            Vector3 move = transform.forward * v;
            controller.Move(move * speed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (animator != null)
                animator.SetBool("isJumping", true);

            if (sfxSource != null && jumpSound != null)
                sfxSource.PlayOneShot(jumpSound);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking && !isRunning);
            animator.SetBool("isRunning", isRunning);
        }

        if (footstepSource != null)
        {
            if (isWalking && isGrounded)
            {
                AudioClip targetClip = isRunning ? runSound : walkSound;

                if (targetClip != null && footstepSource.clip != targetClip)
                {
                    footstepSource.clip = targetClip;
                    footstepSource.Play();
                }
                else if (targetClip != null && !footstepSource.isPlaying)
                {
                    footstepSource.Play();
                }
            }
            else
            {
                footstepSource.Stop();
            }
        }
    }
}