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
    private Queue<int[]> rotationQueue = new Queue<int[]>();  // Queue to store pairs of sides for rotation
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

        if (!isProcessingRotation && rotationQueue.Count > 0)
        {
            StartCoroutine(ProcessRotationQueue());
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

        // Drawing has stopped, process the remaining triggered sides
        if (triggeredSides.Count > 0)
        {
            ProcessTriggeredSides();
        }

        triggeredSides.Clear();  // Clear the list after processing
    }

    private void OnTriggerEnter(Collider other)
    {
        if (drawForPerspective.isDrawing && other.CompareTag("SideTrigger"))
        {
            int triggeredSide = other.GetComponent<SideTrigger>().sideIndex;

            // Avoid registering the same side too quickly
            if (triggeredSides.Count == 0 || (triggeredSides.Count > 0 && triggeredSides[triggeredSides.Count - 1] != triggeredSide))
            {
                triggeredSides.Add(triggeredSide);

                if (triggeredSides.Count == 2)
                {
                    // Add the pair to the queue
                    rotationQueue.Enqueue(new int[] { triggeredSides[0], triggeredSides[1] });

                    // Remove the first element to make room for the next trigger
                    triggeredSides.RemoveAt(0);
                }

                StartCoroutine(DebounceNextTrigger());
            }
        }
    }

    private IEnumerator DebounceNextTrigger()
    {
        yield return new WaitForSeconds(debounceTime);  // Wait to avoid double registration of the same side
    }

    private void ProcessTriggeredSides()
    {
        if (triggeredSides.Count > 1)
        {
            // If there are remaining sides after the drawing stops, process them
            rotationQueue.Enqueue(new int[] { triggeredSides[0], triggeredSides[1] });
        }
    }

    private IEnumerator ProcessRotationQueue()
    {
        isProcessingRotation = true;

        while (rotationQueue.Count > 0)
        {
            int[] rotationPair = rotationQueue.Dequeue();
            int startSide = rotationPair[0];
            int targetSide = rotationPair[1];

            // Check if the player is on the correct start side before rotating
            if (startSide != currentSide)
            {
                continue;  // Skip this rotation and move to the next in the queue
            }

            yield return StartCoroutine(RotateToSideIncrementally(startSide, targetSide, rotationDuration));
        }

        isProcessingRotation = false;
    }

    private IEnumerator RotateToSideIncrementally(int startSide, int targetSide, float duration)
    {
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
    }
}
