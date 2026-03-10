using UnityEngine;

public class Valve : MonoBehaviour
{
    [Header("Valve ID (1,2,3)")]
    public int valveID;

    [Header("Puzzle Manager")]
    public ValvePuzzleManager puzzleManager;

    [Header("Rotation Settings")]
    public float rotationSpeed = 200f;

    [Header("Highlight Settings")]
    public Material highlightMaterial;

    [Header("Liquid Settings")]
    public SkinnedMeshRenderer liquidMesh;
    public int blendShapeIndex = 0;

    [Header("Audio Settings")]
    public AudioSource rotationAudio;

    private Material originalMaterial;
    private Renderer rend;
    private bool isRotating = false;
    private float targetRotation = 0f;
    private float currentRotation = 0f;
    private float liquidLevel = 0f;
    private Quaternion baseRotation;


    public bool IsHighlighted { get; private set; } = false;

    void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
        if (rend != null)
            originalMaterial = rend.material;

        baseRotation = transform.localRotation;
    }

    void Update()
    {
        SmoothRotate();
    }

    public void OnRaycastFocused()
    {
        Highlight(true);
    }

   
    public void OnRaycastUnfocused()
    {
        Highlight(false);
    }

    public void HandleScrollInput()
    {
        if (isRotating) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
            AddRotationStep(90f);
        if (scroll < 0f)
            AddRotationStep(-90f);
    }

    void AddRotationStep(float angle)
    {
        targetRotation += angle;
        targetRotation = Mathf.Repeat(targetRotation, 360f);

        if (rotationAudio != null)
        {
            rotationAudio.Stop();
            rotationAudio.Play();
        }

        if (puzzleManager != null)
            puzzleManager.RegisterValveTurn(valveID);

        isRotating = true;
    }

    void SmoothRotate()
    {
        if (!isRotating) return;

        currentRotation = Mathf.MoveTowardsAngle(
            currentRotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        transform.localRotation = baseRotation * Quaternion.Euler(0f, currentRotation, 0f);

        if (Mathf.Abs(currentRotation - targetRotation) < 0.1f)
        {
            currentRotation = targetRotation;
            isRotating = false;
        }
    }

    public void SetLiquidLevel(int tankValue, int maxAmount)
    {
        liquidLevel = (float)tankValue / maxAmount * 100f;
        UpdateLiquid();
    }

    public void ResetValve()
    {
        currentRotation = 0f;
        targetRotation = 0f;
        transform.localRotation = baseRotation;
        liquidLevel = 0f;
        UpdateLiquid();
        Highlight(false);
    }

    void UpdateLiquid()
    {
        if (liquidMesh != null)
            liquidMesh.SetBlendShapeWeight(blendShapeIndex, liquidLevel);
    }

    void Highlight(bool state)
    {
        IsHighlighted = state;
        if (rend == null || highlightMaterial == null) return;

        if (state)
            rend.material = highlightMaterial;
        else
            rend.material = originalMaterial;
    }
}