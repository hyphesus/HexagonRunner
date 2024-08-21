using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAndRotate : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform player;
    public GameObject[] corridors; // Tüm koridorları tutan bir dizi
    public float rotationSpeed = 10f; // Rotasyon hızı

    private Vector3 startPosition;
    private bool isDrawing = false;

    void Start()
    {
        // Set initial line renderer properties
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;

        Debug.Log("Start method called");
    }

    void Update()
    {
        Debug.Log("Update method called");

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse button down");
            isDrawing = true;
            startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPosition);
        }

        if (isDrawing)
        {
            Debug.Log("Drawing");
            Vector3 endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.SetPosition(1, endPosition);
            Debug.DrawLine(startPosition, endPosition, Color.red);

            // Raycast ile çarpışma kontrolü
            RaycastHit hit;
            if (Physics.Raycast(startPosition, endPosition - startPosition, out hit))
            {
                Debug.Log("Raycast hit");
                // Çarpışma olan koridoru bul ve rotasyonu hesapla
                GameObject hitCorridor = FindNearestCorridor(hit.point);
                if (hitCorridor != null)
                {
                    Debug.Log("Nearest corridor found");
                    int rotationIndex = CalculateRotationIndex(hitCorridor);
                    StartCoroutine(RotatePlayer(rotationIndex));
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse button up");
            isDrawing = false;
            lineRenderer.positionCount = 0;
        }
    }

    // En yakın koridoru bulma
    GameObject FindNearestCorridor(Vector3 point)
    {
        Debug.Log("FindNearestCorridor method called");
        GameObject nearestCorridor = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject corridor in corridors)
        {
            Debug.Log("Checking corridor: " + corridor.name);
            float distance = Vector3.Distance(point, corridor.transform.position);
            if (distance < minDistance)
            {
                nearestCorridor = corridor;
                minDistance = distance;
            }
        }

        return nearestCorridor;
    }

    // Rotasyon indeksini hesaplama (X değerini bulma)
    int CalculateRotationIndex(GameObject corridor)
    {
        Debug.Log("CalculateRotationIndex method called");
        // Koridorun pozisyonuna göre açısal farkı hesapla
        Vector3 directionToCorridor = corridor.transform.position - player.transform.position;
        float angle = Vector3.Angle(player.transform.forward, directionToCorridor);

        // Açıyı 60 derecelik bölümlere ayır
        int rotationIndex = Mathf.RoundToInt(angle / 60f);

        // Yönü kontrol et (sağ veya sol)
        if (Vector3.Cross(player.transform.forward, directionToCorridor).y < 0)
        {
            rotationIndex = -rotationIndex;
        }

        return rotationIndex;
    }

    // Oyuncuyu döndürme
    IEnumerator RotatePlayer(int rotationIndex)
    {
        Debug.Log("RotatePlayer method called");
        Quaternion targetRotation = Quaternion.Euler(0, 60 * rotationIndex, 0);
        while (player.rotation != targetRotation)
        {
            player.rotation = Quaternion.Slerp(player.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}