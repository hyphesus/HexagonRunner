using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineToPlayerMovement : MonoBehaviour
{
    //[SerializeField] private SfxController sfxController;
    public Transform parentObject;
    public float rotationDuration = 1f;
    public Transform[] sideTriggers;
    public float debounceTime = 0.1f;


    private int currentSide = 0;
    private int totalSides = 6;
    private List<int> triggeredSides = new List<int>();
    private Queue<int[]> rotationQueue = new Queue<int[]>();
    private DrawForPerspective drawForPerspective;
    private bool isProcessingRotation = false;



    

    // Initializes the reference to DrawForPerspective
    private void Start()
    {
        drawForPerspective = GetComponent<DrawForPerspective>();
    }
    //Starts drawing sequence or processes the rotation queue based on conditions
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
    //Checks if any drawing or rotation coroutine is currently running
    private bool IsDrawingCoroutineRunning()
    {
        return triggeredSides.Count > 0 || isProcessingRotation;
    }
    //Clears and processes the triggered sides after drawing stops
    private IEnumerator StartDrawingSequence()
    {
        triggeredSides.Clear();

        while (drawForPerspective.isDrawing)
        {
            yield return null;
        }

        if (triggeredSides.Count > 0)
        {
            ProcessTriggeredSides();
        }

        triggeredSides.Clear();
    }
    //Detects triggers and manages the list of triggered sides
    private void OnTriggerEnter(Collider other)
    {
        if (drawForPerspective.isDrawing && other.CompareTag("SideTrigger"))
        {
            int triggeredSide = other.GetComponent<SideTrigger>().sideIndex;

            if (triggeredSides.Count == 0 || (triggeredSides.Count > 0 && triggeredSides[triggeredSides.Count - 1] != triggeredSide))
            {
                triggeredSides.Add(triggeredSide);

                if (triggeredSides.Count == 2)
                {
                    rotationQueue.Enqueue(new int[] { triggeredSides[0], triggeredSides[1] });
                    triggeredSides.RemoveAt(0);
                }

                StartCoroutine(DebounceNextTrigger());
            }
        }
    }

    //Adds a short delay between registering the same side
    private IEnumerator DebounceNextTrigger()
    {
        yield return new WaitForSeconds(debounceTime);
    }
    //Enqueues remaining sides for rotation processing
    private void ProcessTriggeredSides()
    {
        if (triggeredSides.Count > 1)
        {
            rotationQueue.Enqueue(new int[] { triggeredSides[0], triggeredSides[1] });
        }
    }
    //Processes queued rotations sequentially        
    private IEnumerator ProcessRotationQueue()
    {
        isProcessingRotation = true;

        while (rotationQueue.Count > 0)
        {
            int[] rotationPair = rotationQueue.Dequeue();
            int startSide = rotationPair[0];
            int targetSide = rotationPair[1];

            if (startSide != currentSide)
            {
                continue;
            }

            yield return StartCoroutine(RotateToSideIncrementally(startSide, targetSide, rotationDuration));
        }

        isProcessingRotation = false;
    }
    //Rotates the player object from the start side to the target side.
    private IEnumerator RotateToSideIncrementally(int startSide, int targetSide, float duration)
    {
        float rotationDifference = targetSide - startSide;
       // sfxController.PlaySwapSfx();

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
        currentSide = targetSide;
    }

   

    public void ResetCurrentSide()
    {
        currentSide = 0;
    }
}
