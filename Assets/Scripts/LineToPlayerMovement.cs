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

    private DrawForPerspective drawForPerspective;

    private void Start()
    {
        drawForPerspective = GetComponent<DrawForPerspective>();  // Get reference to the DrawForPerspective script
    }

    private void Update()
    {
        if (drawForPerspective.isDrawing && !IsDrawingCoroutineRunning())
        {
            StartCoroutine(StartDrawingSequence());
        }
    }

    private bool IsDrawingCoroutineRunning()
    {
        return triggeredSides.Count > 0;  // Check if the coroutine is running by checking if any triggers have been stored
    }

    private IEnumerator StartDrawingSequence()
    {
        triggeredSides.Clear();  // Clear the list at the start of a new drawing sequence

        while (drawForPerspective.isDrawing)
        {
            yield return null;  // Wait for the next frame to detect triggers
        }

        // Drawing has stopped, process the triggered sides
        ProcessTriggeredSides();
        ReassignIndices();  // Reassign the indices at the end of the drawing
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called");

        if (drawForPerspective.isDrawing && other.CompareTag("SideTrigger"))
        {
            Debug.Log("SideTrigger detected");
            int triggeredSide = other.GetComponent<SideTrigger>().sideIndex;
            Debug.Log($"Triggered Side: {triggeredSide}");

            // Only add the side if it's not already in the list
            if (triggeredSides.Count == 0 || triggeredSides[triggeredSides.Count - 1] != triggeredSide)
            {
                triggeredSides.Add(triggeredSide);
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
