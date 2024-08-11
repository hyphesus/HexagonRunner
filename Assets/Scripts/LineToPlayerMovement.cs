using UnityEngine;

public class LineToPlayerMovement : MonoBehaviour
{
    public Transform parentObject;  // The empty parent object
    private int currentSide = 0;
    private int totalSides = 6;

    private bool isFirstTrigger = true;
    private int firstTriggeredSide;

    void OnTriggerEnter(Collider other)
    {
        // Check if the trigger is tagged correctly
        if (other.CompareTag("SideTrigger"))
        {
            int triggeredSide = other.GetComponent<SideTrigger>().sideIndex;

            Debug.Log($"Triggered Side: {triggeredSide}, Current Side: {currentSide}");

            // First trigger detection
            if (isFirstTrigger)
            {
                firstTriggeredSide = triggeredSide;
                isFirstTrigger = false;

                // Check if the player is already on this side
                if (currentSide == firstTriggeredSide)
                {
                    Debug.Log("Player is already on the first triggered side. No rotation needed.");
                    isFirstTrigger = true;  // Reset for the next interaction
                }
                else
                {
                    RotateToSide(firstTriggeredSide);
                }
            }
            else
            {
                // Second trigger detection
                if (currentSide == triggeredSide)
                {
                    Debug.Log("Player has moved to the first triggered side. Now checking second rotation...");
                    isFirstTrigger = true;  // Reset for the next interaction
                }
                else
                {
                    RotateToSide(triggeredSide);
                }
            }
        }
    }

    void RotateToSide(int targetSide)
    {
        int rotationDifference = targetSide - currentSide;

        // Handle rotation wrapping around the hexagon
        if (rotationDifference > 3)
        {
            rotationDifference -= totalSides;
            Debug.Log("Adjusting for wrap-around rotation to the left.");
        }
        else if (rotationDifference < -3)
        {
            rotationDifference += totalSides;
            Debug.Log("Adjusting for wrap-around rotation to the right.");
        }

        // Log the rotation action
        Debug.Log($"Rotating Parent Object by {-60f * rotationDifference} degrees on Z axis.");

        // Rotate the parent object
        parentObject.Rotate(0, 0, -60f * rotationDifference);

        // Update the current side
        Debug.Log($"Updating current side from {currentSide} to {targetSide}.");
        currentSide = targetSide;
    }
}
