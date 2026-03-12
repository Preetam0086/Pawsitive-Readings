using UnityEngine;

public class Valve : MonoBehaviour
{
    [Header("Valve ID (1,2,3)")]
    public int valveID;

    [Header("Puzzle Manager")]
    public ValvePuzzleManager puzzleManager;

    [Header("Rotation Settings")]
    public float rotationSpeed = 200f;

    [Header("Highlight")]
    public Material highlightMaterial;

    [Header("Liquid")]
    public SkinnedMeshRenderer liquidMesh;
    public int blendShapeIndex = 0;

    [Header("Audio")]
    public AudioSource rotationAudio;

    Renderer rend;
    Material originalMaterial;

    bool isRotating = false;
    float targetRotation = 0f;
    float currentRotation = 0f;

    float liquidLevel = 0f;
    Quaternion baseRotation;

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

    public void HandleKeyInput()
    {
        if (isRotating) return;

        // LEFT MOUSE BUTTON
        if (Input.GetMouseButtonDown(0))
        {
            AddRotationStep(90f);
        }
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

        transform.localRotation =
            baseRotation * Quaternion.Euler(0f, currentRotation, 0f);

        if (Mathf.Abs(currentRotation - targetRotation) < 0.1f)
        {
            currentRotation = targetRotation;
            isRotating = false;
        }
    }

    public void SetLiquidLevel(int tankValue, int maxAmount)
    {
        liquidLevel = (float)tankValue / maxAmount * 100f;

        if (liquidMesh != null)
            liquidMesh.SetBlendShapeWeight(blendShapeIndex, liquidLevel);
    }

    public void ResetValve()
    {
        currentRotation = 0;
        targetRotation = 0;

        transform.localRotation = baseRotation;

        liquidLevel = 0;

        if (liquidMesh != null)
            liquidMesh.SetBlendShapeWeight(blendShapeIndex, liquidLevel);

        Highlight(false);
    }

    void Highlight(bool state)
    {
        if (rend == null || highlightMaterial == null) return;

        rend.material = state ? highlightMaterial : originalMaterial;
    }
}