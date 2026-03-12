using UnityEngine;

public class FPPMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 1.8f;
    public float gravity = -9.81f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 150f;
    public Transform playerCamera;

    [Header("Paw Animation")]
    public Animator leftPawAnimator;
    public Animator rightPawAnimator;

    [Header("Head Bob")]
    public float bobSpeed = 6f;
    public float bobAmount = 0.05f;

    private float defaultCamY;
    private float bobTimer;

    [Header("Footstep Audio")]
    public AudioSource footstepAudio;
    public float stepInterval = 0.5f;

    private float stepTimer;

    private float xRotation = 0f;
    private Vector3 velocity;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        defaultCamY = playerCamera.localPosition.y;

        if (footstepAudio != null)
            footstepAudio.Stop();
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleHeadBob();
        HandleFootsteps();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        bool isMoving = Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f;

        if (leftPawAnimator != null)
            leftPawAnimator.SetBool("isMoving", isMoving);

        if (rightPawAnimator != null)
            rightPawAnimator.SetBool("isMoving", isMoving);
    }

    void HandleHeadBob()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isMoving = Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f;

        if (controller.isGrounded && isMoving)
        {
            bobTimer += Time.deltaTime * bobSpeed;

            float newY = defaultCamY + Mathf.Sin(bobTimer) * bobAmount;

            playerCamera.localPosition = new Vector3(
                playerCamera.localPosition.x,
                newY,
                playerCamera.localPosition.z
            );
        }
        else
        {
            bobTimer = 0;

            playerCamera.localPosition = new Vector3(
                playerCamera.localPosition.x,
                Mathf.Lerp(playerCamera.localPosition.y, defaultCamY, Time.deltaTime * 5f),
                playerCamera.localPosition.z
            );
        }
    }

    void HandleFootsteps()
    {
        if (footstepAudio == null) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isMoving = Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f;

        if (controller.isGrounded && isMoving)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                footstepAudio.pitch = Random.Range(0.95f, 1.05f);
                footstepAudio.Play();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }
}