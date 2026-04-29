using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform target;

    public Vector3 playerOffset = new Vector3(0, 2.5f, -4f);
    public Vector3 tractorOffset = new Vector3(0, 3f, -7f);

    public bool followTractor = false;
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 offset = followTractor ? tractorOffset : playerOffset;
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }

    public void SetTarget(Transform newTarget, bool isTractor)
    {
        target = newTarget;
        followTractor = isTractor;
    }
}