using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeMovement : MonoBehaviour
{
    [SerializeField] private SfxController sfxController;
    public Transform parentObject;
    public float rotationDuration = 0.5f; // Döndürme süresi
    public float minSwipeDistance = 50f; // Swipe'ın minimum mesafesi
    public float debounceTime = 0.1f;

    private Queue<float> rotationQueue = new Queue<float>();
    private DrawForPerspective drawForPerspective;
    private bool isProcessingRotation = false;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isSwiping = false;

    // DrawForPerspective referansını başlatır
    private void Start()
    {
        drawForPerspective = GetComponent<DrawForPerspective>();
    }

    // Swipe algılama ve dönüş kuyruğunu işler
    private void Update()
    {
        // Swipe algılama
        if (Input.touchCount > 0 && drawForPerspective.isDrawing)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isSwiping = true;
                    startTouchPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    endTouchPosition = touch.position;
                    if (isSwiping)
                    {
                        DetectSwipe();
                    }
                    isSwiping = false;
                    break;
            }
        }

#if UNITY_EDITOR
        // Unity Editor'da fare girişi ile test etmek için
        if (Input.GetMouseButtonDown(0) && drawForPerspective.isDrawing)
        {
            isSwiping = true;
            startTouchPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            endTouchPosition = Input.mousePosition;
            DetectSwipe();
            isSwiping = false;
        }
#endif

        if (!isProcessingRotation && rotationQueue.Count > 0)
        {
            StartCoroutine(ProcessRotationQueue());
        }
    }

    // Swipe yönünü algılar ve dönüşü kuyruğa ekler
    private void DetectSwipe()
    {
        float swipeDistance = (endTouchPosition - startTouchPosition).magnitude;

        if (swipeDistance >= minSwipeDistance)
        {
            Vector2 swipeDirection = endTouchPosition - startTouchPosition;
            float x = swipeDirection.x;
            float y = swipeDirection.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                // Yatay swipe
                if (x > 0)
                {
                    // Sağa swipe
                    OnSwipeRight();
                }
                else
                {
                    // Sola swipe
                    OnSwipeLeft();
                }
            }

            StartCoroutine(DebounceNextSwipe());
        }
    }

    // Swipe'lar arasında kısa bir bekleme ekler
    private IEnumerator DebounceNextSwipe()
    {
        isSwiping = false;
        yield return new WaitForSeconds(debounceTime);
    }

    // Sağa swipe olduğunda +60 derece döndürmeyi kuyruğa ekler
    private void OnSwipeRight()
    {
        rotationQueue.Enqueue(60f);
    }

    // Sola swipe olduğunda -60 derece döndürmeyi kuyruğa ekler
    private void OnSwipeLeft()
    {
        rotationQueue.Enqueue(-60f);
    }

    // Dönüş kuyruğunu sırayla işler
    private IEnumerator ProcessRotationQueue()
    {
        isProcessingRotation = true;

        while (rotationQueue.Count > 0)
        {
            float rotationAngle = rotationQueue.Dequeue();
            yield return StartCoroutine(RotateByAngle(rotationAngle, rotationDuration));
        }

        isProcessingRotation = false;
    }

    // Nesneyi belirtilen açı kadar yumuşak bir şekilde döndürür
    private IEnumerator RotateByAngle(float angle, float duration)
    {
        sfxController.PlaySwapSfx();

        float startRotation = parentObject.eulerAngles.z;
        float endRotation = startRotation + angle;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float currentRotation = Mathf.Lerp(startRotation, endRotation, elapsedTime / duration);
            parentObject.rotation = Quaternion.Euler(0, 0, currentRotation);

            yield return null;
        }

        // Floating point hatalarını önlemek için açıyı yuvarla
        parentObject.rotation = Quaternion.Euler(0, 0, Mathf.Round(endRotation / 60f) * 60f);
    }
}
