using UnityEngine;
using UnityEngine.UI;

public class SimonTrigger : MonoBehaviour
{
    [Header("Crosshair UI")]
    public GameObject crosshair;

    private void Start()
    {
        // Make sure crosshair is hidden at start
        if (crosshair != null)
            crosshair.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (crosshair != null)
                crosshair.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (crosshair != null)
                crosshair.SetActive(false);
        }
    }
}