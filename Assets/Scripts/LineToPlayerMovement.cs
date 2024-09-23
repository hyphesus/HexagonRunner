using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineToPlayerMovement : MonoBehaviour
{
    // DRAW İLE DÖNDÜRME İÇİN: DrawALine nesnesindeki SwipeMovement scriptini ve drawForUpdated scriptini sil, drawForPerspective ve LineToPlayerMovement ekle, atamaları yap ve çalıştır.
    [SerializeField] private SfxController sfxController;
    public Transform parentObject;
    public float rotationDuration = 1f;
    public Transform[] sideTriggers;
    public float debounceTime = 0.1f;

    private int currentSide = 0;
    private int totalSides = 6;
    private Queue<int[]> rotationQueue = new Queue<int[]>();
    private DrawForPerspective drawForPerspective;
    private bool isProcessingRotation = false;

    // Initializes the reference to DrawForPerspective
    private void Start()
    {
        drawForPerspective = GetComponent<DrawForPerspective>();
    }

    // Processes the rotation queue based on conditions
    private void Update()
    {
        if (!isProcessingRotation && rotationQueue.Count > 0)
        {
            StartCoroutine(ProcessRotationQueue());
        }
    }

    // Detects triggers and enqueues rotation from current side to triggered side
    private void OnTriggerEnter(Collider other)
    {
        if (drawForPerspective.isDrawing && other.CompareTag("SideTrigger"))
        {
            int triggeredSide = other.GetComponent<SideTrigger>().sideIndex;

            rotationQueue.Enqueue(new int[] { currentSide, triggeredSide });

            StartCoroutine(DebounceNextTrigger());
        }
    }

    // Adds a short delay between registering the same side
    private IEnumerator DebounceNextTrigger()
    {
        yield return new WaitForSeconds(debounceTime);
    }

    // Processes queued rotations sequentially        
    private IEnumerator ProcessRotationQueue()
    {
        isProcessingRotation = true;

        while (rotationQueue.Count > 0)
        {
            int[] rotationPair = rotationQueue.Dequeue();
            int targetSide = rotationPair[1];

            yield return StartCoroutine(RotateToSideIncrementally(currentSide, targetSide, rotationDuration));
        }

        isProcessingRotation = false;
    }

    // Rotates the player object from the current side to the target side.
    private IEnumerator RotateToSideIncrementally(int currentSide, int targetSide, float duration)
    {
        float rotationDifference = targetSide - currentSide;
        sfxController.PlaySwapSfx();

        if (rotationDifference > 3)
        {
            rotationDifference -= totalSides;
        }
        else if (rotationDifference < -3)
        {
            rotationDifference += totalSides;
        }

        float totalRotation = -60f * rotationDifference;
        float startRotation = parentObject.eulerAngles.z;
        float endRotation = startRotation + totalRotation;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float currentRotation = Mathf.Lerp(startRotation, endRotation, elapsedTime / duration);
            parentObject.rotation = Quaternion.Euler(0, 0, currentRotation);

            yield return null;
        }

        parentObject.rotation = Quaternion.Euler(0, 0, Mathf.Round(endRotation / 60f) * 60f);
        this.currentSide = targetSide;
    }

    public void ResetCurrentSide()
    {
        currentSide = 0;
    }
}
