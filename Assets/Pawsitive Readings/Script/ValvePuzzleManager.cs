using UnityEngine;
using UnityEngine.UI;

public class ValvePuzzleManager : MonoBehaviour
{
    [Header("Tank Values")]
    public int tankA;
    public int tankB;
    public int tankC;

    [Header("Targets")]
    public int targetA = 5;
    public int targetB = 4;
    public int targetC = 1;

    [Header("Settings")]
    public int maxAmount = 10;

    [Header("Valve References")]
    public Valve valve1;
    public Valve valve2;
    public Valve valve3;

    [Header("Elevator")]
    public SkinnedMeshRenderer elevatorRenderer;
    public int blendShapeIndex = 0;
    public float openValue = 100f;

    [Header("Raycast")]
    public Camera playerCamera;
    public float interactRange = 3f;
    public LayerMask valveLayerMask;

    [Header("Crosshair")]
    public Image crosshairImage;

    [Header("Status Light")]
    public StatusLight statusLight;

    bool playerInZone = false;
    bool puzzleSolved = false;

    Valve currentFocusedValve;

    void Start()
    {
        ResetPuzzle();

        if (crosshairImage != null)
            crosshairImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (puzzleSolved) return;

        if (playerInZone)
            DoRaycast();
        else
            ClearFocus();
    }

    public void SetPlayerInZone(bool inZone)
    {
        playerInZone = inZone;

        if (crosshairImage != null)
            crosshairImage.gameObject.SetActive(inZone);

        if (!inZone)
            ClearFocus();
    }

    void DoRaycast()
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, valveLayerMask))
        {
            Valve hitValve = hit.collider.GetComponentInParent<Valve>();

            if (hitValve != null)
            {
                if (currentFocusedValve != hitValve)
                {
                    ClearFocus();

                    currentFocusedValve = hitValve;
                    currentFocusedValve.OnRaycastFocused();
                }

                currentFocusedValve.HandleKeyInput();
                return;
            }
        }

        ClearFocus();
    }

    void ClearFocus()
    {
        if (currentFocusedValve != null)
        {
            currentFocusedValve.OnRaycastUnfocused();
            currentFocusedValve = null;
        }
    }

    public void RegisterValveTurn(int valveID)
    {
        if (puzzleSolved) return;

        switch (valveID)
        {
            case 1:
                tankA += 1;
                tankB += 1;
                break;

            case 2:
                tankA += 2;
                break;

            case 3:
                tankB += 1;
                tankC += 1;
                break;
        }

        Debug.Log($"Tank Values ? A:{tankA} B:{tankB} C:{tankC}");

        UpdateLiquids();

        CheckState();
    }

    void UpdateLiquids()
    {
        valve1.SetLiquidLevel(tankA, maxAmount);
        valve2.SetLiquidLevel(tankB, maxAmount);
        valve3.SetLiquidLevel(tankC, maxAmount);
    }

    void CheckState()
    {
        if (tankA == targetA &&
            tankB == targetB &&
            tankC == targetC)
        {
            SolvePuzzle();
            return;
        }

        if (tankA > maxAmount ||
            tankB > maxAmount ||
            tankC > maxAmount)
        {
            Debug.Log("Overflow!");

            statusLight.OnPuzzleFailed();

            ResetPuzzle();
        }
    }

    void SolvePuzzle()
    {
        puzzleSolved = true;

        Debug.Log("PUZZLE SOLVED");

        statusLight.OnPuzzleSolved();

        if (elevatorRenderer != null)
            elevatorRenderer.SetBlendShapeWeight(
                blendShapeIndex,
                openValue);
    }

    void ResetPuzzle()
    {
        tankA = 0;
        tankB = 0;
        tankC = 0;

        UpdateLiquids();

        valve1.ResetValve();
        valve2.ResetValve();
        valve3.ResetValve();
    }
}