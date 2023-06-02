using UnityEngine;

public class OpenCloseDoor : MonoBehaviour
{
    public float openAngle = 90.0f; // The angle to open the door
    public float openSpeed = 2.0f;  // The speed at which the door opens
    public float angleThreshold = 0.1f; // Threshold to consider the door open or closed

    private bool isOpen = false;    // Is the door currently open?
    private GameObject player;      // Player GameObject
    private float targetAngle = 0;  // Target angle for the door
    private float initialRotation;  // Initial rotation of the door

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("No GameObject found with Player tag. Make sure your player is tagged with 'Player'.");
        }
        else
        {
            Debug.Log("Player found!");
        }

        // Save the initial rotation
        initialRotation = transform.localEulerAngles.y;
        Debug.Log("Initial rotation: " + initialRotation);

        // Set the target angle to the initial rotation
        targetAngle = initialRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Smoothly rotate the door if it's not at the target angle yet
        if (Mathf.Abs(targetAngle - transform.localEulerAngles.y) > angleThreshold)
        {
            float angle = Mathf.LerpAngle(transform.localEulerAngles.y, targetAngle, Time.deltaTime * openSpeed);
            transform.localEulerAngles = new Vector3(0, angle, 0);
            Debug.Log("Door angle: " + angle);
        }
    }

    private void DecideOpenAngle()
    {
        Vector3 doorToPlayer = player.transform.position - transform.position;
        Debug.Log("Door to player vector: " + doorToPlayer);

        // Cross product to determine if player is on the left or right of the door (from the door's perspective)
        float crossProduct = Vector3.Cross(transform.right, doorToPlayer).y; // Changed to right vector
        Debug.Log("Cross product: " + crossProduct);

        // Visualizing vectors
        Debug.DrawRay(transform.position, transform.right * 5, Color.blue);  // Changed to right vector
        Debug.DrawRay(transform.position, doorToPlayer, Color.green);

        // If crossProduct is positive, player is on the right side of the door, so door should open to the left (counter-clockwise), hence angle is positive
        // If crossProduct is negative, player is on the left side of the door, so door should open to the right (clockwise), hence angle is negative
        targetAngle = isOpen ? initialRotation + ((crossProduct > 0) ? -openAngle : openAngle) : initialRotation;
        Debug.Log("Target angle: " + targetAngle);
    }

    public void ChangeDoorState()
    {
        isOpen = !isOpen;
        Debug.Log("Door state changed. isOpen: " + isOpen);
        DecideOpenAngle();
    }
}
