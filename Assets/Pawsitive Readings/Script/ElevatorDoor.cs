using UnityEngine;

public class ElevatorDoor : MonoBehaviour
{
    [Header("Door Settings")]
    public SkinnedMeshRenderer doorRenderer;
    public int blendShapeIndex = 0;
    public float openValue = 100f;
    public float closeValue = 0f;
    public float animationSpeed = 2f;

    [Header("Auto Close Delay")]
    public float autoCloseDelay = 2f; // seconds before door closes after player enters

    private float currentValue = 0f;
    private float targetValue = 0f;
    private bool playerInside = false;
    private float closeTimer = 0f;
    private bool timerRunning = false;

    void Start()
    {
        // Start with door OPEN so player can enter
        currentValue = openValue;
        targetValue = openValue;
        if (doorRenderer != null)
            doorRenderer.SetBlendShapeWeight(blendShapeIndex, openValue);
    }

    void Update()
    {
        // Handle auto close timer
        if (timerRunning)
        {
            closeTimer -= Time.deltaTime;
            if (closeTimer <= 0f)
            {
                timerRunning = false;
                targetValue = closeValue; // Close door after delay
            }
        }

        // Smoothly animate blend shape
        currentValue = Mathf.MoveTowards(currentValue, targetValue,
                        animationSpeed * 100f * Time.deltaTime);

        if (doorRenderer != null)
            doorRenderer.SetBlendShapeWeight(blendShapeIndex, currentValue);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            timerRunning = false; // Cancel any close timer
            targetValue = openValue; // Make sure door is open when approaching
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            // Start timer to close door after player walks through
            closeTimer = autoCloseDelay;
            timerRunning = true;
        }
    }
}