using UnityEngine;

public class Pickup : MonoBehaviour
{
    public static bool hasKey = false;

    [Header("UI")]
    public GameObject pickupPanel;

    [Header("Hold Position (Inside Paw)")]
    public Transform holdPoint;

    [Header("hammer Prefab")]
    public GameObject hammerPrefab;

    private GameObject currentKey;
    private GameObject heldKey;
    private bool playerNearKey = false;

    void Start()
    {
        pickupPanel.SetActive(false);
    }

    void Update()
    {
        // PICKUP
        if (playerNearKey && !hasKey && Input.GetKeyDown(KeyCode.K))
        {
            PickupKey();
        }

        // DROP
        if (hasKey && Input.GetKeyDown(KeyCode.P))
        {
            DropKey();
        }
    }

    void PickupKey()
    {
        hasKey = true;
        pickupPanel.SetActive(false);

        if (currentKey != null)
            Destroy(currentKey);

        // Spawn in hand
        heldKey = Instantiate(hammerPrefab, holdPoint.position, holdPoint.rotation);
        heldKey.transform.SetParent(holdPoint);

        Rigidbody rb = heldKey.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    void DropKey()
{
    if (heldKey == null) return;

    hasKey = false;

    heldKey.transform.SetParent(null);

    float radius = 1.5f;
    Vector2 randomCircle = Random.insideUnitCircle * radius;

    Vector3 startPos = new Vector3(
        transform.position.x + randomCircle.x,
        transform.position.y + 2f,
        transform.position.z + randomCircle.y
    );

    RaycastHit hit;
    if (Physics.Raycast(startPos, Vector3.down, out hit, 5f))
    {
        heldKey.transform.position = hit.point;
    }
    else
    {
        heldKey.transform.position = startPos;
    }

    heldKey.transform.rotation = Quaternion.identity;

    Rigidbody rb = heldKey.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    heldKey = null;
}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("hammer"))
        {
            playerNearKey = true;
            currentKey = other.gameObject;
            pickupPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("hammer"))
        {
            playerNearKey = false;
            pickupPanel.SetActive(false);
        }
    }
}
