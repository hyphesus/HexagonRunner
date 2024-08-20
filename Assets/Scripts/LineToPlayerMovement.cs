using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineToPlayerMovement : MonoBehaviour
{
    public Transform parentObject;  // The empty parent object
    public float rotationDuration = 1f;  // Duration of the rotation in seconds
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

        // Monitor for the drawing sequence
        while (drawForPerspective.isDrawing)
        {
            yield return null;  // Wait for the next frame to detect triggers
        }

        // Drawing has stopped, process the triggered sides
        if (triggeredSides.Count > 0)
        {
            ProcessTriggeredSides();
        }

        triggeredSides.Clear();  // Clear the list after processing
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called");

        if (drawForPerspective.isDrawing && other.CompareTag("SideTrigger"))
        {
            Debug.Log("SideTrigger detected");
            int triggeredSide = other.GetComponent<SideTrigger>().sideIndex;
            Debug.Log($"Triggered Side: {triggeredSide}");

            // Only add the side if it's not already in the list and if it's the first or subsequent valid trigger
            if (triggeredSides.Count == 0 || (triggeredSides.Count > 0 && triggeredSides[triggeredSides.Count - 1] != triggeredSide))
            {
                triggeredSides.Add(triggeredSide);
            }
        }
    }

    private void ProcessTriggeredSides()
    {
        // Ensure the player starts from the correct side
        if (triggeredSides[0] != currentSide)
        {
            Debug.Log("Player is not on the correct side, skipping the rotation.");
            return;
        }

        for (int i = 1; i < triggeredSides.Count; i++)
        {
            int targetSide = triggeredSides[i];
            StartCoroutine(RotateToSideIncrementally(targetSide, rotationDuration));
        }
    }

    private IEnumerator RotateToSideIncrementally(int targetSide, float duration)
    {
        float rotationDifference = targetSide - currentSide;

        // Calculate the shortest rotation path (clockwise or counterclockwise)
        if (rotationDifference > 3)
        {
            rotationDifference -= totalSides;
        }
        else if (rotationDifference < -3)
        {
            rotationDifference += totalSides;
        }

        // Determine the total angle to rotate
        float totalRotation = -60f * rotationDifference;  // Negative for counterclockwise
        float startRotation = parentObject.eulerAngles.z;
        float endRotation = startRotation + totalRotation;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float currentRotation = Mathf.Lerp(startRotation, endRotation, elapsedTime / duration);
            parentObject.rotation = Quaternion.Euler(0, 0, currentRotation);
            yield return null;  // Wait for the next frame
        }

        // Ensure final rotation is accurate
        parentObject.rotation = Quaternion.Euler(0, 0, endRotation);

        // Update the current side after rotation
        currentSide = targetSide;
    }
}
