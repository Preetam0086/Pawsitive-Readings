using UnityEngine;

public class HammerPickup : MonoBehaviour
{
    [Header("References")]
    public GameObject hammerInHand;
    public GameObject rightPaw;

    [Header("Drop Settings")]
    public float dropDistance = 1.5f;
    public float dropHeight = 0.5f;

    private bool playerNear = false;
    private bool pickedUp = false;

    private Rigidbody rb;
    private SkinnedMeshRenderer meshRenderer;
    private Collider hammerCollider;
    private Transform playerTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
        hammerCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (playerNear && !pickedUp && Input.GetKeyDown(KeyCode.P))
        {
            PickupHammer();
        }

        if (pickedUp && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Dropping hammer");
            DropHammer();
        }
    }

    void PickupHammer()
    {
        pickedUp = true;

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
        }

        if (meshRenderer != null)
            meshRenderer.enabled = false;

        if (hammerCollider != null)
            hammerCollider.enabled = false;

        if (rightPaw != null)
            rightPaw.SetActive(false);

        if (hammerInHand != null)
            hammerInHand.SetActive(true);
    }

    void DropHammer()
    {
        pickedUp = false;

        if (hammerInHand != null)
            hammerInHand.SetActive(false);

        if (rightPaw != null)
            rightPaw.SetActive(true);

        if (playerTransform != null)
        {
            Vector3 dropPos = playerTransform.position
                            + playerTransform.forward * dropDistance;

            dropPos.y = dropHeight;

            transform.position = dropPos;
        }

        if (meshRenderer != null)
            meshRenderer.enabled = true;

        if (hammerCollider != null)
            hammerCollider.enabled = true;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            playerTransform = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
        }
    }
}