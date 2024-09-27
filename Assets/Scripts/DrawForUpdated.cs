using UnityEngine;

public class DrawForUpdated : MonoBehaviour
{
    private bool drawing;
    private Collider drawCollider;
    private Camera mainCamera;
    public Vector3 direction { get; private set; }
    public float minDrawVelocity = 0.01f;

    public bool isDrawing { get; private set; }  // Public boolean to track if drawing is ongoing

    private void Awake()
    {
        mainCamera = Camera.main;
        drawCollider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        StopDrawing();
    }

    private void OnDisable()
    {
        StopDrawing();
    }

    private void Update()
    {
        // Dokunmatik girişleri kontrol ediyoruz
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartDrawing(touch.position);
                    break;

                case TouchPhase.Moved:
                    if (drawing)
                    {
                        ContinueDrawing(touch.position);
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    StopDrawing();
                    break;
            }
        }

#if UNITY_EDITOR
        // Fare girişleriyle test etmek için (Unity Editor veya Standalone için)
        if (Input.GetMouseButtonDown(0))
        {
            StartDrawing(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopDrawing();
        }
        else if (drawing)
        {
            ContinueDrawing(Input.mousePosition);
        }
#endif
    }

    public void StartDrawing(Vector2 inputPosition)
    {
        Vector3 newPosition = GetWorldPosition(inputPosition);
        transform.position = newPosition;

        drawing = true;
        isDrawing = true;  // Set isDrawing to true when drawing starts
        drawCollider.enabled = true;
    }

    public void StopDrawing()
    {
        drawing = false;
        isDrawing = false;  // Set isDrawing to false when drawing stops
        drawCollider.enabled = false;
    }

    public void ContinueDrawing(Vector2 inputPosition)
    {
        Vector3 newPosition = GetWorldPosition(inputPosition);
        direction = newPosition - transform.position;
        float velocity = direction.magnitude / Time.deltaTime;
        drawCollider.enabled = velocity > minDrawVelocity;

        transform.position = newPosition;
    }

    private Vector3 GetWorldPosition(Vector2 inputPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(inputPosition);
        Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, transform.position.z)); // Assuming drawing on z = 0 plane

        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return transform.position; // Default to current position if raycast fails
    }
}