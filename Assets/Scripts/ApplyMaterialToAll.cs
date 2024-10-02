using UnityEngine;

public class ApplyMaterialToAll : MonoBehaviour
{
    public Material newMaterial;  // Assign this material from the inspector

    void Start()
    {
        // Find all objects in the scene with a Renderer component
        Renderer[] allRenderers = FindObjectsOfType<Renderer>();

        // Loop through all the renderers and apply the new material
        foreach (Renderer rend in allRenderers)
        {
            rend.material = newMaterial;  // Apply the new material
        }

        Debug.Log("Material applied to all objects with a Renderer.");
    }
}
