using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public static bool hasKey = false;

    [Header("UI")]
    public GameObject pickupPanel;

    [Header("Held Key")]
    public GameObject heldKey;      // Key shown in hand
    public GameObject keyPrefab;    // World key prefab (same model)

    private bool playerInside = false;
    private Transform player;

    void Start()
    {
        pickupPanel.SetActive(false);
        heldKey.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            player = other.transform;
            pickupPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            pickupPanel.SetActive(false);
        }
    }

    void Update()
    {
        // PICK UP
        if (playerInside && !hasKey && Input.GetKeyDown(KeyCode.K))
        {
            hasKey = true;
            pickupPanel.SetActive(false);
            heldKey.SetActive(true);

            gameObject.SetActive(false); // hide instead of destroy
        }

        // DROP
        if (hasKey && Input.GetKeyDown(KeyCode.P))
        {
            DropKey();
        }
    }

    void DropKey()
    {
        hasKey = false;
        heldKey.SetActive(false);

        Vector3 dropPosition = player.position + player.forward * 1.2f;
        Instantiate(keyPrefab, dropPosition, Quaternion.identity);

        Destroy(gameObject); // destroy old hidden key
    }
}
