using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0f, 2f, -4f);
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 desiredPosition = player.position + player.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.LookAt(player.position + Vector3.up * 1.5f);
    }
}