using UnityEngine;

public class TractorEnter : MonoBehaviour
{
    public GameObject player;
    public Transform seatPoint;
    public MonoBehaviour playerMovement;
    public MonoBehaviour tractorMovement;

    public float enterDistance = 4f;

    private bool driving = false;

    void Start()
    {
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
        player.transform.position = seatPoint.position;
        player.transform.rotation = seatPoint.rotation;

        player.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (tractorMovement != null)
            tractorMovement.enabled = true;

        driving = true;
       
    }

    void ExitTractor()
    {
        player.transform.position = transform.position + transform.right * 3f;
        player.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (tractorMovement != null)
            tractorMovement.enabled = false;

        driving = false;
    }
}