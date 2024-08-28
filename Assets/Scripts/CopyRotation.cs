using UnityEngine;

public class CopyRotation : MonoBehaviour
{
    public Transform target; // The object to copy the rotation from

    void Update()
    {
        if (target != null)
        {
            // Copy the rotation of the target object
            transform.rotation = target.rotation;
        }
    }
}
