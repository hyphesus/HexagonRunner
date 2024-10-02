using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class AssignTexture : MonoBehaviour
{
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            // Get the original texture from the object's material
            Texture originalTexture = renderer.sharedMaterial.mainTexture;

            // Create a Material Property Block
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();

            // Set the _MainTex property to the original texture
            propBlock.SetTexture("_MainTex", originalTexture);

            // Assign the property block to the renderer
            renderer.SetPropertyBlock(propBlock);
        }
    }
}
