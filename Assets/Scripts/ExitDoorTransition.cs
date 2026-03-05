using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorTransition : MonoBehaviour
{
    public GameObject unlockPanel;        // UI: "Press U to unlock"
    public int spawnIndexAfterReturn = 1; // which room in Scene 1

    private bool playerInside = false;

    // Static data survives scene change
    public static bool hasTransitioned = false;
    public static int nextSpawnIndex = 0;

    void Start()
    {
        unlockPanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && KeyPickup.hasKey)
        {
            playerInside = true;
            unlockPanel.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            unlockPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.U))
        {
            hasTransitioned = true;
            nextSpawnIndex = spawnIndexAfterReturn;

            SceneManager.LoadScene("Scene2");
        }
    }
}
