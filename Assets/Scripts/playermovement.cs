using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 3.5f;
    public float rotationSpeed = 120f;

    [Header("Jump & Gravity")]
    public float gravity = -20f;
    public float jumpHeight = 1.5f;

    [Header("Animator")]
    public Animator animator;

    [Header("Sounds")]
    public AudioSource audioSource;
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
    }

    void Update()
    {
        if (controller == null || !controller.enabled)
            return;

        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;

            if (animator != null)
                animator.SetBool("isJumping", false);
        }

        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        bool isMoving = Mathf.Abs(moveInput) > 0.1f;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        float speed = isRunning ? runSpeed : walkSpeed;

        transform.Rotate(Vector3.up * turnInput * rotationSpeed * Time.deltaTime);

        Vector3 move = transform.forward * moveInput;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (animator != null)
                animator.SetBool("isJumping", true);

            if (jumpSound != null)
                AudioSource.PlayClipAtPoint(jumpSound, transform.position, 1f);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (animator != null)
        {
            animator.SetBool("isWalking", isMoving && !isRunning);
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isGrounded", isGrounded);
        }

        HandleFootstepSounds(isMoving, isRunning);
    }

    void HandleFootstepSounds(bool isMoving, bool isRunning)
    {
        if (audioSource == null) return;

        if (!isGrounded || !isMoving)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();

            return;
        }

        AudioClip targetClip = isRunning ? runSound : walkSound;

        if (targetClip == null) return;

        if (audioSource.clip != targetClip)
        {
            audioSource.clip = targetClip;
            audioSource.loop = true;
            audioSource.Play();
        }
        else if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}