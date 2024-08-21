using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineToPlayerMovement : MonoBehaviour
{
    public Transform parentObject;  // The empty parent object
    public float rotationDuration = 1f;  // Duration of the rotation in seconds
    public Transform[] sideTriggers;  // Array to store side trigger objects (index 0 to 5)
    public float debounceTime = 0.1f;  // Time to wait before registering the next side

    private int currentSide = 0;  // Starting side index
    private int totalSides = 6;  // Total number of sides (hexagon)
    private List<int> triggeredSides = new List<int>();  // List to store triggered side indices
    private DrawForPerspective drawForPerspective;
    private bool isProcessingRotation = false;

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
        return triggeredSides.Count > 0 || isProcessingRotation;  // Check if the coroutine is running by checking if any triggers have been stored or rotation is in progress
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
            Debug.Log("Starting ProcessTriggeredSides. Current Side: " + currentSide);
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

            // Avoid registering the same side too quickly
            if (triggeredSides.Count == 0 || (triggeredSides.Count > 0 && triggeredSides[triggeredSides.Count - 1] != triggeredSide))
            {
                Debug.Log($"Added side {triggeredSide} to triggeredSides.");
                triggeredSides.Add(triggeredSide);
                StartCoroutine(DebounceNextTrigger());
            }
            else
            {
                Debug.Log($"Side {triggeredSide} already added, skipping.");
            }
        }
    }

    private IEnumerator DebounceNextTrigger()
    {
        yield return new WaitForSeconds(debounceTime);  // Wait to avoid double registration of the same side
    }

    private void ProcessTriggeredSides()
    {
        if (triggeredSides.Count > 1 && !isProcessingRotation)
        {
            Debug.Log("Processing Triggered Sides:");
            for (int i = 0; i < triggeredSides.Count; i++)
            {
                Debug.Log($"Side {i}: {triggeredSides[i]}");
            }

            // Ensure the player starts from the correct side
            if (triggeredSides[0] != currentSide)
            {
                Debug.Log("Player is not on the correct side, skipping the rotation.");
                return;
            }

            StartCoroutine(SequentialRotations());
        }
    }

    private IEnumerator SequentialRotations()
    {
        isProcessingRotation = true;

        for (int i = 1; i < triggeredSides.Count; i++)
        {
            int startSide = triggeredSides[i - 1];
            int targetSide = triggeredSides[i];
            Debug.Log($"About to rotate from {startSide} to {targetSide}");
            yield return StartCoroutine(RotateToSideIncrementally(startSide, targetSide, rotationDuration));
        }

        Debug.Log("Finished all rotations in sequence.");
        isProcessingRotation = false;
    }

    private IEnumerator RotateToSideIncrementally(int startSide, int targetSide, float duration)
    {
        Debug.Log($"StartSide: {startSide}, TargetSide: {targetSide}");

        float rotationDifference = targetSide - startSide;

        // Calculate the shortest rotation path (clockwise or counterclockwise)
        if (rotationDifference > 3)
        {
            rotationDifference -= totalSides;
        }
        else if (rotationDifference < -3)
        {
            rotationDifference += totalSides;
        }

        Debug.Log($"Rotation Difference: {rotationDifference}");

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

        // Snap to the exact target rotation
        parentObject.rotation = Quaternion.Euler(0, 0, Mathf.Round(endRotation / 60f) * 60f);

        // Update the current side after rotation
        currentSide = targetSide;
        Debug.Log($"Updated Current Side to: {currentSide}");
    }
}
