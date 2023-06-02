using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private OpenCloseDoor currentDoor; // Current door the player is interacting with

    // Update is called once per frame
    void Update()
    {
        // Check for interaction key press
        if (Input.GetKeyDown(KeyCode.F))
        {
            // If the player is inside a door trigger, interact with it
            if (currentDoor != null)
            {
                currentDoor.ChangeDoorState();
            }
        }
    }

    // Called when the player enters a trigger
    private void OnTriggerEnter(Collider other)
    {
        // Check if the trigger is a door
        OpenCloseDoor door = other.GetComponent<OpenCloseDoor>();
        if (door != null)
        {
            currentDoor = door;
            Debug.Log("Entered door trigger: " + other.gameObject.name);
        }
    }

    // Called when the player leaves a trigger
    private void OnTriggerExit(Collider other)
    {
        // Check if the trigger is the current door
        if (currentDoor != null && other.gameObject == currentDoor.gameObject)
        {
            currentDoor = null;
            Debug.Log("Exited door trigger: " + other.gameObject.name);
        }
    }
}
