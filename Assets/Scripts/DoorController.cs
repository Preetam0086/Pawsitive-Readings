using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator doorAnimator;
    public GameObject doorPanel;

    private bool playerInside = false;
    private bool doorOpened = false;

    private void Start()
    {
        doorPanel.SetActive(false);
        doorAnimator.SetBool("IsOpen", false); // closed by default
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            // Show panel only if door is not opened yet
            if (!doorOpened)
                doorPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            doorPanel.SetActive(false);

            // Auto-close when player leaves
            if (doorOpened)
            {
                doorOpened = false;
                doorAnimator.SetBool("IsOpen", false);
            }
        }
    }

    private void Update()
    {
        if (playerInside && !doorOpened && Input.GetKeyDown(KeyCode.T))
        {
            doorOpened = true;
            doorAnimator.SetBool("IsOpen", true);
            doorPanel.SetActive(false);
        }
    }
}
