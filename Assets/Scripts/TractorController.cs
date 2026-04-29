using UnityEngine;

public class TractorController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float turnSpeed = 60f;

    void Update()
    {
        float move = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.forward * move * moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * turn * turnSpeed * Time.deltaTime);
    }
}