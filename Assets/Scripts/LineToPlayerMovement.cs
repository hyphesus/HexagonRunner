using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineToPlayerMovement : MonoBehaviour
{
    public Transform parentObject;  // The empty parent object
    private int currentSide = 0;
    private int totalSides = 6;

    void OnTriggerEnter(Collider other)
    {
        // Check if the trigger is tagged correctly
        if (other.CompareTag("SideTrigger"))
        {
            int triggeredSide = other.GetComponent<SideTrigger>().sideIndex;

            Debug.Log($"Triggered Side: {triggeredSide}, Current Side: {currentSide}");

            // Determine the rotation needed
            int rotationDifference = triggeredSide - currentSide;

            if (rotationDifference == 0)
            {
                // No rotation needed, player is already on this side
                Debug.Log("No rotation needed, player is already on this side.");
                return;
            }

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
            Debug.Log($"Updating current side from {currentSide} to {triggeredSide}.");
            currentSide = triggeredSide;
        }
    }

}
