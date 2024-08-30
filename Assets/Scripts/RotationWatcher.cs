using UnityEngine;

public class RotationWatcher : MonoBehaviour
{
    public Transform targetObject; // The object whose rotation we're watching
    public AudioSource audioSource; // The audio source that will play the sound effect

    private float lastZRotation; // Stores the last known Z rotation of the target object
    private float rotationDuration = 1f; // The fixed duration of the rotation
    private float maxAudioTime = 2.1f; // Max audio playback time, considering a slight offset
    private bool isRotating = false; // To check if the rotation is currently happening

    void Start()
    {
        if (targetObject != null)
        {
            lastZRotation = targetObject.eulerAngles.z; // Initialize with the current Z rotation
        }

        if (audioSource != null)
        {
            audioSource.loop = false; // Ensure the audio source is not set to loop
        }
    }

    void Update()
    {
        if (targetObject != null)
        {
            float currentZRotation = targetObject.eulerAngles.z;

            // Check if the rotation has started
            if (!isRotating && currentZRotation != lastZRotation)
            {
                isRotating = true;
                audioSource.time = 0f; // Start from the beginning of the sound
                audioSource.Play(); // Play the audio once rotation starts
            }

            // Calculate the rotation progress based on time elapsed
            float rotationProgress = Mathf.Clamp01(Time.time % rotationDuration / rotationDuration);

            // Map the rotation progress to the audio time only if the sound is playing
            if (audioSource.isPlaying)
            {
                audioSource.time = Mathf.Lerp(0f, maxAudioTime, rotationProgress);
            }

            // Stop the sound once the rotation is completed
            if (rotationProgress >= 1f)
            {
                audioSource.Stop();
                isRotating = false;
            }

            lastZRotation = currentZRotation; // Update the last known rotation
        }
    }
}
