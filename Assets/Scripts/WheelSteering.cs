using UnityEngine;

public class WheelSteering : MonoBehaviour
{
    public Transform steeringWheel;

    public float maxSteeringAngle = 180f;
    public float smoothSpeed = 8f;

    private bool playerInside = false;
    private Quaternion steeringStart;

    void Start()
    {
        if (steeringWheel != null)
            steeringStart = steeringWheel.localRotation;
    }

    void Update()
    {
        if (!playerInside) return;
        if (steeringWheel == null) return;

        float steer = Input.GetAxis("Horizontal");

        Quaternion target =
            steeringStart *
            Quaternion.Euler(0f, 0f, steer * maxSteeringAngle);

        steeringWheel.localRotation = Quaternion.Lerp(
            steeringWheel.localRotation,
            target,
            smoothSpeed * Time.deltaTime
        );
    }

    public void StartSteering()
    {
        playerInside = true;
    }

    public void StopSteering()
    {
        playerInside = false;
    }
}