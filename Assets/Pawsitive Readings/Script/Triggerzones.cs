using UnityEngine;

public class Triggerzones : MonoBehaviour
{
    [Header("Reference")]
    public ValvePuzzleManager puzzleManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (puzzleManager != null)
                puzzleManager.SetPlayerInZone(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (puzzleManager != null)
                puzzleManager.SetPlayerInZone(false);
        }
    }
}