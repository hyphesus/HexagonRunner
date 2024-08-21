using UnityEngine;

public class DrawForPerspective : MonoBehaviour
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
        if (Input.GetMouseButtonDown(0))
        {
            StartDrawing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopDrawing();
        }
        else if (drawing)
        {
            ContinueDrawing();
        }
    }

    public void StartDrawing()
    {
        Vector3 newPosition = GetMouseWorldPosition();
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

    public void ContinueDrawing()
    {
        Vector3 newPosition = GetMouseWorldPosition();
        direction = newPosition - transform.position;
        float velocity = direction.magnitude / Time.deltaTime;
        drawCollider.enabled = velocity > minDrawVelocity;

        transform.position = newPosition;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, transform.position.z)); // Assuming drawing on z = 0 plane

        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return transform.position; // Default to current position if raycast fails
    }
}