using UnityEngine;

public class RotationWatcher : MonoBehaviour
{
    public Transform targetObject; // The object whose rotation we're watching
    public AudioSource audioSource; // The audio source that will play the sound effect

    private float lastZRotation; // Stores the last known Z rotation of the target object

    void Start()
    {
        if (targetObject != null)
        {
            lastZRotation = targetObject.eulerAngles.z; // Initialize with the current Z rotation
        }

        if (audioSource != null)
        {
            audioSource.loop = true; // Enable looping so it can continuously play while rotating
        }
    }

    void Update()
    {
        if (targetObject != null)
        {
            float currentZRotation = targetObject.eulerAngles.z;

            // Check if the rotation is changing
            if (currentZRotation != lastZRotation)
            {
                // Play the audio if it's not already playing
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                // Stop the audio if rotation is no longer changing
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }

            lastZRotation = currentZRotation; // Update the last known rotation
        }
    }
}
