using UnityEngine;
using UnityEngine.UI;

public class ValvePuzzleManager : MonoBehaviour
{
    [Header("Tank Values")]
    public int tankA;
    public int tankB;
    public int tankC;

    [Header("Target Values")]
    public int targetA = 5;
    public int targetB = 4;
    public int targetC = 1;

    [Header("Settings")]
    public int maxAmount = 10;

    [Header("Valve References")]
    public Valve valve1;
    public Valve valve2;
    public Valve valve3;

    [Header("Elevator Door")]
    public SkinnedMeshRenderer elevatorRenderer;
    public int blendShapeIndex = 0;
    public float openValue = 100f;

    [Header("Raycast Settings")]
    public Camera playerCamera;            
    public float interactRange = 3f;        
    public LayerMask valveLayerMask;        

    [Header("Crosshair Settings")]
    public Image crosshairImage;            

    public Collider valveMachineZone;       

    private bool playerInZone = false;
    private bool puzzleSolved = false;
    private Valve currentFocusedValve = null;

    void Start()
    {
        ResetPuzzle();

        // Make sure crosshair starts hidden
        if (crosshairImage != null)
            crosshairImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (puzzleSolved) return;

        CheckPlayerZone();

        if (playerInZone)
        {
            DoRaycast();
        }
        else
        {
            // Player left zone — clear everything
            ClearFocus();
            SetCrosshair(false);
        }
    }



    void CheckPlayerZone()
    {
        if (playerCamera == null) return;

        /*
        if (valveMachineZone != null)
        {
            float dist = Vector3.Distance(
                playerCamera.transform.position,
                valveMachineZone.ClosestPoint(playerCamera.transform.position)
            );
            playerInZone = dist <= interactRange;
        }
        */
    }


    public void SetPlayerInZone(bool inZone)
    {
        playerInZone = inZone;
        SetCrosshair(inZone);
        if (!inZone) ClearFocus();
    }


    void DoRaycast()
    {
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, valveLayerMask))
        {
            Valve hitValve = hit.collider.GetComponentInParent<Valve>();

            if (hitValve != null)
            {
                // Focus changed
                if (currentFocusedValve != hitValve)
                {
                    ClearFocus();
                    currentFocusedValve = hitValve;
                    currentFocusedValve.OnRaycastFocused();
                }

                currentFocusedValve.HandleScrollInput();
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

    void SetCrosshair(bool active)
    {
        if (crosshairImage != null)
            crosshairImage.gameObject.SetActive(active);
    }

    

    public void RegisterValveTurn(int valveID)
    {
        if (puzzleSolved) return;

        switch (valveID)
        {
            case 1:
                tankA += 2;
                break;
            case 2:
                tankA += 1;
                tankB += 1;
                break;
            case 3:
                tankB += 1;
                tankC += 1;
                break;
        }

        UpdateLiquids();
        CheckState();
    }

    void UpdateLiquids()
    {
        if (valve1 != null) valve1.SetLiquidLevel(tankA, maxAmount);
        if (valve2 != null) valve2.SetLiquidLevel(tankB, maxAmount);
        if (valve3 != null) valve3.SetLiquidLevel(tankC, maxAmount);
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
            Debug.Log("Overflow! Resetting puzzle...");
            ResetPuzzle();
        }
    }

    void SolvePuzzle()
    {
        puzzleSolved = true;
        Debug.Log("Puzzle Solved!");

        if (elevatorRenderer != null)
            elevatorRenderer.SetBlendShapeWeight(blendShapeIndex, openValue);
    }

    void ResetPuzzle()
    {
        tankA = 0;
        tankB = 0;
        tankC = 0;

        UpdateLiquids();

        if (valve1 != null) valve1.ResetValve();
        if (valve2 != null) valve2.ResetValve();
        if (valve3 != null) valve3.ResetValve();
    }
}