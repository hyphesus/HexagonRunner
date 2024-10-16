using UnityEngine;

public class DrawForOrthographic : MonoBehaviour
{
    private bool drawing;
    private Collider drawCollider;
    private Camera mainCamera;
    public Vector3 direction { get; private set; }
    public float minDrawVelocity = 0.01f;
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
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.nearClipPlane; // Set z to the distance from the camera to the object

        Vector3 newPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        newPosition.z = 0; // If you want to keep the z position at 0

        transform.position = newPosition;

        drawing = true;
        drawCollider.enabled = true;

        Debug.Log("Start Drawing");
    }
    public void StopDrawing()
    {
        drawing = false;
        drawCollider.enabled = false;

        Debug.Log("Stop Drawing");
    }
    public void ContinueDrawing()
    {
        Debug.Log("Continue Drawing");

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.nearClipPlane; // Set z to the distance from the camera to the object

        Vector3 newPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        newPosition.z = 0; // If you want to keep the z position at 0

        direction = newPosition - transform.position;
        float velocity = direction.magnitude / Time.deltaTime;
        drawCollider.enabled = velocity > minDrawVelocity;

        transform.position = newPosition;
    }
}
