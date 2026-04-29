using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    public Transform[] wheels;
    public float rotateSpeed = 300f;

    private bool playerInside = false;

    void Update()
    {
        if (!playerInside) return;

        float move = Input.GetAxis("Vertical");

        if (Mathf.Abs(move) > 0.01f)
        {
            foreach (Transform wheel in wheels)
            {
                if (wheel != null)
                {
                    wheel.Rotate(Vector3.forward * move * rotateSpeed * Time.deltaTime);
                }
            }
        }
    }

    public void StartWheelRotation()
    {
        playerInside = true;
    }

    public void StopWheelRotation()
    {
        playerInside = false;
    }
}