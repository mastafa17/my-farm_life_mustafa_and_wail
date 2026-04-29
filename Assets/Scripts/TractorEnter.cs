using UnityEngine;

public class TractorEnter : MonoBehaviour
{
    public GameObject player;
    public Transform seatPoint;
    public Transform exitPoint;

    public MonoBehaviour playerMovement;
    public MonoBehaviour tractorMovement;

    public float enterDistance = 4f;

    private bool driving = false;
    private CharacterController playerController;
    private Animator animator;

    private TractorSound tractorSound;
    private WheelRotation wheelRotation;
    private WheelSteering wheelSteering;

    void Start()
    {
        tractorSound = GetComponent<TractorSound>();
        wheelRotation = GetComponent<WheelRotation>();
        wheelSteering = GetComponent<WheelSteering>();

        if (player != null)
        {
            playerController = player.GetComponent<CharacterController>();
            animator = player.GetComponentInChildren<Animator>();
        }

        if (tractorMovement != null)
            tractorMovement.enabled = false;
    }

    void Update()
    {
        if (player == null || seatPoint == null) return;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= enterDistance && Input.GetKeyDown(KeyCode.E))
        {
            if (!driving)
                EnterTractor();
            else
                ExitTractor();
        }
    }

    void EnterTractor()
    {
        if (playerController != null)
            playerController.enabled = false;

        if (playerMovement != null)
            playerMovement.enabled = false;

        player.transform.SetParent(seatPoint);
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;

        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isDriving", true);
        }

        if (tractorMovement != null)
            tractorMovement.enabled = true;

        if (tractorSound != null)
            tractorSound.EnterTractorSound();

        if (wheelRotation != null)
            wheelRotation.StartWheelRotation();

        if (wheelSteering != null)
            wheelSteering.StartSteering();

        driving = true;
    }

    void ExitTractor()
    {
        player.transform.SetParent(null);

        if (exitPoint != null)
        {
            player.transform.position = exitPoint.position;
            player.transform.rotation = exitPoint.rotation;
        }
        else
        {
            player.transform.position = transform.position + transform.right * 3f;
        }

        if (animator != null)
        {
            animator.SetBool("isDriving", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            animator.Play("Idle", 0, 0f);
        }

        if (playerController != null)
            playerController.enabled = true;

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (tractorMovement != null)
            tractorMovement.enabled = false;

        if (tractorSound != null)
            tractorSound.ExitTractorSound();

        if (wheelRotation != null)
            wheelRotation.StopWheelRotation();

        if (wheelSteering != null)
            wheelSteering.StopSteering();

        driving = false;
    }
}