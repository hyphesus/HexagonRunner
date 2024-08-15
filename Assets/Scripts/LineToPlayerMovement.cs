using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineToPlayerMovement : MonoBehaviour
{
    public Transform parentObject;  // The empty parent object
    public float rotationDelay = 0.5f;  // Delay between rotations for animation purposes
    public Transform[] sideTriggers;  // Array to store side trigger objects (index 0 to 5)

    private int currentSide = 0;  // Starting side index
    private int totalSides = 6;  // Total number of sides (hexagon)
    private List<int> triggeredSides = new List<int>();  // List to store triggered side indices
    private bool isDrawing = false;  // Flag to check if the player is drawing

    // Method to be called when the player starts drawing
    public void StartDrawing()
    {
        if (!isDrawing)
        {
            isDrawing = true;
            StartCoroutine(DrawingSequence());
        }
    }

    // Method to be called when the player stops drawing
    public void StopDrawing()
    {
        isDrawing = false;
    }

    private IEnumerator DrawingSequence()
    {
        triggeredSides.Clear();  // Clear the list at the start of a new drawing sequence

        while (isDrawing)
        {
            // Wait for the player to trigger sides during the drawing phase
            yield return null;  // Wait for the next frame
        }

        // Now that the drawing is complete, process the triggered sides
        ProcessTriggeredSides();
        ReassignIndices();  // Reassign the indices at the end of the drawing
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDrawing && other.CompareTag("SideTrigger"))
        {
            int triggeredSide = other.GetComponent<SideTrigger>().sideIndex;

            // Only add the side if it's not already in the list
            if (triggeredSides.Count == 0 || triggeredSides[triggeredSides.Count - 1] != triggeredSide)
            {
                triggeredSides.Add(triggeredSide);
                Debug.Log($"Triggered Side: {triggeredSide}");
            }
        }
    }

    private void ProcessTriggeredSides()
    {
        for (int i = 0; i < triggeredSides.Count; i++)
        {
            int targetSide = triggeredSides[i];
            StartCoroutine(RotateAfterDelay(targetSide, i * rotationDelay));
        }
    }

    private IEnumerator RotateAfterDelay(int targetSide, float delay)
    {
        yield return new WaitForSeconds(delay);

        RotateToSide(targetSide);
    }

    private void RotateToSide(int targetSide)
    {
        int rotationDifference = targetSide - currentSide;

        // Handle rotation wrapping around the hexagon
        if (rotationDifference > 3)
        {
            rotationDifference -= totalSides;
        }
        else if (rotationDifference < -3)
        {
            rotationDifference += totalSides;
        }

        // Rotate the parent object
        float rotationAngle = -60f * rotationDifference;
        parentObject.Rotate(0, 0, rotationAngle);

        // Update the current side after rotation
        currentSide = targetSide;
    }

    private void ReassignIndices()
    {
        // Update the indices for each side trigger based on the new orientation
        for (int i = 0; i < totalSides; i++)
        {
            int newSideIndex = (currentSide + i) % totalSides;
            sideTriggers[i].GetComponent<SideTrigger>().sideIndex = newSideIndex;
            Debug.Log($"Reassigned SideTrigger_{i} to index {newSideIndex}");
        }
    }
}
